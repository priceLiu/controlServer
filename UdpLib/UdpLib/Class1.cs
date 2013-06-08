using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace UdpLib
{
    public class UdpClient
    {
        public UdpClient(string host, int port)
        {
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if (string.IsNullOrEmpty(host))
                mSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            else
                mSocket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            for (int i = 0; i < 20; i++)
            {
                SocketAsyncEventArgs saea = new SocketAsyncEventArgs();
                saea.Completed += OnReceiveCompleted;
                saea.SetBuffer(new byte[1024 * 64], 0, 1024 * 64);
                mPools.Enqueue(saea);
            }
            BeginReceive();
        }

        private Exception mLastError;

        private Queue<SocketAsyncEventArgs> mPools = new Queue<SocketAsyncEventArgs>();

        private Socket mSocket;

        private SocketAsyncEventArgs Pop()
        {
            lock (mPools)
            {
                return mPools.Dequeue();
            }
        }

        private void Push(SocketAsyncEventArgs e)
        {
            lock (mPools)
            {
                mPools.Enqueue(e);
            }
        }

        private void OnReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                {
                    UdpReceiveArgs ura = new UdpReceiveArgs();
                    ura.EndPoint = e.RemoteEndPoint;
                    ura.Data = e.Buffer;
                    ura.Offset = 0;
                    ura.Count = e.BytesTransferred;
                    OnReceive(ura);
                }
            }
            catch (Exception e_)
            {
                mLastError = e_;
            }
            finally
            {
                Push(e);
            }
        }

        private void BeginReceive()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            SocketAsyncEventArgs e = Pop();
            e.RemoteEndPoint = endpoint;
            mSocket.ReceiveFromAsync(e);
        }

        protected virtual void OnReceive(UdpReceiveArgs e)
        {
            if (Receive != null)
                Receive(this, e);
        }

        public Exception LastError
        {
            get
            {
                return mLastError;
            }
        }

        public void Send(string data, string host, int port)
        {
            Send(data, new IPEndPoint(IPAddress.Parse(host), port));
        }

        public void Send(byte[] data, string host, int port)
        {
            Send(data, new IPEndPoint(IPAddress.Parse(host), port));
        }

        public void Send(byte[] data, EndPoint point)
        {
            mSocket.SendTo(data, point);
        }

        public void Send(string data, EndPoint point)
        {
            Send(Encoding.UTF8.GetBytes(data), point);
        }

        public event EventHandler<UdpReceiveArgs> Receive;
    }



    public class UdpReceiveArgs : EventArgs
    {

        public EndPoint EndPoint
        {
            get;
            set;
        }

        public byte[] Data
        {
            get;
            set;
        }

        public int Offset
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }
    }

}
