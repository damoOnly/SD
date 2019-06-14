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
        public byte Address { get; set; }

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

        public string DisplayChroma
        {
            get
            {
                if (!IsAnemoscope)
                {
                    return Chroma.ToString();
                }
                else
                {
                    return GetWind(Chroma);
                }
            }
        }

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

        public bool IsAnemoscope { get; set; }

        public static string GetWind(float value)
        {
            string result = "未知";
            if ((value >= 0 && value <= 11.2) || (value >= 348.8 && value <= 360))
            {
                result = "北";
            }
            else if (value >= 11.3 && value <= 33.7)
            {
                result = "北东北";
            }
            else if (value >= 33.8 && value <= 56.2)
            {
                result = "东北";
            }
            else if (value >= 56.3 && value <= 78.7)
            {
                result = "东东北";
            }
            else if (value >= 78.8 && value <= 101.2)
            {
                result = "东";
            }
            else if (value >= 101.3 && value <= 123.7)
            {
                result = "东东南";
            }
            else if (value >= 123.8 && value <= 146.2)
            {
                result = "东南";
            }
            else if (value >= 146.3 && value <= 168.7)
            {
                result = "南东南";
            }
            else if (value >= 168.8 && value <= 191.2)
            {
                result = "南";
            }
            else if (value >= 191.3 && value <= 213.7)
            {
                result = "南西南";
            }
            else if (value >= 213.8 && value <= 236.2)
            {
                result = "西南";
            }
            else if (value >= 236.3 && value <= 258.7)
            {
                result = "西西南";
            }
            else if (value >= 258.8 && value <= 281.2)
            {
                result = "西";
            }
            else if (value >= 218.3 && value <= 303.7)
            {
                result = "西西北";
            }
            else if (value >= 303.8 && value <= 326.2)
            {
                result = "西北";
            }
            else if (value >= 326.3 && value <= 348.7)
            {
                result = "北西北";
            }
            return result;
        }
    }

    //public class EquipmentComparer : IEqualityComparer<Equipment>
    //{

    //}
}
