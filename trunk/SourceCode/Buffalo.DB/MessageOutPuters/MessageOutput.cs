using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
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
        public bool OutPut(MessageType messType, MessageInfo mess) 
        {
            if (_defaultOutputer != null)
            {
                _defaultOutputer.Output( messType, mess);
                return true;
            }
            return false;
        }

       /// <summary>
        /// �����Ϣ
       /// </summary>
       /// <param name="messType">��Ϣ����</param>
       /// <param name="type">����</param>
       /// <param name="extendType">��չ����</param>
       /// <param name="value">ֵ</param>
       /// <returns></returns>
        public bool OutPut(MessageType messType, string type,string extendType,string value)
        {
            MessageInfo mess = new MessageInfo();
            mess[MessageInfo.Type] = type;
            if (extendType != null) 
            {
                mess[MessageInfo.ExtendType] = extendType;
            }
            mess[MessageInfo.Value] = value;
            return OutPut(messType,mess);
        }
    }
}
