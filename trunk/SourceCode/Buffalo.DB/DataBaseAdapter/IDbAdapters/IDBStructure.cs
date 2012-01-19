using System;
using System.Collections.Generic;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IDBStructure
    {
        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        List<string> GetAllTableName(DBInfo info);
        /// <summary>
        /// 添加字段的语句
        /// </summary>
        /// <returns></returns>
        string GetAddParamSQL();
    }
}
