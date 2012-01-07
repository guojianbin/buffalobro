using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.DbCommon;
using Buffalo.Kernel;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// Select�ؼ�����
    /// </summary>
    public class KeyWordSelectItem:CsqlQuery
    {
        private CsqlParamHandle[] parameters;

        /// <summary>
        /// Select�ؼ�����
        /// </summary>
        /// <param name="prmsHandle">�ֶμ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordSelectItem(CsqlParamHandle[] parameters,CsqlQuery previous)
            : base(previous) 
        {
            this.parameters = parameters;
        }

        ///// <summary>
        ///// Ҫ��ѯ���ֶ�
        ///// </summary>
        //internal CsqlParamHandle[] Parameters 
        //{
        //    get 
        //    {
        //        return parameters;
        //    }
        //}
        /// <summary>
        /// From��Щ��
        /// </summary>
        /// <param name="tables">��</param>
        /// <returns></returns>
        public KeyWordFromItem From(params CsqlTableHandle[] tables)
        {
            KeyWordFromItem fromItem = new KeyWordFromItem(tables,this);
            return fromItem;
        }

        /// <summary>
        /// ���ر��β�ѯҪ���ص��ֶ���Ϣ
        /// </summary>
        /// <param name="info"></param>
        internal override void LoadInfo(KeyWordInfomation info) 
        {
            foreach (CsqlParamHandle prm in parameters) 
            {
                prm.FillInfo(info);
            }
        }

        /// <summary>
        /// select�ؼ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new SQLCommon.QueryConditions.SelectCondition(info.DBInfo);
                
            }
            if (info.ParamList == null)
            {
                
                    info.ParamList = new ParamList();
                
            }
            
            StringBuilder ret = new StringBuilder();

            IEnumerable<CsqlParamHandle> coll = parameters;

            if (info.AliasManager != null&& !info.Infos.IsPutPropertyName) 
            {
                coll = info.AliasManager.GetPrimaryAliasParamHandle(parameters);
            }
            foreach (CsqlParamHandle prm in coll)
            {
                //CsqlParamHandle prm = parameters[i];
                
                    ret.Append(prm.DisplayValue(info));
                

                ret.Append(",");

            }
            if (ret.Length > 0) 
            {
                ret.Remove(ret.Length - 1, 1);
            }
            info.Condition.SqlParams.Append(ret);
            //return "select " + ret;
        }
    }
}
