using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// �ش���Ϣ
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
        /// ��������
        /// </summary>
        public string MimeType
        {
            get { return _mimeType; }
            set { _mimeType = value; }
        }

        /// <summary>
        /// ���ص�����
        /// </summary>
        public byte[] ResponseContent 
        {
            get 
            {
                return _stm.ToArray();
            }
        }

        /// <summary>
        /// д������
        /// </summary>
        /// <param name="content"></param>
        public void Write(byte[] content) 
        {
            _stm.Write(content, 0, content.Length);
        }
        /// <summary>
        /// д������
        /// </summary>
        /// <param name="content"></param>
        public void Write(string content)
        {
            byte[] byteStr=_encoding.GetBytes(content);
            Write(byteStr);
        }


        #region IDisposable ��Ա

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
