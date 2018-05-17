using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogLib
{
    class Class
    {
        // 请注意Properties中的AssemblyInfo.cs 最后一行配置文件路径一定要正确
        // 简单使用实例
        private void method1()
        {
            Log.GetLogger("pepole").Debug("要记录的错误信息，也可以是一个异常对象");
        }
    }
}
