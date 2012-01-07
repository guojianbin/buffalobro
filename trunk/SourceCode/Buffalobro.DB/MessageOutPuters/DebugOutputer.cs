using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Buffalobro.DB.MessageOutPuters
{
    public class DebugOutputer : IMessageOutputer
    {
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="messName"></param>
        /// <param name="messInfo"></param>
        public void Output(string messName, string messInfo) 
        {
            Debug.WriteLine(messName + ":" + messInfo);
        }
    }
}
