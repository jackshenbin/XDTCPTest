using System;
using System.Collections.Generic;
using System.Text;

namespace XDTCPTest
{
    /// <summary>
    /// 事件
    /// </summary>
    public class TcpClientEventArgs : EventArgs
    {
        #region ===私有变量===
 
        /// <summary>
        /// 数据大小
        /// </summary>
        private int _dataSize;
 
        /// <summary>
        /// 数据
        /// </summary>
        private byte[] _data;
 
        /// <summary>
        /// 描述
        /// </summary>
        private string _description;
 
        #endregion
 
        #region ===构造函数===
 
        public TcpClientEventArgs(string description) : this(0, null, description)
        {
        }
 
        public TcpClientEventArgs(int dataSize, byte[] data, string description)
        {
            this._dataSize = dataSize;
 
            this._data = data;
 
            this._description = description;
        }
 
        #endregion
 
        #region ===公有属性===
 
        /// <summary>
        /// 数据大小
        /// </summary>
        public int DataSize
        {
            get
            {
                return this._dataSize;
            }
        }
 
        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }
 
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return this._description;
            }
        }
 
        #endregion
    }
}
