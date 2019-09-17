using System;
using System.Collections.Generic;
using System.Text;

namespace DotNettyRPC
{

    /// <summary>
    /// 自动实现自动代理类的创建
    /// </summary>
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Interface)]
    public class RpcClientSvcAttribute:Attribute
    {

        public string Host { get; set; }
        public int Port { get; set; }

        public string ServiceName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Dec { get; set; }
    }
}
