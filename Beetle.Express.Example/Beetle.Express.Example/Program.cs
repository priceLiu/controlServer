using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Beetle.Express.Example
{
    class Program : IServerHandler
    {
        static ServerFactory mFactory;

        private LRUDetect mDetect = new LRUDetect(60000);

        static void Main(string[] args)
        {
            mFactory = new ServerFactory("serverSection");
            foreach (IServer item in mFactory.Servers)
            {
                Console.WriteLine("{0} start @{1}", item.Name, item.Port);
            }
            System.Threading.Thread.Sleep(-1);
        }

        public void Connect(IServer server, ChannelConnectEventArgs e)
        {
            Console.WriteLine("{0} connected  @{1}", e.Channel.EndPoint, server.Name);

        }

        public void Disposed(IServer server, ChannelEventArgs e)
        {
            Console.WriteLine("{0} disposed", e.Channel.EndPoint);
        }

        public void Error(IServer server, ErrorEventArgs e)
        {
            Console.WriteLine("{0} error:{1}", e.Channel.EndPoint, e.Error.Message);

        }

        public void Receive(IServer server, ChannelReceiveEventArgs e)
        {
            string command = e.Data.ToString(Encoding.UTF8);
            ProcessStartInfo sInfo = new ProcessStartInfo(command);
            Process.Start(sInfo);
            //Console.WriteLine("receive:{0}\t@{1}", command, server.Name);
            //Data data = new Data(1024);
            //data.Write(server.Name, Encoding.UTF8);
            //server.Send(data, e.Channel);
        }
        public void SendCompleted(IServer server, ChannelSendEventArgs e)
        {

        }
    }
}
