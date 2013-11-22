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
        #region 写入
        /// <summary>
        /// 数据写入成字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] DataSetToBytes(DataSet ds)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream(5000))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(HeadData);
                    bw.Write(ds.Tables.Count);//写入表的数量
                    foreach (DataTable dt in ds.Tables) 
                    {
                        WriteDataTable(dt, bw);
                    }
                }
            }
        }

        /// <summary>
        /// 写入数据表信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bw"></param>
        private static void WriteDataTable(DataTable dt, BinaryWriter bw) 
        {
            bw.Write(GetStringData(dt.TableName));
            
            //写入列数
            bw.Write(dt.Columns.Count);
            List<MemTypeItem> lstItem = new List<MemTypeItem>(dt.Columns.Count);//列的信息
            MemTypeItem item = null;
            //写入列信息
            foreach (DataColumn col in dt.Columns) 
            {
                bw.Write(GetStringData(col.DataType.FullName));//列名

                //列类型ID
                item = MemTypeManager.GetTypeInfo(col.DataType);
                if (item != null) 
                {
                    bw.Write(item.TypeID);
                    lstItem.Add(item);
                }
            }

            //行数
            bw.Write(dt.Rows.Count);

            //写入数据
            dt.Reset();
            foreach(DataRow row in dt.Rows)
            {
                for (int i = 0; i < lstItem.Count; i++) 
                {

                    if (row.IsNull(i)) 
                    {
                        bw.Write(true);
                        continue;
                    }
                    object value = row[i];
                    lstItem[i].WriterHandle(bw, value);
                }
            }
        }
        #endregion

         /// <summary>
        /// 把数据从流中加载出来
        /// </summary>
        /// <returns></returns>
        public static DataSet LoadDataSet(Stream stm)
        {
            if (!IsHead(stm)) 
            {
                return null;
            }
            using (BinaryReader br = new BinaryReader(stm))
            {
                int tableCount = br.ReadInt32();
                for (int i = 0; i < tableCount; i++) 
                {

                }
            }
        }

        private static DataTable ReadDataTable(BinaryReader br) 
        {
            DataTable dt = new DataTable();

        }

        /// <summary>
        /// 判断数据头是否对应
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        private static bool IsHead(Stream stm) 
        {
            byte[] head = new byte[HeadData.Length];
            stm.Read(head, 0, head.Length);
            for (int i = 0; i < head.Length; i++) 
            {
                if (head[i] != HeadData[i]) 
                {
                    return false;
                }
            }
            return true;
        }
    }
}
