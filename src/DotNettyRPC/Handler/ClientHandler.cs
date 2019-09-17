using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Text;

namespace Coldairarrow.DotNettyRPC
{

    /// <summary>
    /// 客户端处理程序
    /// </summary>
    class ClientHandler : ChannelHandlerAdapter
    {
        private ClientWait _clientWait { get; }
        public ClientHandler(ClientWait clientWait)
        {
            _clientWait = clientWait;
        }

        /// <summary>
        /// 网络通信
        /// </summary>
        /// <param name="context">网络通信上下文</param>
        /// <param name="message">网络通信中传送的消息体</param>
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var buffer = message as IByteBuffer;
            _clientWait.Set(context.Channel.Id.AsShortText(), buffer.ToString(Encoding.UTF8));  //加入到客户端消息管理中心
        }

        /// <summary>
        /// 网络通信完成成后刷新缓冲区
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        /// <summary>
        /// 异常处理
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