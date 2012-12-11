using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI��Ҫ����Ϣ
    /// </summary>
    public class UIModelItem
    {
        /// <summary>
        /// UI��Ϣ
        /// </summary>
        /// <param name="dicCheckItem">ѡ����</param>
        /// <param name="belongProperty">��������</param>
        public UIModelItem(Dictionary<string, bool> dicCheckItem, EntityParamField belongProperty) 
        {
            _dicCheckItem = dicCheckItem;
            _belongProperty = belongProperty;
        }

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

        private EntityParamField _belongProperty;
        /// <summary>
        /// ��������
        /// </summary>
        public EntityParamField BelongProperty
        {
            get { return _belongProperty; }
        }
    }
}
