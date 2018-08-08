using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 通信协议对应的实体,公共部分
    /// </summary>
    public class Equipment
    {
        
        public Equipment()
        {
            ChromaAlertStr = "正常";
            ID = 0;
            EName = string.Empty;
            Unit = string.Empty;
            Revise = 1;
            lostNum = 0;
        }

        public long ID { get; set; }

        /// <summary>
        /// 仪器地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public byte SensorStatus { get; set; }

        /// <summary>
        /// 仪器报警开关
        /// </summary>
        public byte AlertType { get; set; }

        public string AlertTypeStr 
        {
            get
            {
                string str = string.Empty;
                switch (AlertType)
                {
                    case 0:
                        str = "关闭";
                        break;
                    case 1:
                        str = "打开";
                        break;
                    default:
                        break;
                }
                return str;
            }
        }

        /// <summary>
        /// 仪器名称
        /// </summary>
        public string EName { get; set; }

        /// <summary>
        /// 仪器位置
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 量程
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 小数位
        /// </summary>
        public byte Point { get; set; }

        /// <summary>
        /// 修正因子
        /// </summary>
        public float Revise { get; set; }

        /// <summary>
        /// 低报警线
        /// </summary>
        public float LowAlert { get; set; }

        /// <summary>
        /// 高报警线
        /// </summary>
        public float HighAlert { get; set; }

        /// <summary>
        /// 增加时间
        /// </summary>
        public DateTime Addtime { get; set; }

        /// <summary>
        /// 是否连接
        /// </summary>
        public bool IsConnect { get; set; }

        /// <summary>
        /// 浓度
        /// </summary>
        public float Chroma { get; set; }

        /// <summary>
        /// 报警状态
        /// </summary>
        public string ChromaAlertStr { get; set; }

        /// <summary>
        /// 报警对象
        /// </summary>
        public Alert AlertObject { get; set; }

        /// <summary>
        /// 丢包计时
        /// </summary>
        public int lostNum { get; set; }

        public string CRC { get; set; }
    }

    //public class EquipmentComparer : IEqualityComparer<Equipment>
    //{
        
    //}
}
