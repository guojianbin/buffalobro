using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// 回传信息
    /// </summary>
    public class ResponseInfo:IDisposable
    {
        private MemoryStream _stm;
        private Encoding _encoding;
        public ResponseInfo(Encoding encoding) 
        {
            _mimeType="text/html";
            _stm = new MemoryStream();
            _encoding = encoding;
        }

        private string _mimeType = null;

        /// <summary>
        /// 传送类型
        /// </summary>
        public string MimeType
        {
            get { return _mimeType; }
            set { _mimeType = value; }
        }

        /// <summary>
        /// 返回的内容
        /// </summary>
        public byte[] ResponseContent 
        {
            get 
            {
                return _stm.ToArray();
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="content"></param>
        public void Write(byte[] content) 
        {
            _stm.Write(content, 0, content.Length);
        }
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="content"></param>
        public void Write(string content)
        {
            byte[] byteStr=_encoding.GetBytes(content);
            Write(byteStr);
        }


        #region IDisposable 成员

        public void Dispose()
        {
            if (_stm != null) 
            {
                _stm.Close();
            }
        }

        #endregion
    }
}
