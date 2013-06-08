using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UdpLib.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            UdpClient client = new UdpClient("127.0.0.1", 0);
            client.Receive += (o, e) => {
                Console.WriteLine(Encoding.UTF8.GetString(
                    e.Data, 0, e.Count));
            };
            client.Send("http://www.163.com", "127.0.0.1", 8089);
            Console.Read();
        }
    }
}
