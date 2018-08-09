using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Diagnostics;

namespace CommandManager
{
    public static class Parse
    {        
        /// <summary>
        /// 获取实时数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static EquipmentData GetRealData(byte[] data, Equipment eq)
        {
            EquipmentData ed = new EquipmentData();
            
            //Array.Reverse(data, 5, 2);
            if (eq.FunctionNumType == EM_FunctionNumType.Three)
            {
                Array.Reverse(data, 3, 4);
                ed.Chroma = Convert.ToSingle(Math.Round(BitConverter.ToSingle(data, 3) * eq.Revise, eq.Point));
            }
            else
            {
                Array.Reverse(data, 3, 2);
                ed.Chroma = Convert.ToSingle(Math.Round(BitConverter.ToUInt16(data, 3) /10.0, eq.Point));
            }

            if (ed.Chroma>eq.Range)
            {
                ed.ChromaAlertStr= EM_AlertType.超量程报警.ToString();
            }
            else if(ed.Chroma > eq.HighAlert)
            {
                ed.ChromaAlertStr = EM_AlertType.高浓度报警.ToString();
            }
            else if(ed.Chroma > eq.LowAlert)
            {
                ed.ChromaAlertStr = EM_AlertType.低浓度报警.ToString();
            }
            return ed;
        }

        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param> 
        /// <returns></returns> 
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                }
            }
            return returnStr;
        }
    }
}
