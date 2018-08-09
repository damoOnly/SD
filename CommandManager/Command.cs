using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace CommandManager
{
    public class Command
    {
        /// <summary>
        /// 通过构造函数重载来区别是读命令还是写命令
        /// </summary>
        /// <param name="address">通信地址</param>
        /// <param name="highAdr">寄存器起始地址高位</param>
        /// <param name="lowAdr">寄存器起始地址地位</param>
        /// <param name="num">寄存器个数</param>
        public Command(byte address, byte highAdr, byte lowAdr, short num,EM_FunctionNumType functionNumType)        
        {
            byte[] sendb = new byte[8];
            sendb[0] = address;
            sendb[1] = (byte)(functionNumType == EM_FunctionNumType.Three ? 0x03:0x04);
            sendb[2] = highAdr;
            sendb[3] = lowAdr;
            byte[] bb2 = BitConverter.GetBytes(num);
            Array.Reverse(bb2);
            Array.Copy(bb2, 0, sendb, 4, 2);
            Array.Copy(CRC.GetCRC(sendb), 0, sendb, 6, 2);

            ResultLength = 5 + 2 * num;
            SendByte = sendb;
        }
        public Command(byte address, byte highAdr, byte lowAdr,byte[] content)
        {

            byte[] sendb = new byte[9+content.Length];
            sendb[0] = address;
            sendb[1] = 0x10;
            sendb[2] = highAdr;
            sendb[3] = lowAdr;

            byte[] num = BitConverter.GetBytes(Convert.ToInt16(content.Length / 2));
            Array.Reverse(num);
            Array.Copy(num, 0, sendb, 4, 2);
            sendb[6] = Convert.ToByte(content.Length);
            Array.Copy(content, 0, sendb, 7, content.Length);
            Array.Copy(CRC.GetCRC(sendb), 0, sendb, sendb.Length-2, 2);

            ResultLength = 8;
            SendByte = sendb;
        }
        #region 属性
        /// <summary>
        /// 返回桢长度
        /// </summary>
        public int ResultLength { get; set; }

        /// <summary>
        /// 发送帧
        /// </summary>
        public byte[] SendByte { get; set; }

        /// <summary>
        /// 结果桢
        /// </summary>
        public byte[] ResultByte { get; set; }

        /// <summary>
        /// 错误桢
        /// </summary>
        public byte[] ErrorByte { get; set; }
        #endregion

        /// <summary>
        /// 获取发送帧
        /// </summary>
        /// <param name="address"></param>
        /// <param name="highAdr"></param>
        /// <param name="lowAdr"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private byte[] SetReadByte(byte address,byte highAdr,byte lowAdr,short num,float value)
        {
            byte[] sendb = new byte[9+2*num];
            sendb[0] = address;
            sendb[1] = 0x03;  // 功能码
            sendb[2] = highAdr;
            sendb[3] = lowAdr;
            Array.Copy(BitConverter.GetBytes(num), 0, sendb, 4, 2);
            sendb[6] = Convert.ToByte(2 * num);
            Array.Copy(BitConverter.GetBytes(value), 0, sendb, 7, 4);
            Array.Copy(CRC.GetCRC(sendb), 0, sendb, 11, 2);

            ResultLength = 5 + 2 * num;
            return sendb;
        }
    }
}
