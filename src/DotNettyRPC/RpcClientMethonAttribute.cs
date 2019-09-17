using System;
using System.Collections.Generic;
using System.Text;

namespace DotNettyRPC
{

    /// <summary>
    /// 应用到方法
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Method)]
    public class RpcClientMethonAttribute:Attribute
    {

        public string server { get; set; }
    }
}
