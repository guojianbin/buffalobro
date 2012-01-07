using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.QueryConditions
{
    public class ShowEntityCollection : List<CsqlEntityTableHandle>
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
        public new void Add(CsqlEntityTableHandle prm)
        {
            _belongList.HasInner = true;
            this.Add(prm);
        }

        /// <summary>
        /// ���һ������ֶ�
        /// </summary>
        /// <param name="prm"></param>
        public void AddRang(params CsqlEntityTableHandle[] prms)
        {
            _belongList.HasInner = true;
            this.AddRang(prms);
        }
    }
}
