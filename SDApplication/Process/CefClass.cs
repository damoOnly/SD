using CefSharp;
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

    }

    public class EquipmentItem
    {
        public int id { get; set; }
        public string eqName { get; set; }
        public float temperature { get; set; }
        public float humidity { get; set; }
        public bool isAlert { get; set; }
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
