using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordHavingItem : CsqlQuery
    {
        private CsqlCondition condition;

        /// <summary>
        /// Where关键字项
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordHavingItem(CsqlCondition condition, CsqlQuery previous)
            : base(previous) 
        {
            this.condition = condition;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }
        ///// <summary>
        ///// 条件
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
