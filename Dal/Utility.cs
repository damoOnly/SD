using Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class Utility
    {
        public static DateTime CutOffMillisecond(DateTime dt)
        {
            return new DateTime(dt.Ticks - (dt.Ticks % TimeSpan.TicksPerSecond), dt.Kind);
        }

        public static void ExportListToCSV(List<EquipmentData> list, string path)
        {
            System.IO.FileStream fs = new FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, new System.Text.UnicodeEncoding());
            //Tabel header
            sw.Write("温度(℃)");
            sw.Write("\t");
            sw.Write("湿度(%RH)");
            sw.Write("\t");
            sw.Write("时间");
            sw.Write("\t");
            sw.WriteLine("");

            //Table body
            for (int i = 0; i < list.Count; i++)
            {
                var ed = list[i];
                sw.Write(ed.temperature);
                sw.Write("\t");
                sw.Write(ed.humidity);
                sw.Write("\t");
                sw.Write(ed.AddTime.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Write("\t");
                sw.WriteLine("");
            }
            sw.Flush();
            sw.Close();
        }
    }
}
