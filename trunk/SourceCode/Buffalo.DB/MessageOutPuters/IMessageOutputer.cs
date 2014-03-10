using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
{
    public interface IMessageOutputer
    {
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messType">信息类型</param>
        /// <param name="mess">信息</param>
        void Output(MessageType messType, MessageInfo mess);
    }
}
