using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            Array.Reverse(data, 3, 4);
            //Array.Reverse(data, 5, 2);
            ed.Chroma = Convert.ToSingle(Math.Round(BitConverter.ToSingle(data, 3) * eq.Revise, eq.Point));

            if (ed.Chroma > eq.Range)
            {
                ed.ChromaAlertStr = EM_AlertType.超量程报警.ToString();
            }
            else if (ed.Chroma > eq.HighAlert)
            {
                ed.ChromaAlertStr = EM_AlertType.高浓度报警.ToString();
            }
            else if (ed.Chroma > eq.LowAlert)
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

        public static List<EquipmentData> GetSocketDataList(string data)
        {
            List<EquipmentData> result = new List<EquipmentData>();
            MatchCollection mc = Regex.Matches(data, @"##0096([\s\S]*?)\&\&[0-9a-fA-F]{4}");
            foreach (Match item in mc)
            {
                result.Add(GetSocketData(item.Value)); 
            }
            
            return result;
        }

        public static EquipmentData GetSocketData(string one)
        {
            EquipmentData result = new EquipmentData();
            Match rtd = Regex.Match(one, @"025-Rtd=(-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0))");
            result.Chroma = rtd.Groups.Count >= 2 ? float.Parse(rtd.Groups[1].Value) : 0;

            Match mn = Regex.Match(one, @"MN=([1-9]\d*)");
            result.Address = mn.Groups.Count >= 2 ? mn.Groups[1].Value : string.Empty;

            Match flag = Regex.Match(one, @"025-Flag=N");
            result.Flag = flag.Success;

            Match CRC = Regex.Match(one, @"\&\&([0-9a-fA-F]{4})");
            result.CRC = CRC.Groups.Count >= 2 ? CRC.Groups[1].Value : string.Empty;

            return result;
        }
    }
}
