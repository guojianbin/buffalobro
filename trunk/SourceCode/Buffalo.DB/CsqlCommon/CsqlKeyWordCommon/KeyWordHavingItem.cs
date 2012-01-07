using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordHavingItem : CsqlQuery
    {
        private CsqlCondition condition;

        /// <summary>
        /// Where�ؼ�����
        /// </summary>
        /// <param name="condition">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordHavingItem(CsqlCondition condition, CsqlQuery previous)
            : base(previous) 
        {
            this.condition = condition;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

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

        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            info.Condition.Having.Append(condition.DisplayValue(info));
            info.IsWhere = false;
        }
    }
}
