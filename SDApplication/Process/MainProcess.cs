using CefSharp.WinForms;
using CommandManager;
using Dal;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDApplication.Process
{
    class MainProcess
    {
        public static ChromiumWebBrowser chromeBrower;
        public static List<Equipment> mainList = new List<Equipment>();

        public static void readMain(Equipment eq)
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
            data.EquipmentID = eq.ID;            

            eq.Chroma = data.Chroma;

            // 添加数据库
            //EquipmentDataDal.AddOne(data);

            // 添加报警到数据库
            //addAlertData(eq, data);

            // 显示数据到界面
            renderOne(data, eq.Address);
        }

        private static void addAlertData(Equipment eq,  EquipmentData data) {
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

        // 显示数据到界面
        private static void renderOne(EquipmentData data, int address)
        {
            EquipmentItem item = new EquipmentItem();
            // 计算房间号
            item.roomId = (int)Math.Ceiling((double)(address / 12.00));
            item.id = address;
            Random ran = new Random();
            if (item.id % 2 == 0)
            {
                item.humidity = data.Chroma;
                item.isAlertHumidity = data.ChromaAlertStr != string.Empty && data.ChromaAlertStr != EM_AlertType.正常.ToString();
            }
            else {
                item.temperature = data.Chroma;
                item.isAlertTemperature = data.ChromaAlertStr != string.Empty && data.ChromaAlertStr != EM_AlertType.正常.ToString();
            }
            item.eqName = data.EName;

            string str = JsonConvert.SerializeObject(item);

            MainProcess.chromeBrower.GetBrowser().MainFrame.ExecuteJavaScriptAsync(string.Format(@"window.setOneData({0});", str));
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

            string str = JsonConvert.SerializeObject(list);
            Trace.WriteLine(str);
            return str;
        }
    }
}
