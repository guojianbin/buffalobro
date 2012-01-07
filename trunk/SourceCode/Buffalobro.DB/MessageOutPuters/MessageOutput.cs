using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.MessageOutPuters
{
    /// <summary>
    /// ��Ϣ�����
    /// </summary>
    public class MessageOutput
    {
        private static IMessageOutputer _debugOutputer = new DebugOutputer();


        private IMessageOutputer _defaultOutputer =null;

        /// <summary>
        /// ��Ϣ�����
        /// </summary>
        public MessageOutput() 
        {
#if DEBUG
            _defaultOutputer = _debugOutputer;
#endif
        }

        /// <summary>
        /// Ĭ�ϵ���Ϣ�����
        /// </summary>
        public IMessageOutputer DefaultOutputer
        {
            get { return _defaultOutputer; }
            set { _defaultOutputer = value; }
        }

        /// <summary>
        /// �ж��Ƿ��������
        /// </summary>
        public bool HasOutput 
        {
            get 
            {
                return _defaultOutputer != null;
            }
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messName">��Ϣ����������</param>
        /// <param name="messInfo">��Ϣ</param>
        /// <returns></returns>
        public bool OutPut(string messName, string messInfo) 
        {
            if (_defaultOutputer != null)
            {
                _defaultOutputer.Output(messName, messInfo);
                return true;
            }
            return false;
        }
    }
}
