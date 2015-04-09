using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace XDTCPTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        XdTcpHelper xd;
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            xd = new XdTcpHelper();
            xd.OnConnected += xd_OnConnected;
            xd.OnDisConnected += xd_OnDisConnected;
            xd.OnReceiveData += xd_OnReceiveData;
            xd.Open(textBoxIP.Text, int.Parse(textBoxPort.Text));
            xd.OnReceiveLogin += xd_OnReceiveLogin;
            xd.OnReceiveGetDevChargeInfo += xd_OnReceiveGetDevChargeInfo;
            xd.OnReceiveGetDevVersion += xd_OnReceiveGetDevVersion;
            xd.OnReceiveNoteDevChargeStatus += xd_OnReceiveNoteDevChargeStatus;
            xd.OnReceiveNoteDevStatus += xd_OnReceiveNoteDevStatus;
            xd.OnReceiveSubscribrDevChargeStatus += xd_OnReceiveSubscribrDevChargeStatus;
            xd.OnReceiveSubscribrDevStatus += xd_OnReceiveSubscribrDevStatus;
        }

        void xd_OnReceiveData(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(xd_OnReceiveData), new object[] { sender, e });
            }
            else
            {
                listBox1.Items.Add( sender + System.Environment.NewLine);
            }
        }

        void xd_OnReceiveSubscribrDevStatus(SubscribeDevStatusRet obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<SubscribeDevStatusRet>(xd_OnReceiveSubscribrDevStatus), obj);
            }
            else
            {
                listBox1.Items.Add("xd_OnReceiveSubscribrDevStatus retFlag:" + obj.retFlag + System.Environment.NewLine);
            }
        }

        void xd_OnReceiveSubscribrDevChargeStatus(SubscribeDevChargeStatusRet obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<SubscribeDevChargeStatusRet>(xd_OnReceiveSubscribrDevChargeStatus), obj);
            }
            else
            {
                listBox1.Items.Add("xd_OnReceiveSubscribrDevChargeStatus retFlag:" + obj.retFlag + System.Environment.NewLine);
            }
        }

        void xd_OnReceiveNoteDevStatus(DevStatusNote obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<DevStatusNote>(xd_OnReceiveNoteDevStatus), obj);
            }
            else
            {
                string msg = string.Format("xd_OnReceiveNoteDevStatus devID:{0}"
                    + ",IsOnline:{1}"
                    + ",ServiceStat:{2}"
                    + ",UserID:{3}"
                    , obj.DevID
                    , obj.IsOnline
                    , obj.ServiceStat
                    , obj.UserID
                    );
                listBox1.Items.Add(msg + System.Environment.NewLine);
            }
        }

        void xd_OnReceiveNoteDevChargeStatus(DevChargeStatusNote obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<DevChargeStatusNote>(xd_OnReceiveNoteDevChargeStatus), obj);
            }
            else
            {
                string msg = string.Format("xd_OnReceiveNoteDevChargeStatus devID:{0}"
                    + ",ChongDianShuChuDianLiu:{1}"
                    + ",ChongDianShuChuDianYa:{2}"
                    + ",LianJieQueRenKaiGuanZhuangTai:{3}"
                    + ",ShiFouLianJieDianChi:{4}"
                    + ",ShuChuJiDianQiZhuangTai:{5}"
                    + ",WorkStat:{6}"
                    + ",YouGongZongDianDu:{7}"
                    , obj.DevID
                    , obj.ChongDianShuChuDianLiu
                    , obj.ChongDianShuChuDianYa
                    , obj.LianJieQueRenKaiGuanZhuangTai
                    , obj.ShiFouLianJieDianChi
                    , obj.ShuChuJiDianQiZhuangTai
                    , obj.WorkStat
                    , obj.YouGongZongDianDu
                    );
                listBox1.Items.Add(msg + System.Environment.NewLine);
            }
        }

        void xd_OnReceiveGetDevVersion(GetDevVersionRet obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<GetDevVersionRet>(xd_OnReceiveGetDevVersion), obj);
            }
            else
            {
                string msg = string.Format("xd_OnReceiveGetDevVersion devID:{0}"
                    + ",FactoryID:{1}"
                    + ",DevSoftVersion:{2}"
                    + ",CRC:{3}"
                    , obj.DevID
                    , obj.FactoryID
                    , obj.DevSoftVersion
                    , obj.CRC
                    );
                listBox1.Items.Add(msg + System.Environment.NewLine);
            }
        }

        void xd_OnReceiveGetDevChargeInfo(GetDevChargeInfoRet obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<GetDevChargeInfoRet>(xd_OnReceiveGetDevChargeInfo), obj);
            }
            else
            {
                string msg = string.Format("xd_OnReceiveGetDevChargeInfo devID:{0}"
                    + ",DevType:{1}"
                    + ",FengZongDianDu:{2}"
                    + ",GuZongDianDu:{3}"
                    + ",JianZongDianDu:{4}"
                    + ",JiaoLiuShuChuDianLiu:{5}"
                    + ",JiaoLiuShuChuDianYaUXiang:{6}"
                    + ",JiaoLiuShuChuDianYaVXiang:{7}"
                    + ",JiaoLiuShuChuDianYaWXiang:{8}"
                    + ",JiaoLiuShuRuDianYaUXiang:{9}"
                    + ",JiaoLiuShuRuDianYaVXiang:{10}"
                    + ",JiaoLiuShuRuDianYaWXiang:{11}"
                    + ",PingZongDianDu:{12}"
                    + ",YouGongZongDianDu:{13}"
                    + ",ZhiLiuShuChuDianLiu:{14}"
                    + ",ZhiLiuShuChuDianYa:{15}"
                    , obj.DevID
                    , obj.DevType
                    , obj.FengZongDianDu
                    , obj.GuZongDianDu
                    , obj.JianZongDianDu
                    , obj.JiaoLiuShuChuDianLiu
                    , obj.JiaoLiuShuChuDianYaUXiang
                    , obj.JiaoLiuShuChuDianYaVXiang
                    , obj.JiaoLiuShuChuDianYaWXiang
                    , obj.JiaoLiuShuRuDianYaUXiang
                    , obj.JiaoLiuShuRuDianYaVXiang
                    , obj.JiaoLiuShuRuDianYaWXiang
                    , obj.PingZongDianDu
                    , obj.YouGongZongDianDu
                    , obj.ZhiLiuShuChuDianLiu
                    , obj.ZhiLiuShuChuDianYa
                    );
                listBox1.Items.Add(msg + System.Environment.NewLine);
            }
        }
 
        void xd_OnDisConnected(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler( xd_OnDisConnected), new object[]{sender,e});
            }
            else
            {
                listBox1.Items.Add("xd_OnDisConnected :" + sender + System.Environment.NewLine);
            }
        }
 
        void xd_OnConnected(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new EventHandler(xd_OnConnected), new object[] { sender, e });
            }
            else
            {
                listBox1.Items.Add("xd_OnConnected :"+sender + System.Environment.NewLine);
            }
        }
 
        void xd_OnReceiveLogin(LoginRet obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LoginRet>(xd_OnReceiveLogin), obj);
            }
            else
            {
                float a = 0f;
                a = xd.Convert2Float(obj.ClientType, 3);
                listBox1.Items.Add("xd_OnReceiveLogin ClientName:" + obj.ClientName + ",ClientType:" + obj.ClientType + ",ret:" + obj.ret + System.Environment.NewLine);
            }
        }
 
 
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            xd.SendLogin("admin",1);
 
        }
 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (xd != null)
            {
                xd.Close();
                xd = null;
            }
        }
 
        private void buttonDisConnect_Click(object sender, EventArgs e)
        {
            if (xd != null)
            {
                xd.Close();
                xd = null;
            }
        }
 
        private void buttonSendSubscribeDevStatus_Click(object sender, EventArgs e)
        {
            xd.SendSubscribeDevStatus();
        }
 
        private void buttonSendSubscribeDevChargeStatus_Click(object sender, EventArgs e)
        {
            xd.SendSubscribeDevChargeStatus();
        }
 
        private void buttonSendGetDevChargeInfo_Click(object sender, EventArgs e)
        {
            xd.SendGetDevChargeInfo("1234567890123456");
        }
 
        private void buttonSendGetDevVersion_Click(object sender, EventArgs e)
        {
            xd.SendGetDevVersion("1234567890123456");
 
        }
    }
}
