using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Buffalo.DB.MessageOutPuters
{
    public class DebugOutputer : IMessageOutputer
    {
        /// <summary>
        ///  ‰≥ˆ–≈œ¢
        /// </summary>
        /// <param name="messName"></param>
        /// <param name="messInfo"></param>
        public void Output(string messName, string messType, string[] mess) 
        {
            StringBuilder smsg = new StringBuilder(mess.Length * 20);
            foreach (string msg in mess)
            {
                smsg.Append(msg);

                smsg.Append(",");
            }

            Debug.WriteLine(messName+"["+messType + "]:" + smsg.ToString());
        }
    }
}
