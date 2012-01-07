using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class CsqlOtherTableHandle:CsqlTableHandle
    {
        string tableName;
        public CsqlOtherTableHandle(string tableName)
        {
            this.tableName = tableName;
            //this.valueType = CsqlValueType.Table;
        }
        ///// <summary>
        ///// 对应的实体类型
        ///// </summary>
        //internal Type __EntityType__
        //{
        //    get
        //    {
        //        return entityInfo.EntityType;
        //    }
        //}

        ///// <summary>
        ///// 对应的表名
        ///// </summary>
        //internal string __TableName__
        //{
        //    get
        //    {
        //        return entityInfo.TableName;
        //    }
        //}
        internal override void FillInfo(KeyWordInfomation info)
        {
            //if (_db != null && info.DBInfo == null) 
            //{
            //    info.DBInfo = _db;
            //}
        }
        /// <summary>
        /// 获取对应的实体属性
        /// </summary>
        /// <returns></returns>
        internal override List<ParamInfo> GetParamInfoHandle()
        {
            List<ParamInfo> lst = new List<ParamInfo>();
            return lst;
        }
       
        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            return idba.FormatTableName(tableName);
        }

        

        ///// <summary>
        ///// 给这个表定义一个别名
        ///// </summary>
        ///// <param name="asName">别名</param>
        ///// <returns></returns>
        //public CsqlAliasHandle AS(string asName)
        //{
        //    CsqlAliasHandle item = new CsqlAliasHandle(this, asName);
        //    return item;
        //}

        
    }
}
