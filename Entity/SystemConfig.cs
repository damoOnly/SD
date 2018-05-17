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
    }
}
