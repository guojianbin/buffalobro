using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Threading;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordJoinItem : KeyWordFromItem
    {
        //private BQLTableHandle joinTable;
        private BQLCondition _condition;
        private string _keyWord;
        /// <summary>
        /// LeftJoin�ؼ�����
        /// </summary>
        /// <param name="prmsHandle">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordJoinItem(BQLTableHandle joinTable, BQLCondition condition, string keyWord, BQLQuery previous)
            : base(new BQLTableHandle[]{joinTable}, previous) 
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
