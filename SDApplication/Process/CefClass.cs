
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

    public class MessageData
    {
        public MessageData(string _type, string _data)
        {
            this.type = _type;
            this.data = _data;
        }
        public string type { get; set; }
        public string data { get; set; }
    }

    public class HistoryQuery
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string fileId { get; set; }
        public string type { get; set; }
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
