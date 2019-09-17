using Common;
using DotNettyRPC;
using System;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //IHello client = RPCClientFactory.GetClient<IHello>("127.0.0.1", 39999);
            //var serviceProxy = new ProxyDecorator<IHello>();
            //IHello client = serviceProxy.Create("127.0.0.1", 39999, "IHello");
            var serviceProxy = new ProxyDecorator<IMail>();
            IMail client = serviceProxy.Create("127.0.0.1", 39999, "IMail");
            string   msg= client.Send("张三丰");
            Console.WriteLine(msg);
            Console.WriteLine("完成");
            Console.ReadLine();
        }
    }
}
