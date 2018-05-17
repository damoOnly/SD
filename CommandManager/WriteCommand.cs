using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandManager
{
    public class WriteCommand
    {
        public WriteCommand()        
        {
            ResultLength = 8;
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
        private byte[] GetSendByte(byte address,byte highAdr,byte lowAdr,short num,float value)
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
