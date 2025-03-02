using CommandManager;
using Dal;
using Entity;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDApplication.Process
{
    class MainProcess
    {
        public static MainForm mainForm;
        public static WebView2 webView;
        public static List<Equipment> mainList = new List<Equipment>();

        public static DateTime lastRemoteTime = Utility.CutOffMillisecond(DateTime.Now);

        public static List<EquipmentDataWrap> equipmentDataWrapList = new List<EquipmentDataWrap>();
        public static List<EquipmentDataWrap> equipmentDataWrapListAverage = new List<EquipmentDataWrap>();

        public static void readMain(Equipment eq, DateTime dt, List<EquipmentData> tempList)
        {
            Command cd = new Command(eq.Address, 0x00, 0x00, 3);

            if (!CommandResult.GetResult(cd))
            {
                if (eq.lostNum >= 10)
                {
                    eq.IsConnect = false;
                }
                else
                {
                    eq.lostNum++;
                }
                return;
            }
            else
            {
                eq.IsConnect = true;
                eq.lostNum = 0;
            }

            EquipmentData data = Parse.GetRealData(cd.ResultByte, eq);
            // 用地址代替设备id，设备id没有用了
            data.EquipmentID = eq.Address;
            data.AddTime = dt;
            eq.Chroma = data.Chroma;

            // 添加报警到数据库
            //addAlertData(eq, data);

            // 准备用于存库的数据
            addDataOne(data, tempList);

            // 显示数据到界面
            renderOne(data);
        }

        private static void addAlertData(Equipment eq, EquipmentData data)
        {
            if (eq.AlertType == 0)
            {
                eq.ChromaAlertStr = Gloabl.NormalStr;
            }
            else
            {
                // 报警记录
                if (eq.ChromaAlertStr != data.ChromaAlertStr)
                {

                    if (eq.ChromaAlertStr.Equals(Gloabl.NormalStr, StringComparison.OrdinalIgnoreCase))
                    {
                        Alert art = new Alert();
                        art.AlertName = data.ChromaAlertStr;
                        art.EquipmentID = eq.ID;
                        eq.AlertObject = AlertDal.AddOneR(art);
                    }
                    else
                    {
                        eq.AlertObject.EndTime = DateTime.Now;
                        AlertDal.UpdateOne(eq.AlertObject);
                        if (!eq.ChromaAlertStr.Equals(data.ChromaAlertStr, StringComparison.OrdinalIgnoreCase))
                        {
                            Alert art = new Alert();
                            art.AlertName = data.ChromaAlertStr;
                            art.EquipmentID = eq.ID;
                            eq.AlertObject = AlertDal.AddOneR(art);
                        }
                    }
                    eq.ChromaAlertStr = data.ChromaAlertStr;
                }
                else
                {
                    if (!eq.ChromaAlertStr.Equals(Gloabl.NormalStr, StringComparison.OrdinalIgnoreCase))
                    {
                        eq.AlertObject.EndTime = DateTime.Now;
                        if (!AlertDal.UpdateOne(eq.AlertObject))
                        {
                            Alert art = new Alert();
                            art.AlertName = data.ChromaAlertStr;
                            art.EquipmentID = eq.ID;
                            eq.AlertObject = AlertDal.AddOneR(art);
                        }
                    }
                }
            }
        }


        // 添加数据到一次循环的临时对象中
        private static void addDataOne(EquipmentData data, List<EquipmentData> tempList)
        {
            long address = data.EquipmentID;
            bool isHumidity = address % 2 == 0;
            if (isHumidity)
            {
                address = address - 1;
            }

            // 如果没有就新增
            EquipmentData originData = tempList.FirstOrDefault(cc => cc.EquipmentID == address);
            if (null == originData)
            {
                originData = new EquipmentData();
                tempList.Add(originData);
            }
            originData.EquipmentID = address;

            if (isHumidity)
            {
                originData.humidity = data.Chroma;
            }
            else
            {
                originData.temperature = data.Chroma;
            }
        }

        // 显示数据到界面
        private static void renderOne(EquipmentData data)
        {
            EquipmentItem item = new EquipmentItem();
            // 计算房间号
            item.roomId = (int)Math.Ceiling((double)(data.EquipmentID / 12.00));
            item.id = data.EquipmentID;

            if (item.id % 2 == 0)
            {
                item.humidity = data.Chroma;
                item.isAlertHumidity = data.ChromaAlertStr != string.Empty && data.ChromaAlertStr != EM_AlertType.正常.ToString();
            }
            else
            {
                item.temperature = data.Chroma;
                item.isAlertTemperature = data.ChromaAlertStr != string.Empty && data.ChromaAlertStr != EM_AlertType.正常.ToString();
            }
            item.eqName = data.EName;

            string dataStr = JsonConvert.SerializeObject(item);
            MainProcess.postMessage("setOneData", dataStr);
        }

        public static string getRoomList()
        {
            List<RoomItem> list = new List<RoomItem>();
            MainProcess.mainList.ForEach(c =>
            {
                // id号，偶数是湿度，奇数是温度,统一用温度的id
                int id = (int)(c.Address % 2 == 0 ? c.Address - 1 : c.Address);
                //房间号
                int roomId = (int)Math.Ceiling((double)(c.Address / 12.00));
                int roomIndex = list.FindIndex(r =>
                {
                    return r.roomId == roomId;
                });

                if (roomIndex < 0)
                {
                    RoomItem item = new RoomItem();
                    item.roomId = roomId;
                    item.roomName = c.Place;
                    item.dataList = new List<EquipmentItem>();
                    // 新房间第一个肯定要新建
                    EquipmentItem eq = new EquipmentItem();
                    eq.eqName = c.EName;
                    eq.id = id;

                    item.dataList.Add(eq);
                    list.Add(item);
                }
                else
                {
                    RoomItem item = list[roomIndex];

                    int eqIndex = item.dataList.FindIndex(e => { return e.id == id; });

                    // 没有找到设备需要新建,找到了，暂时不用管，看看后面是否需要初始化数据
                    if (eqIndex < 0)
                    {
                        EquipmentItem eq = new EquipmentItem();
                        eq.id = id;
                        eq.eqName = c.EName;

                        item.dataList.Add(eq);
                    }
                }
            });

            string dataStr = JsonConvert.SerializeObject(list);

            return dataStr;
        }

        public static string getMessageStr(string type, string data)
        {
            MessageData message = new MessageData(type, data);
            string str = JsonConvert.SerializeObject(message);

            return str;
        }

        public static void postMessage(string type, string data)
        {
            string str = getMessageStr(type, data);

            MainProcess.mainForm.Invoke(new Action(() =>
            {
                if (MainProcess.webView != null && MainProcess.webView.CoreWebView2 != null)
                {
                    Trace.WriteLine(str);
                    MainProcess.webView.CoreWebView2.PostWebMessageAsJson(str);
                }
            }));


        }

        // 把数据添加到内存，并且每隔一定时间触发一次数据库存储
        public static void addWrapList(List<EquipmentData> tempList, DateTime nowTemp)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                EquipmentData origin = tempList[i];
                // 这里的设备id，其实是地址
                EquipmentDataWrap wrap = MainProcess.equipmentDataWrapList.FirstOrDefault(cc => cc.fileId == origin.EquipmentID);
                if (null == wrap) 
                {
                    wrap = new EquipmentDataWrap();
                    wrap.fileId = (int)origin.EquipmentID;
                    MainProcess.equipmentDataWrapList.Add(wrap);
                }

                wrap.list.Add(origin);

            }

            addWrapListAverage(tempList, nowTemp);

            // 一定间隔存储数据
            if (nowTemp.AddSeconds(-10) > MainProcess.lastRemoteTime)
            {
                ThreadPool.QueueUserWorkItem(MainProcess.saveData);
                MainProcess.lastRemoteTime = Utility.CutOffMillisecond(DateTime.Now);
            }
        }

        public static void saveData(object state)
        {
            foreach (var item in MainProcess.equipmentDataWrapList)
            {
                if (item.list.Count <= 0)
                {
                    continue;
                }
                EquipmentDataDal.AddList(item.list, item.fileId.ToString());
                item.list.Clear();
            }

            // 保存平局数据
            foreach (var item in MainProcess.equipmentDataWrapListAverage)
            {
                if (item.list.Count <= 0)
                {
                    continue;
                }
                EquipmentDataDalAverage.AddList(item.list, item.fileId.ToString());
                item.list.Clear();
            }
        }

        public static void addWrapListAverage(List<EquipmentData> tempList, DateTime nowTemp)
        {
            // 根据房间号计算平均值
            List<EquipmentData> list = new List<EquipmentData>();

            for (int i = 0; i < tempList.Count; i++)
            {
                EquipmentData origin = tempList[i];
                //房间号
                int roomId = (int)Math.Ceiling((double)(origin.EquipmentID / 12.00));
                EquipmentData roomData = list.FirstOrDefault(r =>
                {
                    // 用id表示room id
                    return r.ID == roomId;
                });

                if (null == roomData)
                {
                    roomData = new EquipmentData();
                    roomData.ID = roomId;
                    roomData.temperature = origin.temperature;
                    roomData.humidity = origin.humidity;
                    list.Add(roomData);
                }
                else
                {
                    //roomData.temperature = Convert.ToSingle(Math.Round((origin.temperature + roomData.temperature) / 2, 0));
                    //roomData.humidity = Convert.ToSingle(Math.Round((origin.humidity + roomData.humidity) / 2, 1));
                    roomData.temperature = (origin.temperature + roomData.temperature) / 2;
                    roomData.humidity = (origin.humidity + roomData.humidity) / 2;
                }               

            }

            // 
            foreach (var item in list)
            {
                // 这里的id和file id，是房间号
                EquipmentDataWrap wrap = MainProcess.equipmentDataWrapListAverage.FirstOrDefault(cc => cc.fileId == item.ID);
                if (null == wrap)
                {
                    wrap = new EquipmentDataWrap();
                    wrap.fileId = item.ID;
                    MainProcess.equipmentDataWrapListAverage.Add(wrap);
                }

                wrap.list.Add(item);
            }
        }
    }
}
