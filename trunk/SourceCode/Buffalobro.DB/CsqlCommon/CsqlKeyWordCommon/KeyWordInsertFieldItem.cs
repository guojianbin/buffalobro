using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.CsqlCommon.IdentityInfos;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.QueryConditions;
using System.Data;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordInsertFieldItem : CsqlQuery
    {
        private CsqlParamHandle[] paramHandles;
        /// <summary>
        /// Insert���ֶιؼ�����
        /// </summary>
        /// <param name="paramHandles">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordInsertFieldItem(CsqlParamHandle[] paramHandles, CsqlQuery previous)
            : base(previous) 
        {
            this.paramHandles = paramHandles;
        }

        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        ///// <summary>
        ///// Ҫ������ֶ�
        ///// </summary>
        //internal CsqlParamHandle[] ParamHandles 
        //{
        //    get 
        //    {
        //        return paramHandles;
        //    }
        //}

        /// <summary>
        /// ֵ
        /// </summary>
        /// <param name="values">ֵ</param>
        /// <returns></returns>
        public KeyWordInsertValueItem Values(params object[] values)
        {
            CsqlValueItem[] vitems = new CsqlValueItem[values.Length];
            for (int i = 0; i < values.Length;i++ )
            {

                if (values[i] == null) 
                {
                    vitems[i] = new CsqlValueTypeItem(null);
                }
                else if (values[i] is CsqlValueItem) 
                {
                    vitems[i] = values[i] as CsqlValueItem;
                }
                else if (values[i].GetType().IsEnum)
                {
                    vitems[i] = new CsqlValueTypeItem((int)values[i]);
                }
                else
                {
                    vitems[i] = new CsqlValueTypeItem(values[i]);
                }
            }
            KeyWordInsertValueItem valuesItem = new KeyWordInsertValueItem(vitems, this);
            return valuesItem;
        }
        /// <summary>
        /// ����һ����ѯ����
        /// </summary>
        /// <param name="query">��ѯ</param>
        /// <returns></returns>
        public KeyWordInsertQueryItem ByQuery(CsqlQuery query)
        {
            return new KeyWordInsertQueryItem(query, this);
        }
        /// <summary>
        /// �Զ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal string IdentityFieldString(KeyWordInfomation info) 
        {
            StringBuilder ret=new StringBuilder(500);
            IDBAdapter idb = info.DBInfo.CurrentDbAdapter;
            string prmName=null;
            foreach(IdentityInfo identInfo in info.IdentityInfos)
            {
                //IdentityInfo identInfo = info.IdentityInfos[i];

                prmName=idb.GetIdentityParamName(identInfo.PropertyInfo);
                if(string.IsNullOrEmpty(prmName))
                {
                    return "";
                }
                ret.Append(idb.FormatTableName(identInfo.EntityInfo.TableName));
                ret.Append(".");
                ret.Append(prmName);
                ret.Append(',');
                
            }
            return ret.ToString();
        }

        internal override void Tran(KeyWordInfomation info)
        {
            
            StringBuilder condition = new StringBuilder();
            List<DbType> lstParamType=info.Condition.ParamTypes;
            foreach (CsqlParamHandle vItem in paramHandles)
            {
                //CsqlParamHandle vItem = paramHandles[i];
                condition.Append(vItem.DisplayValue(info));
                
                condition.Append(',');

                
                if (lstParamType != null) 
                {
                    lstParamType.Add(vItem.ValueDbType);
                }
            }
            
            condition.Append(IdentityFieldString(info));
            if (condition.Length > 0) 
            {
                condition.Remove(condition.Length - 1, 1);
            }
            info.Condition.SqlParams.Append( condition);
        
        }
    }
}
