using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// 缓存数据序列化
    /// </summary>
    public class MemDataSerialize
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 数据头表示
        /// </summary>
        private static readonly byte[] HeadData = {77,68,65,84,65 };//MDATA
        /// <summary>
        /// 把字符串转成字节数组
        /// </summary>
        /// <param name="data">字符串</param>
        /// <returns></returns>
        private static byte[] GetStringData(string data) 
        {
            return DefaultEncoding.GetBytes(data);
        }

        /// <summary>
        /// MemDataSet数据写入成字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] MemDataSetToBytes(MemDataSet ds)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream(5000))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(HeadData);
                    bw.Write(ds.Tables.Count);//写入表的数量
                    foreach (MemDataTable dt in ds.Tables) 
                    {

                    }
                }
            }
        }
        /// <summary>
        /// 写入数据表信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bw"></param>
        private static void WriteDataTable(MemDataTable dt, BinaryWriter bw) 
        {
            bw.Write(GetStringData(dt.TableName));
            
            //写入列数
            bw.Write(dt.Columns.Count);
            //写入列信息
            foreach (DataColumn col in dt.Columns) 
            {
                //bw.Write(col.DataType.FullName
            }
        }


    }
}
