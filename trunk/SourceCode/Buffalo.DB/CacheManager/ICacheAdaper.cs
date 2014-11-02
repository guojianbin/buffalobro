using System;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
namespace Buffalo.DB.CacheManager
{
    public interface ICacheAdaper
    {
        System.Data.DataSet GetData(IDictionary<string,bool> tableNames,string sql,DataBaseOperate oper);
        void RemoveBySQL(IDictionary<string, bool> tableNames, string sql, DataBaseOperate oper);
        void RemoveByTableName(string tableName, DataBaseOperate oper);
        bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds, DataBaseOperate oper);
        DBInfo Info{get;}
    }
}
