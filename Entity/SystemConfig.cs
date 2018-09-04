using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 系统配置实体
    /// </summary>
    [Serializable] 
    public class SystemConfig
    {
        public SystemConfig()
        {
            BaudRate = 9600;
            Preiod = 5;
            PreiodUnit = 0;
            Xrange = 30;
            BackupPreiod = 30;
            CenterPort = 5718;
            RunTimeSpan = "00:00:00";
        }
        /// <summary>
        /// 串口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 采集周期
        /// </summary>
        public int Preiod { get; set; }

        /// <summary>
        /// 周期单位
        /// </summary>
        public int PreiodUnit { get; set; }

        /// <summary>
        /// 实时曲线X范围
        /// </summary>
        public int Xrange { get; set; }

        /// <summary>
        /// 环境温度
        /// </summary>
        public float Temperature { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// 相对分子量
        /// </summary>
        public int Molecular { get; set; }

        /// <summary>
        /// 备份路径
        /// </summary>
        public string BackupPath { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public bool Isbackup { get; set; }

        /// <summary>
        /// 数据库备份周期
        /// </summary>
        public int BackupPreiod { get; set; }

        /// <summary>
        /// 中心站IP
        /// </summary>
        public string CenterIP { get; set; }

        /// <summary>
        /// 中心站端口号
        /// </summary>
        public int CenterPort { get; set; }

        /// <summary>
        /// 声音文件名称
        /// </summary>
        public string SoundName { get; set; }

        /// <summary>
        /// 普通用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 普通用户密码
        /// </summary>
        public string UserPWD { get; set; }

        /// <summary>
        /// 管理员用户名
        /// </summary>
        public string Admin { get; set; }

        /// <summary>
        /// 管理员用户密码
        /// </summary>
        public string AdminPWD { get; set; }

        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName { get; set; }

        /// <summary>
        /// 是否自动检测
        /// </summary>
        public bool Isauto { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string BankName { get; set; }


        /// <summary>
        /// 室内温度
        /// </summary>
        public string NameTemperatureIn { get; set; }
        public string ValueTemperatureIn { get; set; }

        /// <summary>
        /// 室外温度
        /// </summary>
        public string NameTemperatureOut { get; set; }
        public string ValueTemperatureOut { get; set; }

        /// <summary>
        /// 室内湿度
        /// </summary>
        public string NameHumidityIn { get; set; }
        public string ValueHumidityIn { get; set; }

        /// <summary>
        /// 室外湿度
        /// </summary>
        public string NameHumidityOut { get; set; }
        public string ValueHumidityOut { get; set; }

        /// <summary>
        /// 噪音
        /// </summary>
        public string NameSound { get; set; }
        public string ValueSound { get; set; }

        public string NameNew { get; set; }
        public string ValueNew { get; set; }

        public string NameBack { get; set; }
        public string ValueBack { get; set; }

        public string NameTVOC { get; set; }
        public string ValueTVOC { get; set; }
        public string ValueMinTVOC { get; set; }
        public string ValueMaxTVOC { get; set; }
        public string Value2MinTVOC { get; set; }
        public string Value2MaxTVOC { get; set; }

        public string NameO2 { get; set; }
        public string ValueO2 { get; set; }
        public string ValueMinO2 { get; set; }
        public string ValueMaxO2 { get; set; }
        public string Value2MinO2 { get; set; }
        public string Value2MaxO2 { get; set; }

        public string NameCO { get; set; }
        public string ValueCO { get; set; }
        public string ValueMinCO { get; set; }
        public string ValueMaxCO { get; set; }
        public string Value2MinCO { get; set; }
        public string Value2MaxCO { get; set; }

        public string NameSO2 { get; set; }
        public string ValueSO2 { get; set; }
        public string ValueMinSO2 { get; set; }
        public string ValueMaxSO2 { get; set; }
        public string Value2MinSO2 { get; set; }
        public string Value2MaxSO2 { get; set; }

        public string NameNOX { get; set; }
        public string ValueNOX { get; set; }
        public string ValueMinNOX { get; set; }
        public string ValueMaxNOX { get; set; }
        public string Value2MinNOX { get; set; }
        public string Value2MaxNOX { get; set; }

        public string NameNewarea { get; set; }
        public string ValueNewarea { get; set; }
        //public string ValueMinNewarea { get; set; }
        //public string ValueMaxNewarea { get; set; }

        public string NameBackarea { get; set; }
        public string ValueBackarea { get; set; }
        //public string ValueMinBackarea { get; set; }
        //public string ValueMaxBackarea { get; set; }

        public string NameAirQuality { get; set; }
        public string ValueAirQuality { get; set; }
        public string ValueAirQuality2 { get; set; }
        public string ValueMinAirQuality { get; set; }
        public string ValueMaxAirQuality { get; set; }
        public string Value2MinAirQuality { get; set; }
        public string Value2MaxAirQuality { get; set; }

        public string NameSterilize { get; set; }
        public string ValueSterilize { get; set; }
        public string ValueSterilize2 { get; set; }
        public string ValueMinSterilize { get; set; }
        public string ValueMaxSterilize { get; set; }
        public string Value2MinSterilize { get; set; }
        public string Value2MaxSterilize { get; set; }

        public string NamePM { get; set; }
        public string ValuePM { get; set; }
        public string ValueMinPM { get; set; }
        public string ValueMaxPM { get; set; }
        public string Value2MinPM { get; set; }
        public string Value2MaxPM { get; set; }

        public string RunTimeSpan { get; set; }
    }
}
