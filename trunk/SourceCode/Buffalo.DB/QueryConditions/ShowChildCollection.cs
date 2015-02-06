using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    public class ShowChildCollection : List<ShowChildItem>
    {
        /// <summary>
        /// ���һ����
        /// </summary>
        /// <param name="prm">��ѯ����</param>
        /// <param name="filter">ɸѡ����</param>
        public void Add(BQLEntityTableHandle prm,ScopeList filter)
        {
            if (_belongList != null)
            {
                _belongList.HasInner = true;
            }
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = filter;
            base.Add(item);
        }
        /// <summary>
        /// ���һ����
        /// </summary>
        /// <param name="prm">��ѯ����</param>
        public new void Add(BQLEntityTableHandle prm)
        {
            if (_belongList != null)
            {
                _belongList.HasInner = true;
            }
            ShowChildItem item = new ShowChildItem();
            item.ChildItem = prm;
            item.FilterScope = new ScopeList();
            base.Add(item);
        }
    }
}
