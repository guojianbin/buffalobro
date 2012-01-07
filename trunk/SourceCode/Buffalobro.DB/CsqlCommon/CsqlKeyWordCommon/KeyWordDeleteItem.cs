using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordDeleteItem: CsqlQuery
    {
        private CsqlTableHandle table;
        
        /// <summary>
        /// Delete关键字项
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordDeleteItem(CsqlTableHandle table, CsqlQuery previous)
            : base(previous) 
        {
            this.table = table;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            table.FillInfo(info);
        }
        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal CsqlTableHandle Table
        //{
        //    get 
        //    {
        //        return table;
        //    }
        //}
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public KeyWordWhereItem Where(CsqlCondition condition)
        {
            KeyWordWhereItem item = new KeyWordWhereItem(condition, this);
            return item;
        }

        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new SQLCommon.QueryConditions.DeleteCondition(info.DBInfo);
                //info.ParamList = new SQLCommon.DbCommon.ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new SQLCommon.DbCommon.ParamList();
            }
            info.Condition.Tables.Append(table.DisplayValue(info));
        
        }
    }
}
