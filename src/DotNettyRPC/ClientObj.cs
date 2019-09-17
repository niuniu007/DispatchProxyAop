using System.Threading;

namespace Coldairarrow.DotNettyRPC
{

    /// <summary>
    /// 信号量+string字段干嘛？
    /// </summary>
    class ClientObj
    {
        public AutoResetEvent WaitHandler { get; set; } = new AutoResetEvent(false);
        public string ResponseString { get; set; }
    }
}
