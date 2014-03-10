using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Buffalo.Kernel;
using System.Collections;

namespace Buffalo.DB.MessageOutPuters
{
    public class DebugOutputer : IMessageOutputer
    {
        /// <summary>
        ///  ‰≥ˆ–≈œ¢
        /// </summary>
        /// <param name="messType"></param>
        /// <param name="mess"></param>
        public void Output(MessageType messType,MessageInfo mess) 
        {
            string messName = messType.ToString();
            StringBuilder smsg = new StringBuilder();
            smsg.Append(messName);

            object val=mess.GetValue(MessageInfo.Type);
            smsg.Append("[");
            if (val!=null) 
            {
                smsg.Append(val);
            }
            val = mess.GetValue(MessageInfo.ExtendType);
            if (val != null)
            {
                smsg.Append(","+val);
            }
            smsg.Append("]");

            val = mess.GetValue(MessageInfo.Value);
            if (val != null)
            {
                smsg.Append(":"+val);
            }

            Debug.WriteLine(smsg.ToString());
        }
    }
}
