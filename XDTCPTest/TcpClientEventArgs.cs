using System;
using System.Collections.Generic;
using System.Text;

namespace XDTCPTest
{
    /// <summary>
    /// �¼�
    /// </summary>
    public class TcpClientEventArgs : EventArgs
    {
        #region ===˽�б���===
 
        /// <summary>
        /// ���ݴ�С
        /// </summary>
        private int _dataSize;
 
        /// <summary>
        /// ����
        /// </summary>
        private byte[] _data;
 
        /// <summary>
        /// ����
        /// </summary>
        private string _description;
 
        #endregion
 
        #region ===���캯��===
 
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
 
        #region ===��������===
 
        /// <summary>
        /// ���ݴ�С
        /// </summary>
        public int DataSize
        {
            get
            {
                return this._dataSize;
            }
        }
 
        /// <summary>
        /// ����
        /// </summary>
        public byte[] Data
        {
            get
            {
                return this._data;
            }
        }
 
        /// <summary>
        /// ����
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
