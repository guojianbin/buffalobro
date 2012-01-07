using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.QueryConditions;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordInserItem : CsqlQuery
    {
        private CsqlTableHandle tableHandle;
        /// <summary>
        /// Insert关键字项
        /// </summary>
        /// <param name="tableHandle">要插入的表</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordInserItem(CsqlTableHandle tableHandle, CsqlQuery previous)
            : base(previous) 
        {
            this.tableHandle = tableHandle;
        }

        ///// <summary>
        ///// 要插入的表
        ///// </summary>
        //internal CsqlTableHandle TableHandle 
        //{
        //    get 
        //    {
        //        return tableHandle;
        //    }
        //}
        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="paramhandles">字段</param>
        /// <returns></returns>
        public KeyWordInsertFieldItem Fields(params CsqlParamHandle[] paramhandles)
        {
            KeyWordInsertFieldItem fItem = new KeyWordInsertFieldItem(paramhandles, this);
            return fItem;
        }

        /// <summary>
        /// 插入一个查询集合
        /// </summary>
        /// <param name="query">查询</param>
        /// <returns></returns>
        public KeyWordInsertQueryItem ByQuery(CsqlQuery query)
        {
            return new KeyWordInsertQueryItem(query,this);
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            tableHandle.FillInfo(info);
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new InsertCondition(info.DBInfo);
                //info.ParamList = new ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new SQLCommon.DbCommon.ParamList();
            }
            info.Condition.Tables.Append(tableHandle.DisplayValue(info));
            
        
        }
    }
}
