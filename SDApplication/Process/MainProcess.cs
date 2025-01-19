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
    }
}
