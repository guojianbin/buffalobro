using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Buffalobro.DB.MessageOutPuters
{
    public class DebugOutputer : IMessageOutputer
    {
        /// <summary>
        ///  ‰≥ˆ–≈œ¢
        /// </summary>
        /// <param name="messName"></param>
        /// <param name="messInfo"></param>
        public void Output(string messName, string messInfo) 
        {
            Debug.WriteLine(messName + ":" + messInfo);
        }
    }
}
