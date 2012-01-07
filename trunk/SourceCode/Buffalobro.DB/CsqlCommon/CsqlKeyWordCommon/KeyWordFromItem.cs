using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordFromItem : CsqlQuery
    {
        protected CsqlTableHandle[] _tables;
        
        /// <summary>
        /// From�ؼ�����
        /// </summary>
        /// <param name="tables">����</param>
        /// <param name="previous">��һ���ؼ���</param>
        public KeyWordFromItem(CsqlTableHandle[] tables, CsqlQuery previous)
            : base(previous) 
        {
            this._tables = tables;
        }

        ///// <summary>
        ///// Ҫ��ѯ���ֶ�
        ///// </summary>
        //internal CsqlTableHandle[] Tables 
        //{
        //    get 
        //    {
        //        return tables;
        //    }
        //}

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem LeftJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "left", this);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem RightJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "right", this);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem InnerJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "inner", this);
            return item;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem CrossJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "cross", this);
            return item;
        }
        /// <summary>
        /// ȫ����
        /// </summary>
        /// <param name="jionTable">��</param>
        /// <param name="on">����</param>
        /// <returns></returns>
        public KeyWordJoinItem FullJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "full", this);
            return item;
        }
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
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
            return item;
        }

        ///// <summary>
        ///// ��ѯ��Χ
        ///// </summary>
        ///// <param name="star">��ʼ����(��0��ʼ)</param>
        ///// <param name="totleRecord">��ʾ����</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totleRecord) 
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totleRecord, this);
        //    return item;
        //}

        /// <summary>
        /// ���ر�ı�����Ϣ
        /// </summary>
        /// <param name="info"></param>
        internal override void LoadInfo(KeyWordInfomation info) 
        {
            foreach (CsqlTableHandle tab in _tables) 
            {
                tab.FillInfo(info);
            }
        }

        /// <summary>
        /// from�ؼ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder ret = new StringBuilder();


            if (info.AliasManager == null)
            {
                for (int i = 0; i < _tables.Length; i++)
                {
                    CsqlTableHandle table = _tables[i];

                    ret.Append(table.DisplayValue(info));
                    if (i < _tables.Length - 1)
                    {
                        ret.Append(",");
                    }
                }
                info.Condition.Tables.Append(ret.ToString());
            }
            else 
            {
                for (int i = 0; i < _tables.Length; i++)
                {
                    CsqlTableHandle table = _tables[i];

                    CsqlAliasHandle ahandle= info.AliasManager.GetPrimaryAliasHandle(table);
                    if (!Commons.CommonMethods.IsNull(ahandle)) 
                    {
                        _tables[i] = ahandle;
                    }
                }


                KeyWordFromItem from=info.AliasManager.ToInnerTable(this,info);
                TableAliasNameManager manager = info.AliasManager;
                info.AliasManager = null;
                Stack<KeyWordFromItem> stkFrom = new Stack<KeyWordFromItem>();
                while (from != null) 
                {
                    stkFrom.Push(from);
                    from = from.Previous as KeyWordFromItem;
                }
                while (stkFrom.Count > 0)
                {
                    from = stkFrom.Pop();
                    from.Tran(info);
                }
                info.AliasManager = manager;
            }
            
            
        }



        
    }
}
