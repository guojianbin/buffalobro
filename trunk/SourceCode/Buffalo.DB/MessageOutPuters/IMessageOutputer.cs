using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
{
    public interface IMessageOutputer
    {
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messType">��Ϣ����</param>
        /// <param name="mess">��Ϣ</param>
        void Output(MessageType messType, MessageInfo mess);
    }
}
