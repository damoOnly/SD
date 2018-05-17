using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 用于保存配置文件XML
    /// </summary>
    public class SensorInfoConfig
    {
        public SensorInfoConfig()
        {
            AD2 = string.Empty;
            AD3 = string.Empty;
            Chroma2 = string.Empty;
            Chroma3 = string.Empty;
            AlertType = string.Empty;
        }

        /// <summary>
        /// 通道编号
        /// </summary>
        public byte SensorNum { get; set; }

        /// <summary>
        /// 是否低浓度报警
        /// </summary>
        public bool IsLow { get; set; }

        /// <summary>
        /// 是否A1报警
        /// </summary>
        public bool IsA1 { get; set; }

        /// <summary>
        /// 是否A2报警
        /// </summary>
        public bool IsA2 { get; set; }

        /// <summary>
        /// 是否TWA报警
        /// </summary>
        public bool IsTWA { get; set; }

        /// <summary>
        /// 是否STEL报警
        /// </summary>
        public bool IsSTEL { get; set; }

        /// <summary>
        /// 气体类型
        /// </summary>
        public byte GasType { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public byte Unit { get; set; }

        /// <summary>
        /// 量程
        /// </summary>
        public string Range { get; set; }

        /// <summary>
        /// 低浓度报警点
        /// </summary>
        public string LowAlert { get; set; }

        /// <summary>
        /// A1报警点
        /// </summary>
        public string A1 { get; set; }

        /// <summary>
        /// A2报警点
        /// </summary>
        public string A2 { get; set; }

        /// <summary>
        /// TWA报警点
        /// </summary>
        public string TWA { get; set; }

        /// <summary>
        /// STEL报警点
        /// </summary>
        public string STEL { get; set; }

        /// <summary>
        /// STEL报警点时长
        /// </summary>
        public string STELtime { get; set; }

        /// <summary>
        /// 开关状态
        /// </summary>
        public string OFFStatus { get; set; }

        /// <summary>
        /// 小数点
        /// </summary>
        public byte Point { get; set; }

        /// <summary>
        /// 校准类型
        /// </summary>
        public string CheckType { get; set; }

        /// <summary>
        /// 0级AD
        /// </summary>
        public string AD0 { get; set; }

        /// <summary>
        /// 1级AD
        /// </summary>
        public string AD1 { get; set; }

        /// <summary>
        /// 2级AD
        /// </summary>
        public string AD2 { get; set; }

        /// <summary>
        /// 3级AD
        /// </summary>
        public string AD3 { get; set; }

        /// <summary>
        /// 0级浓度
        /// </summary>
        public string Chroma0 { get; set; }

        /// <summary>
        /// 1级浓度
        /// </summary>
        public string Chroma1 { get; set; }

        /// <summary>
        /// 2级浓度
        /// </summary>
        public string Chroma2 { get; set; }

        /// <summary>
        /// 3级浓度
        /// </summary>
        public string Chroma3 { get; set; }

        /// <summary>
        /// 报警响应类型
        /// </summary>
        public string AlertType { get; set; }

        /// <summary>
        /// 存储模式
        /// </summary>
        public byte SaveModel { get; set; }

        /// <summary>
        /// 存储周期
        /// </summary>
        public string SavePeriod { get; set; }

        /// <summary>
        /// 放大倍数
        /// </summary>
        public string Big { get; set; }

        /// <summary>
        /// 预热时间
        /// </summary>
        public string Preheat { get; set; }
    }
}
