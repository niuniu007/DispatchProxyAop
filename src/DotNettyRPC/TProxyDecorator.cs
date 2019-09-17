using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using Coldairarrow.DotNettyRPC;
using DotNetty.Transport.Channels;
using Coldairarrow.Util;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels.Sockets;
using DotNetty.Codecs;
using DotNetty.Buffers;

namespace DotNettyRPC
{
    public class ProxyDecorator<T> : DispatchProxy
    {
        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string ServiceName { get; set; }
        static Bootstrap _bootstrap { get; }
        static ClientWait _clientWait { get; } = new ClientWait();

        static ProxyDecorator()
        {
            _bootstrap = new Bootstrap()
                .Group(new MultithreadEventLoopGroup())
                .Channel<TcpSocketChannel>()
                .Option(ChannelOption.TcpNodelay, true)
                .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("framing-enc", new LengthFieldPrepender(8));
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 8, 0, 8));

                    pipeline.AddLast(new ClientHandler(_clientWait));
                }));
        }

        public ProxyDecorator()
        {

        }

        ///// <summary>
        ///// 创建代理实例
        ///// </summary>
        ///// <param name="decorated">代理的接口类型</param>
        ///// <returns></returns>
        public T Create(string serverIp, int port, string serviceName)
        {

            object proxy = Create<T, ProxyDecorator<T>>();   //调用DispatchProxy 的Create  创建一个新的T
            ((ProxyDecorator<T>)proxy).ServerIp = serverIp;
            ((ProxyDecorator<T>)proxy).ServerPort = port;
            ((ProxyDecorator<T>)proxy).ServiceName = serviceName;
            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod == null) throw new Exception("无效的方法");

            try
            {

                ResponseModel response = null;
                IChannel client = null;
                try
                {
                    client = AsyncHelpers.RunSync(() => _bootstrap.ConnectAsync($"{ServerIp}:{ServerPort}".ToIPEndPoint()));
                }
                catch
                {
                    throw new Exception("连接到服务端失败!");
                }
                if (client != null)
                {
                    _clientWait.Start(client.Id.AsShortText());
                    RequestModel requestModel = new RequestModel
                    {
                        ServiceName = ServiceName,
                        MethodName = targetMethod.Name,
                        Paramters = args.ToList()
                    };
                    var sendBuffer = Unpooled.WrappedBuffer(requestModel.ToJson().ToBytes(Encoding.UTF8));

                    client.WriteAndFlushAsync(sendBuffer);
                    var responseStr = _clientWait.Wait(client.Id.AsShortText()).ResponseString;
                    response = responseStr.ToObject<ResponseModel>();
                }
                else
                {
                    throw new Exception("连接到服务端失败!");
                }

                if (response == null)
                    throw new Exception("服务器超时未响应");
                else if (response.Success)
                {
                    Type returnType = targetMethod.ReturnType;
                    if (returnType == typeof(void))
                        return null;
                    else
                         return response.Data;
                }
                else
                    throw new Exception($"服务器异常，错误消息：{response.Msg}");

            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    LogException(ex.InnerException ?? ex, targetMethod);
                    throw ex.InnerException ?? ex;
                }
                else
                {
                    throw ex;
                }
            }
        }


        /// <summary>
        /// aop异常的处理
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="methodInfo"></param>
        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            try
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Class {methodInfo.IsAbstract.GetType().FullName}");
                errorMessage.AppendLine($"Method {methodInfo?.Name} threw exception");
                errorMessage.AppendLine(exception.Message);

                //_logError?.Invoke(errorMessage.ToString());  记录到文件系统
            }
            catch (Exception)
            {
                // ignored  
                //Method should return original exception  
            }
        }
    }
}