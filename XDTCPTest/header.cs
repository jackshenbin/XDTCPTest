using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace XDTCPTest
{
    /// <summary>
    /// ͳһ��ͷ
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HEAD
    {
        /// <summary>
        /// ��ͷ��־
        /// </summary>
        public byte flag1;
        /// <summary>
        /// <summary>
        /// ��ͷ��־
        /// </summary>
        public byte flag2;
        /// Э��汾 ��:1
        /// </summary>
        public byte  version;
        /// <summary>
        /// ��Ϣ���ȣ�2�ֽ���� 
        /// </summary>
        public short len;
        /// <summary>
        /// �������ţ��ɷ������ɣ����շ���ԭֵ����
        /// </summary>
        public byte  workflow;
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public byte messagetype;
        /// <summary>
        /// ���͵�ַ
        /// </summary>
        public byte sendaddr;
        /// <summary>
        /// ��������
        /// </summary>
        public byte sendtype;
        /// <summary>
        /// ���յ�ַ
        /// </summary>
        public byte recvaddr;
        /// <summary>
        /// ��������
        /// </summary>
        public byte recvtype;
    }
}
