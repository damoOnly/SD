using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace SDApplication
{
    public static class Gloabl
    {
        public static UserInfo Userinfo { get; set; }

        public static bool IsOpen { get; set; }

        public static string NormalStr = "正常";

        public static bool IsAdmin { get; set; }
    }
}
