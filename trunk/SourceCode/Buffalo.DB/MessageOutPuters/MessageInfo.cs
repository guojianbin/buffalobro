using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
    public class MessageInfo:Dictionary<string,object>
    {
        public const string Type = "Type";
        public const string ExtendType = "ExtendType";
        public const string Value = "Value";
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public object GetValue(string key) 
        {
            object ret = null;
            if(this.TryGetValue(key,out ret))
            {
                return ret;
            }
            return ret;
        }

    }
}
