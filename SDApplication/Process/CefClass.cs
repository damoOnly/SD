using CefSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDApplication.Process
{
    class CefClass
    {
    }

    public class MenuHandler : IContextMenuHandler
    {

        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model)
        {
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser webBrowser, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser webBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters,
            IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }

    public class BoundObject
    {
        LogLib.Log log = LogLib.Log.GetLogger("BoundObject");
        private MainForm form = null;

        public BoundObject(MainForm _form)
        {
            this.form = _form;
        }

        public void OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //      if(e.Frame.IsMain)
            //      {
            //        browser.ExecuteScriptAsync(@"
            //          document.body.onmouseup = function()
            //          {
            //            bound.onSelected(window.getSelection().toString());
            //          }
            //        ");
            //      }
        }

        public void OnSelected(string selected)
        {
            Trace.WriteLine("The user selected some text [" + selected + "]");
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public string getRoomList()
        {
            List<RoomItem> list = new List<RoomItem>();
            MainProcess.mainList.ForEach(c => {
                // id号，偶数是湿度，奇数是温度,统一用温度的id
                int id = (int)(c.Address % 2 == 0 ? c.Address - 1 : c.Address);
                //房间号
                int roomId = (int)Math.Ceiling((double)(c.Address / 12.00));
                int roomIndex = list.FindIndex(r =>
                {
                    return r.roomId == roomId;
                });

                if (roomIndex < 0) {
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

    public class EquipmentItem
    {
        public long id { get; set; }
        public string eqName { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
        public bool isAlertTemperature { get; set; }
        public bool isAlertHumidity { get; set; }
        public int roomId { get; set; }
    }

    public class RoomItem
    {
        public int roomId { get; set; }
        public string roomName { get; set; }
        public List<EquipmentItem> dataList { get; set; }
    }

    public class RoomAverageItem
    {
        public int roomId { get; set; }
        public string roomName { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
        public bool isAlertTemperature { get; set; }
        public bool isAlertHumidity { get; set; }
    }

    //public class InitState
    //{
    //    public InitState()
    //    {
    //        portList = new List<string>();
    //        sysConfig = new StructSystemConfig();
    //        mainList = new List<Equipment>();
    //    }
    //    public List<string> portList { get; set; }
    //    public StructSystemConfig sysConfig { get; set; }
    //    public List<Equipment> mainList { get; set; }

    //}
}
