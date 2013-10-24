using System;
namespace Buffalo.DB.CacheManager
{
    public interface ICacheAdaper
    {
        System.Data.DataSet GetData(string sql);
        void RemoveBySQL(string sql);
        void RemoveByTableName(string tableName);
        bool SetData(System.Collections.Generic.ICollection<string> tableNames, string sql, System.Data.DataSet ds);
    }
}
