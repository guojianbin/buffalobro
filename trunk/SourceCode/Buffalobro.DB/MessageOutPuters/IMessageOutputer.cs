using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.MessageOutPuters
{
    public interface IMessageOutputer
    {
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="messName">信息发送者名称</param>
        /// <param name="messInfo">信息</param>
        void Output(string messName, string messInfo);
    }
}
