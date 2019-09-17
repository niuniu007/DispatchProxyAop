using Coldairarrow.Util;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace Coldairarrow.DotNettyRPC
{
    /// <summary>
    /// 适配器
    /// </summary>
    class ServerHandler : ChannelHandlerAdapter
    {
        public ServerHandler(RPCServer rPCServer)
        {
            _rpcServer = rPCServer;
        }

        RPCServer _rpcServer { get; }  //服务管理器

        /// <summary>
        /// 服务调用端口
        /// socket监测到服务调用后会执行此方法
        /// </summary>
        /// <param name="context">网络通信上下文</param>
        /// <param name="message">网络通信传入的消息体</param>
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var msg = message as IByteBuffer;
            ResponseModel response = _rpcServer.GetResponse(msg.ToString(Encoding.UTF8).ToObject<RequestModel>());
            var sendMsg = response.ToJson().ToBytes(Encoding.UTF8);
            context.WriteAndFlushAsync(Unpooled.WrappedBuffer(sendMsg));
            context.CloseAsync();
        }

        /// <summary>
        /// 网络通信完成后刷新缓冲区
        /// </summary>
        /// <param name="context">网络通信上下文</param>
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        /// <summary>
        /// 异常处理？？
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
    }
}