using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// 别名
    /// </summary>
    public class CsqlAliasHandle : CsqlTableHandle
    {
        private CsqlQuery _query;
        private string _aliasName;
        private CsqlTableHandle _table;

        /// <summary>
        /// 获取别名
        /// </summary>
        internal string GetAliasName() 
        {
           
                return _aliasName;
            
        }

        public CsqlAliasHandle(CsqlQuery query, string aliasName)
        {
            this._query = query;
            this._aliasName = aliasName;
        }

        public CsqlAliasHandle(CsqlTableHandle table, string aliasName)
        {
            this._table = table;
            this._aliasName = aliasName;
        }

        internal override List<ParamInfo> GetParamInfoHandle()
        {
            if (!CommonMethods.IsNull(_table))
            {
                return _table.GetParamInfoHandle();
            }
            return new List<ParamInfo>();
        }
        internal override void FillInfo(KeyWordInfomation info)
        {
            //QueryParamCollection coll = new QueryParamCollection();
            //coll.TableName = aliasName;
            //if (!CommonMethods.IsNull(table,null)) 
            //{
            //    List<ParamInfo> lstInfo = table.GetParamInfoHandle();
            //    foreach (ParamInfo pinfo in lstInfo) 
            //    {
            //        coll[pinfo.PropertyName] = pinfo;
            //    }
                
            //}
            //else if (query != null) 
            //{
            //    foreach (ParamInfo pinfo in qinfo.QueryParams) 
            //    {
            //        coll[pinfo.PropertyName] = pinfo;
            //    }
            //}
            //if (info != null)
            //{
            //    info.Alias[aliasName] = coll;
            //}
            //if (info.DBInfo == null)
            //{
            //    if (!CommonMethods.IsNull(table, null))
            //    {
            //        table.FillInfo(info);
            //    }
            //    else if (query != null)
            //    {
            //        KeyWordInfomation qinfo = info.Clone() as KeyWordInfomation;
            //        qinfo.Condition = new SelectCondition(info.DBInfo);
            //        qinfo.ParamList = info.ParamList;
            //        KeyWordConver objCover = new KeyWordConver();
            //        objCover.CollectItem(query, qinfo);
            //        //if (info.Condition.PrimaryKey.Length <= 0 && qinfo.Condition.PrimaryKey.Length > 0)
            //        //{
            //        //    info.Condition.PrimaryKey.Append(qinfo.Condition.PrimaryKey.ToString());
            //        //}
            //        if (qinfo.DBInfo != null) 
            //        {
            //            info.DBInfo = qinfo.DBInfo;
            //        }
            //    }
            //}
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            string result = null;
            if (!CommonMethods.IsNull(_table))
            {
                result = _table.DisplayValue(info);
            }
            else if (_query != null)
            {
                KeyWordInfomation qinfo = info.Clone() as KeyWordInfomation;
                qinfo.Condition = new SelectCondition(info.DBInfo);
                qinfo.ParamList = info.ParamList;
                KeyWordConver objCover = new KeyWordConver();
                string sql = objCover.ToConver(_query, qinfo).GetSql();
                //if (info.Condition.PrimaryKey.Length <= 0 && qinfo.Condition.PrimaryKey.Length > 0)
                //{
                //    info.Condition.PrimaryKey.Append(qinfo.Condition.PrimaryKey.ToString());
                //}
                
                 result = "(" + sql + ")";
                
            }
            if (!string.IsNullOrEmpty(_aliasName)) 
            {
                result += " " + info.DBInfo.CurrentDbAdapter.FormatTableName(_aliasName);
            }
            return result;
        }
    }
}
