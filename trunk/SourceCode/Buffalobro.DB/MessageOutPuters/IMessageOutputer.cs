using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.MessageOutPuters
{
    public interface IMessageOutputer
    {
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messName">��Ϣ����������</param>
        /// <param name="messInfo">��Ϣ</param>
        void Output(string messName, string messInfo);
    }
}
