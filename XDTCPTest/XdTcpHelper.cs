using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace XDTCPTest
{
    public class XdTcpHelper
    {
        static byte workflow = 0;

        TcpHelper m_tcpManager = new TcpHelper();

        bool m_isLogin = false;
        string m_userName = "";
        int m_userType = 0;

        public event EventHandler OnConnected;
        public event EventHandler OnDisConnected;
        public event EventHandler OnReceiveData;

        public event Action<HB> OnReceiveHB;
        public event Action<LoginRet> OnReceiveLogin;
        public event Action<SubscribeDevStatusRet> OnReceiveSubscribrDevStatus;
        public event Action<DevStatusNote> OnReceiveNoteDevStatus;
        public event Action<SubscribeDevChargeStatusRet> OnReceiveSubscribrDevChargeStatus;
        public event Action<DevChargeStatusNote> OnReceiveNoteDevChargeStatus;
        public event Action<GetDevChargeInfoRet> OnReceiveGetDevChargeInfo;
        public event Action<GetDevVersionRet> OnReceiveGetDevVersion;


        public void Open(string ip, int port)
        {
            m_tcpManager.OnConnectFail += m_tcpManager_OnConnectFail;
            m_tcpManager.OnConnectSuccess += m_tcpManager_OnConnectSuccess;
            m_tcpManager.OnReceiveData += m_tcpManager_OnReceiveData;
            m_tcpManager.NewTcpHelper(ip, port);
            //m_tcpManager.OnSendData += m_tcpManager_OnSendData;

        }

        public void Close()
        {
            m_isLogin = false;
            m_userName = "";
            m_userType = 0;
            m_tcpManager.Stop();
        }

        void m_tcpManager_OnReceiveData(object sender, TcpClientEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Len:" + e.DataSize+",Data:");
            for (int i = 0; i < e.DataSize; i++)
            {
                sb.Append(e.Data[i].ToString("X2"));
            }
            if (OnReceiveData != null)
                OnReceiveData(sb.ToString(), null);

            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HEAD)));
            Marshal.Copy(e.Data, 0, pdata, Marshal.SizeOf(typeof(HEAD)));
            HEAD hd = (HEAD)Marshal.PtrToStructure(pdata, typeof(HEAD));
            if (!CheckCRC(e.Data))
            {
                return;
            }
            int len = e.Data[3] + e.Data[4] << 8 - 8;
            byte[] body = new byte[len];
            Array.Copy(e.Data, 11, body, 0, len);
            switch ((EnumProtocolType)hd.messagetype)
            {
                case EnumProtocolType.RET_HEART_BEAT:
                    OnReceiveData_HB(body);
                    break;
                case EnumProtocolType.RET_USER_LOGIN:
                    OnReceiveData_LOGIN(body);
                    break;
                case EnumProtocolType.RET_SUBSCTIBR_DEV_STATUS:
                    OnReceiveData_SubscribrDevStatus(body);
                    break;
                case EnumProtocolType.NOTE_DEV_STATUS:
                    OnReceiveData_NoteDevStatus(body);
                    break;
                case EnumProtocolType.RET_SUBSCTIBR_DEV_CHARGE_STATUS:
                    OnReceiveData_SubscribrDevChargeStatus(body);
                    break;
                case EnumProtocolType.NOTE_DEV_CHARGE_STATUS:
                    OnReceiveData_NoteDevChargeStatus(body);
                    break;
                case EnumProtocolType.RET_GET_DEV_CHARGE_INFO:
                    OnReceiveData_GetDevChargeInfo(body);
                    break;
                case EnumProtocolType.RET_GET_DEV_VERSION:
                    OnReceiveData_GetDevVersion(body);
                    break;

                default:
                    break;
            }
        }

        private void OnReceiveData_GetDevVersion(byte[] body)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GetDevVersionRet)));
            Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(GetDevVersionRet)));
            GetDevVersionRet msg = (GetDevVersionRet)Marshal.PtrToStructure(pdata, typeof(GetDevVersionRet));

            if (OnReceiveGetDevVersion != null)
                OnReceiveGetDevVersion(msg);
        }

        private void OnReceiveData_GetDevChargeInfo(byte[] body)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(GetDevChargeInfoRet)));
            Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(GetDevChargeInfoRet)));
            GetDevChargeInfoRet msg = (GetDevChargeInfoRet)Marshal.PtrToStructure(pdata, typeof(GetDevChargeInfoRet));

            if (OnReceiveGetDevChargeInfo != null)
                OnReceiveGetDevChargeInfo(msg);
        }

        private void OnReceiveData_NoteDevChargeStatus(byte[] body)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DevChargeStatusNote)));
            Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(DevChargeStatusNote)));
            DevChargeStatusNote msg = (DevChargeStatusNote)Marshal.PtrToStructure(pdata, typeof(DevChargeStatusNote));

            if (OnReceiveNoteDevChargeStatus != null)
                OnReceiveNoteDevChargeStatus(msg);
        }

        private void OnReceiveData_SubscribrDevChargeStatus(byte[] body)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SubscribeDevChargeStatusRet)));
            Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(SubscribeDevChargeStatusRet)));
            SubscribeDevChargeStatusRet msg = (SubscribeDevChargeStatusRet)Marshal.PtrToStructure(pdata, typeof(SubscribeDevChargeStatusRet));

            if (OnReceiveSubscribrDevChargeStatus != null)
                OnReceiveSubscribrDevChargeStatus(msg);
        }

        private void OnReceiveData_NoteDevStatus(byte[] body)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(DevStatusNote)));
            Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(DevStatusNote)));
            DevStatusNote msg = (DevStatusNote)Marshal.PtrToStructure(pdata, typeof(DevStatusNote));

            if (OnReceiveNoteDevStatus != null)
                OnReceiveNoteDevStatus(msg);
        }

        private void OnReceiveData_SubscribrDevStatus(byte[] body)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SubscribeDevStatusRet)));
            Marshal.Copy(body, 0, pdata, Marshal.SizeOf(typeof(SubscribeDevStatusRet)));
            SubscribeDevStatusRet msg = (SubscribeDevStatusRet)Marshal.PtrToStructure(pdata, typeof(SubscribeDevStatusRet));

            if (OnReceiveSubscribrDevStatus != null)
                OnReceiveSubscribrDevStatus(msg);
        }
        private void OnReceiveData_HB(byte[] data)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(HB)));
            Marshal.Copy(data, 0, pdata, Marshal.SizeOf(typeof(HB)));
            HB hb = (HB)Marshal.PtrToStructure(pdata, typeof(HB));

            if (OnReceiveHB != null)
                OnReceiveHB(hb);
        }
        private void OnReceiveData_LOGIN(byte[] data)
        {
            IntPtr pdata = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LoginRet)));
            Marshal.Copy(data, 0, pdata, Marshal.SizeOf(typeof(LoginRet)));
            LoginRet hb = (LoginRet)Marshal.PtrToStructure(pdata, typeof(LoginRet));

            if (hb.ret == 0)
            {
                m_userName = hb.ClientName;
                m_userType = hb.ClientType;
                m_isLogin = true;
                new System.Threading.Thread(ThHBSend).Start();
            }
            if (OnReceiveLogin != null)
                OnReceiveLogin(hb);
        }

        void ThHBSend()
        {
            int index = 0;
            while (m_isLogin)
            {
                System.Threading.Thread.Sleep(1000);
                index++;
                if (index % 10 == 0)
                    SendHB();
            }
        }

        void m_tcpManager_OnConnectFail(object sender, TcpClientEventArgs e)
        {
            m_isLogin = false;
            if (OnDisConnected != null)
                OnDisConnected(e.Description, null);
        }

        void m_tcpManager_OnConnectSuccess(object sender, TcpClientEventArgs e)
        {
            if (OnConnected != null)
                OnConnected(e.Description, null);
        }


        private void SendHB()
        {
            HB hb = new HB { ClientName = m_userName, ClientType = (byte)m_userType };
            //IntPtr phb = Marshal.AllocHGlobal(Marshal.SizeOf(hb));
            ////System.Net.IPAddress.HostToNetworkOrder()
            //Marshal.StructureToPtr(hb,phb,false);
            //byte[] bhb = new byte[Marshal.SizeOf(hb)];
            //Marshal.Copy(phb, bhb, 0, Marshal.SizeOf(hb));
            ////byte[] hb = new byte[] { 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x01 };
            //Marshal.FreeHGlobal(phb);
            Send(hb, typeof(HB), EnumProtocolType.REQ_HEART_BEAT);

        }

        public void SendLogin(string userName, int userType)
        {
            LoginReq login = new LoginReq { ClientName = userName, ClientType = (byte)userType, ClientMac = GetMacAddress() };
            //IntPtr plogin = Marshal.AllocHGlobal(Marshal.SizeOf(login));
            //Marshal.StructureToPtr(login, plogin, false);
            //byte[] blogin = new byte[Marshal.SizeOf(login)];
            //Marshal.Copy(plogin, blogin, 0, Marshal.SizeOf(login));
            //Marshal.FreeHGlobal(plogin);
            Send(login, typeof(LoginReq), EnumProtocolType.REQ_USER_LOGIN);
        }

        public void SendSubscribeDevStatus()
        {
            SubscribeDevStatusReq msg = new SubscribeDevStatusReq { Flag = 1 };
            Send(msg, typeof(SubscribeDevStatusReq), EnumProtocolType.REQ_SUBSCTIBR_DEV_STATUS);

        }

        public void SendSubscribeDevChargeStatus()
        {
            SubscribeDevChargeStatusReq msg = new SubscribeDevChargeStatusReq { Flag = 1 };
            Send(msg, typeof(SubscribeDevChargeStatusReq), EnumProtocolType.REQ_SUBSCTIBR_DEV_CHARGE_STATUS);

        }

        public void SendGetDevChargeInfo(string devID)
        {
            GetDevChargeInfoReq msg = new GetDevChargeInfoReq { DevID = devID };
            Send(msg, typeof(GetDevChargeInfoReq), EnumProtocolType.REQ_GET_DEV_CHARGE_INFO);
        }

        public void SendGetDevVersion(string devID)
        {
            GetDevVersionReq msg = new GetDevVersionReq { DevID = devID };
            Send(msg, typeof(GetDevVersionReq), EnumProtocolType.REQ_GET_DEV_VERSION);
        }


        private void Send(object st, Type sttype, EnumProtocolType protocoltype)
        {

            IntPtr plogin = Marshal.AllocHGlobal(Marshal.SizeOf(st));
            Marshal.StructureToPtr(st, plogin, false);
            byte[] body = new byte[Marshal.SizeOf(st)];
            Marshal.Copy(plogin, body, 0, Marshal.SizeOf(st));
            Marshal.FreeHGlobal(plogin);

            HEAD hd = CreateHeaderByProtocolType(protocoltype);
            hd.len = (short)(8 + body.Length);
            short crc = (short)((hd.len & 0xff) + ((hd.len >> 8) & 0xff) + hd.messagetype + hd.recvaddr + hd.recvtype + hd.sendaddr + hd.sendtype + hd.version + hd.workflow);
            for (int i = 0; i < body.Length; i++)
            {
                crc += (short)body[i];
            }
            //hd.len = System.Net.IPAddress.HostToNetworkOrder(hd.len);
            IntPtr phd = Marshal.AllocHGlobal(Marshal.SizeOf(hd));
            Marshal.StructureToPtr(hd, phd, false);
            byte[] protocol = new byte[Marshal.SizeOf(hd) + body.Length + 2];
            Marshal.Copy(phd, protocol, 0, Marshal.SizeOf(hd));
            Array.Copy(body, 0, protocol, Marshal.SizeOf(hd), body.Length);
            crc = System.Net.IPAddress.HostToNetworkOrder(crc);
            protocol[protocol.Length - 2] = (byte)((crc >> 8) & 0xff);
            protocol[protocol.Length - 1] = (byte)(crc & 0xff);
            Marshal.FreeHGlobal(phd);
            m_tcpManager.SendData(protocol);

        }
        /// <summary>
        /// 摘要:依据协议类型,创建包头
        /// </summary>
        /// <param name="ptype">协议类型</param>
        /// <returns>协议包包头</returns>
        private static HEAD CreateHeaderByProtocolType(EnumProtocolType ptype)
        {
            HEAD h = new HEAD();
            h.flag1 = 0x58;
            h.flag2 = 0x44;
            h.messagetype = (byte)ptype;
            h.version = 0x35;
            h.workflow = unchecked(workflow++);
            h.recvaddr = 0;
            h.recvtype = 0;
            h.sendaddr = 0;
            h.sendtype = 0;

            return h;
        }
        private bool CheckCRC(byte[] data)
        {
            return true;
        }

        private static string GetMacAddress()
        {
            string mac = "10BF483ED5AC";
            //System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_NetworkAdapterConfiguration");
            //System.Management.ManagementObjectCollection moc = mc.GetInstances();
            //foreach (System.Management.ManagementObject item in moc)
            //{
            //    if ((bool)item["IPEnable"])
            //    {
            //        mac = item["MacAddress"].ToString();
            //        break;
            //    }
            //}
            //moc = null;
            //mc = null;
            return mac;
        }

        public float Convert2Float(int val,uint sit=1)
        {
            if (val == 0)
                return 0f;
            if (sit > 5)
                return 0f;
            double beichushu = Math.Pow(10, (double)sit);
            return (float)(val / beichushu);
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HB
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ClientName;//不足补0，包含英文和数字
        public byte ClientType;//1：系统管理员 2：卡管理员 3：设备维护员
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LoginReq
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ClientName;//不足补0，包含英文和数字
        public byte ClientType;//1：系统管理员 2：卡管理员 3：设备维护员
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ClientMac;//客户端MAC地址
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LoginRet
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ClientName;//不足补0，包含英文和数字
        public byte ClientType;//1：系统管理员 2：卡管理员 3：设备维护员
        public byte ret;//0：注册成功 1：注册失败 3：无此用户 4：MAC地址错误

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevStatusReq
    {
        public byte Flag;//1：定制，0：退订
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevStatusRet
    {
        public byte retFlag;//0：定制或者退订成功；1：定制或者退订失败
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DevStatusNote
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string UserID;//用户编号
        public byte IsOnline;//在线状态 1：在线；0：离线
        public byte ServiceStat;//服务状态 1：服务状态 2：暂停服务 3：维护状态 4：测试状态
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevChargeStatusReq
    {
        public byte Flag;//1：定制，0：退订
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SubscribeDevChargeStatusRet
    {
        public byte retFlag;//0：定制或者退订成功；1：定制或者退订失败

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DevChargeStatusNote
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevID;//充电桩编号
        public UInt16 ChongDianShuChuDianYa;//充电输出电压 精确到小数点后一位
        public UInt16 ChongDianShuChuDianLiu;//充电输出电流 精确到小数点后二位
        public byte ShuChuJiDianQiZhuangTai;//输出继电器状态 布尔型, 变化上传;0关（未输出），1开（输出）
        public byte LianJieQueRenKaiGuanZhuangTai;//连接确认开关状态 布尔型, 变化上传;0关（未好），1开（好）
        public UInt32 YouGongZongDianDu;//有功总电度 精确到小数点后一位
        public byte ShiFouLianJieDianChi;//是否连接电池 布尔型, 变化上传，0：否，1：是
        public byte WorkStat;//工作状态 0离线，1故障 2待机，3工作

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevChargeInfoReq
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevID;//充电桩编号
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevChargeInfoRet
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevID;//充电桩编号
        public byte DevType;//0x02：单相交流离散桩 0x12：三相相交流离散桩 0x03：三相直流离散桩 0x13：单相直流离散桩
        public Int16 JiaoLiuShuRuDianYaUXiang;//交流输入电压U相 精确到小数点后一位
        public Int16 JiaoLiuShuRuDianYaVXiang;//交流输入电压V相 精确到小数点后一位
        public Int16 JiaoLiuShuRuDianYaWXiang;//交流输入电压W相 精确到小数点后一位
        public Int16 JiaoLiuShuChuDianYaUXiang;//交流输出电压U相 精确到小数点后一位
        public Int16 JiaoLiuShuChuDianYaVXiang;//交流输出电压V相 精确到小数点后一位
        public Int16 JiaoLiuShuChuDianYaWXiang;//交流输出电压W相 精确到小数点后一位
        public Int16 JiaoLiuShuChuDianLiu;//交流输出电流 精确到小数点后二位
        public Int16 ZhiLiuShuChuDianYa;//直流输出电压 精确到小数点后一位
        public Int16 ZhiLiuShuChuDianLiu;//直流输出电流 精确到小数点后二位
        public Int32 YouGongZongDianDu;//有功总电度 精确到小数点后一位
        public Int32 JianZongDianDu;//尖总电度 精确到小数点后一位
        public Int32 FengZongDianDu;//峰总电度 精确到小数点后一位
        public Int32 PingZongDianDu;//平总电度 精确到小数点后一位
        public Int32 GuZongDianDu;//谷总电度 精确到小数点后一位

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevVersionReq
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevID;//充电桩编号
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GetDevVersionRet
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevID;//充电桩编号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string FactoryID;//厂商编号
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string DevSoftVersion;//软件版本号
        public Int32 CRC;//唯一CRC校验码
    }

}
