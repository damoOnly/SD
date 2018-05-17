using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 报警记录
    /// </summary>
    public class Alert
    {
        public Alert()
        {
            StartTime = DateTime.Now;
        }
        /// <summary>
        /// 自身ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 报警类别
        /// </summary>
        public string AlertName { get; set; }

        /// <summary>
        /// 报警值
        /// </summary>
        public float AlertValue { get; set; }

        /// <summary>
        /// 仪器名称
        /// </summary>
        public string EName { get; set; }

        /// <summary>
        /// 仪器位置
        /// </summary>
        public string Place { get; set; }

        /// <summary>
        /// 仪器地址
        /// </summary>
        public byte Address { get; set; }

        public string ATimeSpan
        {
            get
            {
                TimeSpan ts = EndTime - StartTime;
                TimeSpan ts1 = new TimeSpan(0, 0, 0, 1);
                if (ts < ts1)
                {
                    return "00:00:00";
                }
                return ts.ToString();
            }
        }
    }

    /// <summary>
    /// 比较器
    /// </summary>
    public class AlertComparer : IEqualityComparer<Alert>
    {
        public bool Equals(Alert x, Alert y)
        {
            return x.AlertName == y.AlertName;
        }

        public int GetHashCode(Alert obj)
        {
            return obj.AlertName.GetHashCode();
        }
    }
}
