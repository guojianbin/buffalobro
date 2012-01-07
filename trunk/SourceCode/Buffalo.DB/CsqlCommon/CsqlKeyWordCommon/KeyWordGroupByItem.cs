using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordGroupByItem:CsqlQuery
    {
        private IList<CsqlParamHandle> paramhandles;

        /// <summary>
        /// Where关键字项
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordGroupByItem(IList<CsqlParamHandle> paramhandles, CsqlQuery previous)
            : base(previous) 
        {
            this.paramhandles = paramhandles;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }
        ///// <summary>
        ///// 字段集合
        ///// </summary>
        //internal CsqlParamHandle[] Paramhandles 
        //{
        //    get 
        //    {
        //        return paramhandles;
        //    }
        //}

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public KeyWordHavingItem Having(CsqlCondition condition)
        {
            KeyWordHavingItem item = new KeyWordHavingItem(condition, this);
            return item;
        }
        ///// <summary>
        ///// 查询范围
        ///// </summary>
        ///// <param name="star">开始条数</param>
        ///// <param name="totleRecord">显示条数</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totleRecord)
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totleRecord, this);
        //    return item;
        //}
        internal override void Tran(KeyWordInfomation info)
        {
            info.IsWhere = true;
            string ret = "";
            for (int i = 0; i < paramhandles.Count; i++)
            {
                CsqlParamHandle prm = paramhandles[i];
                ret += prm.DisplayValue(info);
                if (i < paramhandles.Count - 1)
                {
                    ret += ",";
                }
            }
            SelectCondition con = info.Condition as SelectCondition;
            if (con != null)
            {
                con.HasGroup = true;
            }
            info.Condition.GroupBy.Append(ret);
            info.IsWhere = false;
            //return " group by " + ret;
        
        }
    }
}
