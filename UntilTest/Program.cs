using CommandManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UntilTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "##0096ST=31;CN=2051;PW=123456;MN=94780141100108;CP=&&DataTime=20180517154200;025-Rtd=2,025-Flag=N,;&&E401";
            var eq = Parse.GetSocketDataList(str);
        }
    }
}
