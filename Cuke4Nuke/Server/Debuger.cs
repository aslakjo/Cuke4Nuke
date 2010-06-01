using System.Diagnostics;
using System.Threading;

namespace Cuke4Nuke.Server
{
    class DebugerCommand : IDebugerCommand
    {
        public void WaitForDebuger()
        {
            while (!Debugger.IsAttached)
                Thread.Sleep(1000);
        }
    }
}
