using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordUpdateItem : CsqlQuery
    {
        private CsqlTableHandle table;
        
        /// <summary>
        /// Update关键字项
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordUpdateItem(CsqlTableHandle table, CsqlQuery previous)
            : base(previous) 
        {
            this.table = table;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            table.FillInfo(info);
        }
        /// <summary>
        /// 要查询的字段
        /// </summary>
        internal CsqlTableHandle Table
        {
            get 
            {
                return table;
            }
        }
        /// <summary>
        /// 添加一个set项
        /// </summary>
        /// <param name="parameter">字段</param>
        /// <param name="valueItem">值</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(CsqlParamHandle parameter, CsqlValueItem valueItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(parameter, valueItem, this);
            return item;
        }
        /// <summary>
        /// 添加一个set项
        /// </summary>
        /// <param name="parameter">字段</param>
        /// <param name="valueItem">值</param>
        /// <returns></returns>
        public KeyWordUpdateSetItem Set(UpdateSetParamItemList lstSetItem)
        {
            KeyWordUpdateSetItem item = new KeyWordUpdateSetItem(lstSetItem, this);
            return item;
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new SQLCommon.QueryConditions.UpdateCondition(info.DBInfo);
                //info.ParamList = new SQLCommon.DbCommon.ParamList();
            }
            if (info.ParamList == null)
            {
                info.ParamList = new SQLCommon.DbCommon.ParamList();
            }
            info.Condition.Tables.Append(table.DisplayValue(info));
            //return "update " + table.DisplayValue(info);
        }
    }
}
