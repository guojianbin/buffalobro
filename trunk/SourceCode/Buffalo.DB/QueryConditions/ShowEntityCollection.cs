using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.QueryConditions
{
    public class ShowEntityCollection : List<BQLEntityTableHandle>
    {
        private ScopeList _belongList;//�����ļ���

        /// <summary>
        /// Ҫ��ʵ��ʵ�弯��
        /// </summary>
        /// <param name="belong"></param>
        internal ShowEntityCollection(ScopeList belong) 
        {
            _belongList = belong;
        }

        /// <summary>
        /// ���һ����
        /// </summary>
        /// <param name="prm"></param>
        public new void Add(BQLEntityTableHandle prm)
        {
            _belongList.HasInner = true;
            base.Add(prm);
        }

        /// <summary>
        /// ���һ������ֶ�
        /// </summary>
        /// <param name="prm"></param>
        public void AddRange(params BQLEntityTableHandle[] prms)
        {
            _belongList.HasInner = true;
            base.AddRange(prms);
        }
    }
}
