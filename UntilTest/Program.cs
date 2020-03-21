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
            string str = "##0130QN=20200725210900001;ST=27;CN=2011;PW=123456;MN=MK130502VQ0162;Flag=4;CP=&&DataTime=20200725210800;a24087-Rtd=10.3,a24087-Flag=N&&0F00";
            var eq = Parse.GetSocketDataList(str);
        }
    }
}
