using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordWhereItem:CsqlQuery
    {
        private CsqlCondition _condition;

        /// <summary>
        /// Where�ؼ�����
        /// </summary>
        /// <param name="condition">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordWhereItem(CsqlCondition condition, CsqlQuery previous)
            : base(previous) 
        {
            this._condition = condition;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            _condition.FillInfo(info);
        }
        ///// <summary>
        ///// ����
        ///// </summary>
        //internal CsqlCondition Condition 
        //{
        //    get 
        //    {
        //        return condition;
        //    }
        //}
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
            return item;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="condition">����</param>
        /// <returns></returns>
        public KeyWordHavingItem Having(CsqlCondition condition)
        {
            KeyWordHavingItem item = new KeyWordHavingItem(condition, this);
            return item;
        }
        ///// <summary>
        ///// ��ѯ��Χ
        ///// </summary>
        ///// <param name="star">��ʼ����</param>
        ///// <param name="totleRecord">��ʾ����</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totleRecord)
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totleRecord, this);
        //    return item;
        //}
        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            string strCondition = _condition.DisplayValue(info);
            info.Condition.Condition.Append(strCondition);
            info.IsWhere = false;
        }
    }
}
