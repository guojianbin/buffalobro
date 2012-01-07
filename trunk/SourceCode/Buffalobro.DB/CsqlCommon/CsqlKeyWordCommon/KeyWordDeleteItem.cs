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
        /// Delete�ؼ�����
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="previous">��һ���ؼ���</param>
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
        ///// Ҫ��ѯ���ֶ�
        ///// </summary>
        //internal CsqlTableHandle Table
        //{
        //    get 
        //    {
        //        return table;
        //    }
        //}
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="condition">����</param>
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
