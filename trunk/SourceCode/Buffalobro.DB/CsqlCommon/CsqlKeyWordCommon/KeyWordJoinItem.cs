using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using System.Threading;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordJoinItem : KeyWordFromItem
    {
        //private CsqlTableHandle joinTable;
        private CsqlCondition _condition;
        private string _keyWord;
        /// <summary>
        /// LeftJoin�ؼ�����
        /// </summary>
        /// <param name="prmsHandle">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordJoinItem(CsqlTableHandle joinTable, CsqlCondition condition, string keyWord, CsqlQuery previous)
            : base(new CsqlTableHandle[]{joinTable}, previous) 
        {
            //this.joinTable = joinTable;
            this._condition = condition;
            this._keyWord = keyWord;
        }
        
        /// <summary>
        /// ���ر�ı�����Ϣ
        /// </summary>
        /// <param name="info"></param>
        //internal void LoadTableInfo(KeyWordInfomation info)
        //{
        //    joinTable.FillInfo(info);
        //}

        internal override void Tran(KeyWordInfomation info)
        {
            
            string ret = " "+_keyWord + " join " + _tables[0].DisplayValue(info) + " on " + _condition.DisplayValue(info);
            info.Condition.Tables.Append(ret);
        }
    }
}
