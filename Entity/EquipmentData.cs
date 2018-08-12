using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 设备数据
    /// </summary>
    public class EquipmentData
    {
        public EquipmentData()
        {
            AddTime = DateTime.Now;
            ChromaAlertStr = "正常";
        }
        public int ID { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentID { get; set; }

        /// <summary>
        /// 浓度
        /// </summary>
        public float Chroma { get; set; }

        /// <summary>
        /// 增加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 气体名称
        /// </summary>
        public string EName { get; set; }

        /// <summary>
        /// 报警状态
        /// </summary>
        public string ChromaAlertStr { get; set; }

    }
}
