using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandManager
{
    public class CRC
    {
        #region CRC校验
        static public byte[] GetCRC(byte[] bytes)
        {
            byte[] crc = new byte[2];
            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (bytes.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ bytes[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            crc[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            crc[0] = CRCLow = (byte)(CRCFull & 0xFF);
            return crc;
        }
        #endregion
        #region 校验回应信息
        static public bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            byte[] CRC = new byte[2];
            CRC = GetCRC(response);
            if (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1])
                return true;
            else
                return false;
        }
        #endregion
        #region 创建命令和数据解析
        //要根据寄存的数量来判断收取帧的大小
        // 04 读输入寄存器  2N+5  N是寄存器的个数
        // 06 写寄存器      8
        public static bool Analysis(byte[] response, out string result)
        {
            //return adu;
            // 解析的时候要区分 命令  不同的命令不一样的
            //一般一个仪器的值是4个字节  所以是2个寄存器
            //4个byte 要转成个float
            if (response.Length == 2)
            {
                //说明是 错误码 
                result = response[1].ToString();
                return false;
            }
            else if ((response.Length == 9))
            {
                if (response[1] == 4)
                {
                    if (CheckResponse(response))
                    {
                        //response[1] 功能码是 04
                        //response[2] 是字节数  一个寄存器 =2个字节数
                        int num = (int)response[2]; //num+5 就是接收帧的大小
                        //response[3] response[4] response[5] response[6]   转换成float
                        result = "";
                        //读命令 要把数据交给 result

                        return true;
                    }
                    else
                    {
                        result = "校验失败";
                        return false;
                    }
                }
                else
                {
                    result = "数据错误";
                    return false;
                }

            }
            else if ((response.Length == 8))
            {
                if (response[1] == 6)
                {
                    if (CheckResponse(response))
                    {
                        //response[1] 功能码是 04
                        result = "完成";
                        return true;
                    }
                    //response[2] 是字节数  一个寄存器 =2个字节数                                        
                    else
                    {
                        result = "校验失败";
                        return false;
                    }
                }
                else
                {
                    result = "数据错误";
                    return false;
                }

            }
            else //长度不对啊
            {
                result = "";
                return false;
            }

        }
        #endregion
    }  
}
