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
        #region ===˽�б���===

        /// <summary>
        /// TcpClient
        /// </summary>
        private TcpClient _client;

        /// <summary>
        /// Tcp���������
        /// </summary>
        private IPEndPoint _serverIPEndPoint;

        /// <summary>
        /// �ͻ����Ƿ�������
        /// </summary>
        private bool _isConnected = false;

        /// <summary>
        /// �������ݵ��Զ��建������С
        /// </summary>
        private readonly int _receiveDataBufferSize = 1024 * 1024 * 10;

        /// <summary>
        /// �ܷ����ֽ���
        /// </summary>
        private decimal _totalSendBytesNum = 0;

        /// <summary>
        /// �ܽ����ֽ���
        /// </summary>
        private decimal _totalReceiveBytesNum = 0;

        /// <summary>
        /// �û�session
        /// </summary>
        private int _session = 0;

        /// <summary>
        /// �û�id
        /// </summary>
        private ulong _userid = 0;


        ///// <summary>
        ///// ���ݽ��ջ�����
        ///// </summary>
        //private static ReadWriteRingBuffer ringbuffer = new ReadWriteRingBuffer(1024 * 1024 * 10);

        ///// <summary>
        ///// 
        ///// </summary>
        //static bool recvFlag = false;

        #endregion

        #region # ��������  #
        /// <summary>
        /// �ͻ����Ƿ�������
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; }
        }


        /// <summary>
        /// ժҪ:����Ự����ֵ
        /// </summary>
        public int Session
        {
            get { return _session; }
            set { _session = value; }
        }
        /// <summary>
        /// ժҪ:����UserId
        /// </summary>
        public UInt64 UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// ժҪ:���͵��ֽ���
        /// </summary>
        public decimal SendBytesNumber
        {
            get { return _totalSendBytesNum; }
        }

        /// <summary>
        /// ժҪ:���յ��ֽ���
        /// </summary>
        public decimal ReceiveBytesNumber
        {
            get { return _totalReceiveBytesNum; }
        }

        #endregion

        #region ===�¼�����===

        /// <summary>
        /// ����ʧ��ʱ�¼�
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnConnectFail;

        /// <summary>
        /// ���ӳɹ�ʱ�¼�
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnConnectSuccess;

        /// <summary>
        /// ��������ʱ�¼�
        /// </summary>
        public event EventHandler<TcpClientEventArgs> OnSendData;

        /// <summary>
        /// ��������ʱ�¼�
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
        /// �����ͻ���
        /// ͬ������
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
                Trace.WriteLine(string.Format("���ӷ�����SocketException��{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("���ӷ�����ObjectDisposedException��{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("���ӷ�������{0}", ex));
            }

            if (bError)
            {
                // ����ʧ�ܣ���������ʧ���¼�
                this._isConnected = false;

                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("����ʧ��"));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex));
                    }
                }
            }
            else
            {
                // ���ӳɹ����������ӳɹ��¼�
                this._isConnected = true;

                if (this.OnConnectSuccess != null)
                {
                    try
                    {
                        this.OnConnectSuccess(this, new TcpClientEventArgs("���ӳɹ�"));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("�������ӳɹ��¼�ʱ��{0}", ex));
                    }
                }

                // ��ʼ��������
                this.PostReceive();
            }

            //if (this._checkTimer != null)
            //{
            //    this._checkTimer.Change(2000, 5000);
            //}

            return !bError;
        }

        /// <summary>
        /// ��ʼ�첽��������
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
                    data                                //�������ݵĻ�����
                    , 0                                 //
                    , data.Length                       //
                    , SocketFlags.None
                    , new AsyncCallback(this.ReceiveDataCallback), data);

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("��ʼ��������ʱSocketException��{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("��ʼ��������ʱObjectDisposedException��{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("��ʼ��������ʱ��{0}", ex));
            }

            if (_isConnected && _isConnected != !bError)
            {
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex1));
                    }
                }

            }
            this._isConnected = !bError;

        }


        /// <summary>
        /// �������ݻص�����
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

                //��5��'/r/n' Ϊ�ڵ㴦���
                // ͳ�ƽ����ֽ���
                this._totalReceiveBytesNum += iReceive;

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("��������ʱSocketException��{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("��������ʱObjectDisposedException��{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("��������ʱ��{0}", ex));
            }

            if (!bError)
            {
                if (iReceive > 0)
                {
                    // ���ճɹ������ý��������¼�
                    //if (this.OnReceiveData != null)
                    //{
                    try
                    {
                        byte[] data = ar.AsyncState as byte[];
                        //ringbuffer.Write(data, 0, iReceive);
                        OnReceiveData(
                           this
                           , new TcpClientEventArgs(iReceive, data, string.Format("������{0}�ֽ�����", iReceive)));

                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("TcpHelper==>>ReceiveDataCallback();���ý��������¼�ʱ��{0}", ex));

                        this._isConnected = false;
                        if (this.OnConnectFail != null)
                        {
                            try
                            {
                                this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                            }
                            catch (Exception ex1)
                            {
                                Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex1));
                            }
                        }

                        return;
                    }
                    //}
                    this.PostReceive();

                }
                else        // ���ӶϿ�
                {
                    this._isConnected = false;
                    if (this.OnConnectFail != null)
                    {
                        try
                        {
                            this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex));
                        }
                    }

                    Trace.WriteLine(string.Format("TcpHelper==>>ReceiveDataCallback();�������ݳ���Ϊ��"));

                }
            }
            else
            {
                this._isConnected = false;
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex1));
                    }
                }

            }
        }

        /// <summary>
        /// ֹͣ�ͻ���
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
                Trace.WriteLine(string.Format("�ر�Tcpʧ�ܣ�{0}", ex));
            }

            this._client = null;

            this._isConnected = false;

            if (this.OnConnectFail != null)
            {
                try
                {
                    this.OnConnectFail(this, new TcpClientEventArgs("�û������Ͽ�"));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex));
                }
            }

            Trace.WriteLine("���ӹر�");
            return !bError;
        }


        /// <summary>
        /// ��������
        /// �첽����
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
                Trace.WriteLine(string.Format("��������ʱSocketException��{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("��������ʱObjectDisposedException��{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("��������ʱ��{0}", ex));
            }

            if (bError)
            {
                this._isConnected = false;
                if (this.OnConnectFail != null)
                {
                    try
                    {
                        this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex1));
                    }
                }

            }
        }

        /// <summary>
        /// �������ݻص�����
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

                // ͳ�Ʒ����ֽ���
                this._totalSendBytesNum += iSend;

                bError = false;
            }
            catch (SocketException sex)
            {
                Trace.WriteLine(string.Format("��������ʱSocketException��{0}", sex));
            }
            catch (ObjectDisposedException oex)
            {
                Trace.WriteLine(string.Format("��������ʱObjectDisposedException��{0}", oex));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("��������ʱ��{0}", ex));
            }

            if (!bError)
            {
                // ���ͳɹ������÷��������¼�
                if (this.OnSendData != null)
                {
                    try
                    {
                        this.OnSendData(
                            this
                            , new TcpClientEventArgs(iSend, null, string.Format("������{0}�ֽ�����", iSend)));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(string.Format("���÷��������¼�ʱ��{0}", ex));

                        this._isConnected = false;
                        if (this.OnConnectFail != null)
                        {
                            try
                            {
                                this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                            }
                            catch (Exception ex1)
                            {
                                Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex1));
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
                        this.OnConnectFail(this, new TcpClientEventArgs("���ӶϿ�"));
                    }
                    catch (Exception ex1)
                    {
                        Trace.WriteLine(string.Format("��������ʧ���¼�ʱ��{0}", ex1));
                    }
                }

            }
        }
    }
}
