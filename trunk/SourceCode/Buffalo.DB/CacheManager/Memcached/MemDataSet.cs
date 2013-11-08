using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// 缓存数据集信息
    /// </summary>
    public class MemDataSet
    {
        private List<MemDataTable> _tables=new List<MemDataTable>();
        /// <summary>
        /// 数据表
        /// </summary>
        public List<MemDataTable> Tables
        {
            get { return _tables; }
        }

        /// <summary>
        /// 把数据库Reader的内容保存到MemDataSet
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static MemDataSet ReadToMemDataSet(DbDataReader reader) 
        {
            
            MemDataSet ds = new MemDataSet();
            int index = 1;
            string tableName = "表" + index;
            do
            {
                MemDataTable dt = ReadTable(tableName, reader);
                index++;
                ds._tables.Add(dt);
            }
            while(reader.NextResult());
            return ds;
        }



        /// <summary>
        /// 读取表
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="reader">读取器</param>
        /// <returns></returns>
        private static MemDataTable ReadTable(string name,DbDataReader reader) 
        {
            MemDataTable dt = new MemDataTable();

            dt.TableName = name;
            int colcount = reader.FieldCount;

            //添加列
            List<DataColumn> lstCol = dt.Columns;
            for (int i = 0; i < colcount; i++)
            {
                DataColumn col = new DataColumn(reader.GetName(i), reader.GetFieldType(i));

                lstCol.Add(col);
            }

            while (reader.Read())
            {
                MemDataItem item = new MemDataItem(lstCol.Count);
                for (int i = 0; i < colcount; i++)
                {
                    item[i] = reader[i];
                }
                dt.AddItem(item);
            }
            return dt;
        }
    }
}
