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
        public UIModelItem(Dictionary<string, object> dicCheckItem, EntityParamField belongProperty) 
        {
            _dicCheckItem = dicCheckItem;
            _belongProperty = belongProperty;
        }

        private Dictionary<string, object> _dicCheckItem;

        /// <summary>
        /// ��ȡ�Ƿ�ѡ����
        /// </summary>
        /// <param name="itemName">������</param>
        /// <returns></returns>
        public bool HasItem(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret)) 
            {
                if(ret is bool)
                {
                    return (bool)ret;
                }
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
