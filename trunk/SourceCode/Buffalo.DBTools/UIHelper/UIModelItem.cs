using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI��Ҫ����Ϣ
    /// </summary>
    public class UIModelItem
    {
        private Dictionary<string, bool> _dicCheckItem;

        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName) 
        {
            bool ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret)) 
            {
                return ret;
            }
            return false;
        }
    }
}
