using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 别名
    /// </summary>
    public class BQLAliasHandle : BQLTableHandle
    {
        private BQLQuery _query;
        private string _aliasName;
        private BQLTableHandle _table;

        /// <summary>
        /// 获取别名
        /// </summary>
        internal string GetAliasName() 
        {
           
                return _aliasName;
            
        }
        /// <summary>
        /// 主键
        /// </summary>
        public virtual string GetPrimaryParam()
        {
            return _table.GetPrimaryParam();
        }
        public BQLAliasHandle(BQLQuery query, string aliasName)
        {
            this._query = query;
            this._aliasName = aliasName;
        }

        public BQLAliasHandle(BQLTableHandle table, string aliasName)
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
            
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            if (info.Condition.PrimaryKey.Length <= 0 && !CommonMethods.IsNull(_table))
            {
                if (!string.IsNullOrEmpty(_aliasName))
                {
                    info.Condition.PrimaryKey.Append(info.DBInfo.CurrentDbAdapter.FormatTableName(_aliasName) + ".");
                }
                else
                {
                    info.Condition.PrimaryKey.Append(_table.DisplayValue(info) + ".");
                }
                info.Condition.PrimaryKey.Append(info.DBInfo.CurrentDbAdapter.FormatParam(_table.GetPrimaryParam()));
            }

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
