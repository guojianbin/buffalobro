using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ������ֵ
    /// </summary>
    public class ContextValue
    {
        public readonly static ContextValue Current = new ContextValue();

        /// <summary>
        /// �Ƿ�Web��Ŀ
        /// </summary>
        private bool _isWeb = CommonMethods.IsWebContext;

        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <param name="key">��ȡֵ�ļ�</param>
        /// <returns></returns>
        public object this[string key]
        {
            get 
            {
                if (_isWeb)
                {
                    return System.Web.HttpContext.Current.Items[key];
                }
                else
                {
                    return System.Runtime.Remoting.Messaging.CallContext.GetData(key);
                }
            }
            set 
            {
                if (_isWeb)
                {
                    System.Web.HttpContext.Current.Items[key]=value;
                }
                else
                {
                    System.Runtime.Remoting.Messaging.CallContext.SetData(key,value);
                }
            }
        }
    }
}
