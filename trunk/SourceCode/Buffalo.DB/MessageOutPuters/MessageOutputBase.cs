using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// ���������
    /// </summary>
    public abstract class MessageOutputBase
    {
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="mess">��Ϣ</param>
        public abstract void OutPut(MessageType messType, MessageInfo mess);

        
        /// <summary>
        /// ���SQLʱ���Ƿ���������Ʊ���ֵ��Hex
        /// </summary>
        public virtual bool ShowBinary
        {
            get { return false; }
        }

        /// <summary>
        /// ���SQLʱ������һ��ֵ�����ַ��������������ʱ��������ֵ
        /// </summary>
        public virtual int HideTextLength
        {
            get { return 0; }
            
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="type">����</param>
        /// <param name="extendType">��չ����</param>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public void OutPut(MessageType messType, string type, string extendType, string value)
        {
            MessageInfo mess = new MessageInfo();
            mess.Type = type;
            if (extendType != null)
            {
                mess.ExtendType = extendType;
            }
            mess.Value = value;
            OutPut(messType, mess);
        }
    }
}
