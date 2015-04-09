using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace XDTCPTest
{
    public class TcpHelper
    {
        #region ===私有变量===

        /// <summary>
        /// TcpClient
        /// </summary>
        private TcpClient _client;

        /// <summary>
        /// Tcp服务器结点
        /// </summary>
        private IPEndPoint _serverIPEndPoint;

        /// <summary>
        /// 客户端是否已连接
        /// </summary>
        private bool _isConnected = false;

        /// <summary>
        /// 接收数据的自定义缓冲区大小
        /// </summary>
        private readonly int _receiveDataBufferSize = 1024 * 1024 * 10;

        /// <summary>
        /// 总发送字节数
        /// </summary>
        private decimal _totalSendBytesNum = 0;

        /// <summary>
        /// 总接收字节数
        /// </summary>
        private decimal _totalReceiveBytesNum = 0;

        /// <summary>
        /// 用户session
        /// </summary>
        private int _session = 0;

        /// <summary>
        /// 用户id
        /// </summary>
        private ulong _userid = 0;


        ///// <summary>
        ///// 数据接收缓冲区
        ///// </summary>
        //private static ReadWriteRingBuffer ringbuffer = new ReadWriteRingBuffer(1024 * 1024 * 10);

        ///// <summary>
        ///// 
        ///// </summary>
        //static bool recvFlag = false;

        #endregion

        #region # 对外属性  #
        /// <summary>
        /// 客户端是否已连接
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }


        /// <summary>
        /// 摘要:保存会话参数值
        /// </summary>
        public int Session
        {
            get { return _session; }
            set { _session = value; }
        }
        /// <summary>
        /// 摘要:保存UserId
        /// </summary>
        public UInt64 UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// 摘要:发送的字节数
        /// </summary>
        public decimal SendBytesNumber
        {
            get { return _totalSendBytesNum; }
        }

        /// <summary>
        /// 摘要:接收的字节数
        /// </summary>
        public decimal ReceiveBytesNumber
        {
            get { return _totalReceiveBytesNum; }
        }

        #endregion

        #region ===事件定义===

        /// <summary>
        /// 连接失败时事件
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnConnectFail;

        /// <summary>
        /// 连接成功时事件
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnConnectSuccess;

        /// <summary>
        /// 发送数据时事件
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnSendData;

        /// <summary>
        /// 接收数据时事件
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnReceiveData;

        #endregion

        //private bool StartProcessThread()
        //{
        //    Thread t = new Thread(ProcessDate);
        //    t.Start();
        //    return true;
        //}

        public TcpHelper()
        {
        }


        public void NewTcpHelper(string ipHost, int port)
        {
            IPEndPoint serverIPEndPoint = new IPEndPoint(IPAddress.Parse(ipHost), port);
            this._serverIPEndPoint = serverIPEndPoint;
            //recvFlag = true;

            this.Start();
        }

        /// <summary>
        /// 启动客户端
        /// 同步连接
        /// </summary>
        /// <returns></returns>
        private bool Start()
        {
            this._client = new TcpClient(new IPEndPoint(IPAddress.Any, 0));
            this._client.ReceiveBufferSize = 1024 * 1024 * 10;
            this._client.SendBufferSize = 1024 * 1024 * 10;

            bool bError = true;

            try
            {
                this._client.Connect(this._serverIPEndPoint);

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("连接服务器SocketException：{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("连接服务器ObjectDisposedException：{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("连接服务器错：{0}", ex));
            }

            if (bError)
            {
                // 连接失败，调用连接失败事件
                this._isConnected = false;

                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("连接失败"));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex));
                    }
                }
            }
            else
            {
                // 连接成功，调用连接成功事件
                this._isConnected = true;

                if (this.OnConnectSuccess != null)
                {
                    try
                    {
                        this.OnConnectSuccess(this, new TcpClientEventArgs("连接成功"));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("调用连接成功事件时错：{0}", ex));
                    }
                }

                // 开始接收数据
                this.PostReceive();
            }

            //if (this._checkTimer != null)
            //{
            //    this._checkTimer.Change(2000, 5000);
            //}

            return !bError;
        }

        /// <summary>
        /// 开始异步接收数据
        /// </summary>
        private void PostReceive()
        {
            if (this._client == null)
            {
                return;
            }

            byte[] data = new byte[this._receiveDataBufferSize];

            bool bError = true;

            try
            {
                this._client.Client.BeginReceive(
                    data                                //接收数据的缓冲区
                    , 0                                 //
                    , data.Length                       //
                    , SocketFlags.None
                    , new AsyncCallback(this.ReceiveDataCallback), data);

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("开始接收数据时SocketException：{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("开始接收数据时ObjectDisposedException：{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("开始接收数据时：{0}", ex));
            }

            if (_isConnected && _isConnected != !bError)
            {
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex1));
                    }
                }

            }
            this._isConnected = !bError;

        }


        /// <summary>
        /// 接收数据回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveDataCallback(IAsyncResult ar)
        {
            bool bError = true;

            int iReceive = 0;

            try
            {
                SocketError se = SocketError.Success;
                lock (this)
                {
                    if (this._client != null)
                    {
                        iReceive = this._client.Client.EndReceive(ar, out se);
                    }
                }

                //以5个'/r/n' 为节点处理包
                // 统计接收字节数
                this._totalReceiveBytesNum += iReceive;

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("接收数据时SocketException：{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("接收数据时ObjectDisposedException：{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("接收数据时错：{0}", ex));
            }

            if (!bError)
            {
                if (iReceive > 0)
                {
                    // 接收成功，调用接收数据事件
                    //if (this.OnReceiveData != null)
                    //{
                    try
                    {
                        byte[] data = ar.AsyncState as byte[];
                        //ringbuffer.Write(data, 0, iReceive);
                        OnReceiveData(
                           this
                           , new TcpClientEventArgs(iReceive, data, string.Format("共接收{0}字节数据", iReceive)));

                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("TcpHelper==>>ReceiveDataCallback();调用接收数据事件时错：{0}", ex));

                        this._isConnected = false;
                        if (this.OnConnectFail != null)
                        {
                            try
                            {
                                this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                            }
                            catch (Exception ex1)
                            {
                                Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex1));
                            }
                        }

                        return;
                    }
                    //}
                    this.PostReceive();

                }
                else        // 连接断开
                {
                    this._isConnected = false;
                    if (this.OnConnectFail != null)
                    {
                        try
                        {
                            this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex));
                        }
                    }

                    Trace.WriteLine(string.Format("TcpHelper==>>ReceiveDataCallback();接收数据长度为零"));

                }
            }
            else
            {
                this._isConnected = false;
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex1));
                    }
                }

            }
        }

        /// <summary>
        /// 停止客户端
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            if (this._client == null)
            {
                return true;
            }

            //if (this._checkTimer != null)
            //{
            //    this._checkTimer.Change(Timeout.Infinite, Timeout.Infinite);
            //}
            bool bError = true;

            try
            {
                this._client.Client.Close();

                this._client.Close();

                bError = false;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("关闭Tcp失败：{0}", ex));
            }

            this._client = null;

            this._isConnected = false;

            if (this.OnConnectFail != null)
            {
                try
                {
                    this.OnConnectFail(this, new TcpClientEventArgs("用户主动断开"));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex));
                }
            }

            Trace.WriteLine("连接关闭");
            return !bError;
        }


        /// <summary>
        /// 发送数据
        /// 异步发送
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            if (!this._isConnected)
            {
                return;
            }

            bool bError = true;

            try
            {
                this._client.Client.BeginSend(
                    data
                    , 0
                    , data.Length
                    , SocketFlags.None
                    , new AsyncCallback(this.SendDataCallBack), this._client.Client);

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("发送数据时SocketException：{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("发送数据时ObjectDisposedException：{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("发送数据时：{0}", ex));
            }

            if (bError)
            {
                this._isConnected = false;
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex1));
                    }
                }

            }
        }

        /// <summary>
        /// 发送数据回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void SendDataCallBack(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;

            bool bError = true;

            int iSend = 0;

            try
            {
                iSend = socket.EndSend(ar);

                // 统计发送字节数
                this._totalSendBytesNum += iSend;

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("发送数据时SocketException：{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("发送数据时ObjectDisposedException：{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("发送数据时错：{0}", ex));
            }

            if (!bError)
            {
                // 发送成功，调用发送数据事件
                if (this.OnSendData != null)
                {
                    try
                    {
                        this.OnSendData(
                            this
                            , new TcpClientEventArgs(iSend, null, string.Format("共发送{0}字节数据", iSend)));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("调用发送数据事件时错：{0}", ex));

                        this._isConnected = false;
                        if (this.OnConnectFail != null)
                        {
                            try
                            {
                                this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                            }
                            catch (Exception ex1)
                            {
                                Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex1));
                            }
                        }

                    }
                }
            }
            else
            {
                this._isConnected = false;
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("连接断开"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("调用连接失败事件时错：{0}", ex1));
                    }
                }

            }
        }
    }
}
