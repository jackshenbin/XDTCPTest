using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Serialization;

namespace XDTCPTest
{
    /// <summary>
    /// 统一包头
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HEAD
    {
        /// <summary>
        /// 包头标志
        /// </summary>
        public byte flag1;
        /// <summary>
        /// <summary>
        /// 包头标志
        /// </summary>
        public byte flag2;
        /// 协议版本 如:1
        /// </summary>
        public byte  version;
        /// <summary>
        /// 消息长度，2字节组成 
        /// </summary>
        public short len;
        /// <summary>
        /// 工作流号，由发起方生成，接收方按原值返回
        /// </summary>
        public byte  workflow;
        /// <summary>
        /// 消息类型
        /// </summary>
        public byte messagetype;
        /// <summary>
        /// 发送地址
        /// </summary>
        public byte sendaddr;
        /// <summary>
        /// 发送类型
        /// </summary>
        public byte sendtype;
        /// <summary>
        /// 接收地址
        /// </summary>
        public byte recvaddr;
        /// <summary>
        /// 接收类型
        /// </summary>
        public byte recvtype;
    }
}
