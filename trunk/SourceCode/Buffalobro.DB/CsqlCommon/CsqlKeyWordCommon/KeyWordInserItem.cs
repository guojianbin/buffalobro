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
        /// Insert�ؼ�����
        /// </summary>
        /// <param name="tableHandle">Ҫ����ı�</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordInserItem(CsqlTableHandle tableHandle, CsqlQuery previous)
            : base(previous) 
        {
            this.tableHandle = tableHandle;
        }

        ///// <summary>
        ///// Ҫ����ı�
        ///// </summary>
        //internal CsqlTableHandle TableHandle 
        //{
        //    get 
        //    {
        //        return tableHandle;
        //    }
        //}
        /// <summary>
        /// �ֶ�
        /// </summary>
        /// <param name="paramhandles">�ֶ�</param>
        /// <returns></returns>
        public KeyWordInsertFieldItem Fields(params CsqlParamHandle[] paramhandles)
        {
            KeyWordInsertFieldItem fItem = new KeyWordInsertFieldItem(paramhandles, this);
            return fItem;
        }

        /// <summary>
        /// ����һ����ѯ����
        /// </summary>
        /// <param name="query">��ѯ</param>
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
