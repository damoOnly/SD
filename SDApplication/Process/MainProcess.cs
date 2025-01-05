using CefSharp.WinForms;
using CommandManager;
using Dal;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDApplication.Process
{
    class MainProcess
    {
        public static ChromiumWebBrowser chromeBrower;

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
            EquipmentDataDal.AddOne(data);

            // 添加报警到数据库
            addAlertData(eq, data);

            // 显示数据到界面
            renderOne(data);
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
        private static void renderOne(EquipmentData data)
        {
            string str = JsonConvert.SerializeObject(data);

            MainProcess.chromeBrower.GetBrowser().MainFrame.ExecuteJavaScriptAsync(string.Format(@"window.setOneData({0});", str));
        }
    }
}
