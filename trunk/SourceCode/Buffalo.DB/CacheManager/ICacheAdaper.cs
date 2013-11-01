using System;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
namespace Buffalo.DB.CacheManager
{
    public interface ICacheAdaper
    {
        System.Data.DataSet GetData(IDictionary<string,bool> tableNames,string sql);
        void RemoveBySQL(IDictionary<string, bool> tableNames,string sql);
        void RemoveByTableName(string tableName);
        bool SetData(IDictionary<string, bool> tableNames, string sql, System.Data.DataSet ds);
        DBInfo Info{get;}
    }
}
