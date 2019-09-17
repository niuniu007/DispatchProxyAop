using DotNettyRPC;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientTest
{

    /// <summary>
    /// 
    /// </summary>
    [RpcClientSvc(Host = "127.0.0.1",Port = 39999,ServiceName ="TestConsul")]
    public interface TestClientInterface
    {
        [RpcClientMethon(server ="/home/sayhello")]
        string sayHello(string  name);
        
    }
}
