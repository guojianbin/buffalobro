using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI需要的信息
    /// </summary>
    public class UIModelItem
    {
        private Dictionary<string, bool> _dicCheckItem;

        /// <summary>
        /// 获取是否选中项
        /// </summary>
        /// <param name="itemName">项名称</param>
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
