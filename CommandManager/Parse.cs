﻿using System;
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

        public static EquipmentData GetSocketData(string data)
        {
            EquipmentData result = new EquipmentData();

            string[] dataList = data.Split(';');
            if (dataList != null && dataList.Length > 0)
            {
                for (int i = 0; i < dataList.Length; i++)
                {
                    string[] valueList = dataList[i].Split('=');
                    if (valueList != null && valueList.Length == 2)
                    {
                        switch (valueList[0])
                        {
                            case "MN":
                                result.EquipmentID = Convert.ToInt32(valueList[1]);
                                break;
                            case "025-Rtd":
                                result.Chroma = float.Parse(valueList[1]);
                                break;
                            case "025-Flag":
                                if (valueList[1] == "N")
                                {
                                    result.Flag = false;
                                }
                                else
                                {
                                    result.Flag = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
