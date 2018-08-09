using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GasInfo
    {
        public GasInfo()
        {
            IsDel = 0;
            AddTime = DateTime.Now;
        }
        /// <summary>
        /// 数据库中记录的ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 气体名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分子式
        /// </summary>
        public string MolecularFormula { get; set; }

        /// <summary>
        /// 量程
        /// </summary>
        public UInt32 Range { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public byte Unit { get; set; }
        public string UnitStr
        {
            get
            {
                string str = string.Empty;
                switch (Unit)
                {

                    case 0:
                        str = "PPM";
                        break;
                    case 1:
                        str = "%VOL";
                        break;
                    case 2:
                        str = "%LEL";
                        break;
                    case 3:
                        str = "MG/M3";
                        break;
                        case 4:
                        str = "PPB";
                        break;
                    default:
                        break;
                }
                return str;
            }
        }

        /// <summary>
        /// 基准小数点 
        /// </summary>
        public byte Point { get; set; }
        public string PointStr
        {
            get
            {
                string str = string.Empty;
                switch (Point)
                {
                    case 0:
                        str = "无小数点";
                        break;
                    case 1:
                        str = "1位小数点";
                        break;
                    case 2:
                        str = "2位小数点";
                        break;
                    case 3:
                        str = "3位小数点";
                        break;
                    default:
                        break;
                }
                return str;
            }
        }

        /// <summary>
        /// 分子量
        /// </summary>
        public float MolecularWeight { get; set; }

        /// <summary>
        /// 低浓度报警点
        /// </summary>
        public float LowChromaAlert { get; set; }

        /// <summary>
        /// A1报警点
        /// </summary>
        public float A1ChromaAlert { get; set; }

        /// <summary>
        /// A2报警点
        /// </summary>
        public float A2ChromaAlert { get; set; }

        /// <summary>
        /// 气体编号，对应下位机
        /// </summary>
        public byte GasType { get; set; }

        /// <summary>
        /// 是否删除,1为已删除，0为存在
        /// </summary>
        public int IsDel { get; set; }

        /// <summary>
        /// 增加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
