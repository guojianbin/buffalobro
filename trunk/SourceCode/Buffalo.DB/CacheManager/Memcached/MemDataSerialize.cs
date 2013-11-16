using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// �����������л�
    /// </summary>
    public class MemDataSerialize
    {
        /// <summary>
        /// Ĭ�ϱ���
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// ����ͷ��ʾ
        /// </summary>
        private static readonly byte[] HeadData = {77,68,65,84,65 };//MDATA
        /// <summary>
        /// ���ַ���ת���ֽ�����
        /// </summary>
        /// <param name="data">�ַ���</param>
        /// <returns></returns>
        private static byte[] GetStringData(string data) 
        {
            return DefaultEncoding.GetBytes(data);
        }

        /// <summary>
        /// MemDataSet����д����ֽ�����
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
                    bw.Write(ds.Tables.Count);//д��������
                    foreach (MemDataTable dt in ds.Tables) 
                    {

                    }
                }
            }
        }
        /// <summary>
        /// д�����ݱ���Ϣ
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bw"></param>
        private static void WriteDataTable(MemDataTable dt, BinaryWriter bw) 
        {
            bw.Write(GetStringData(dt.TableName));
            
            //д������
            bw.Write(dt.Columns.Count);
            //д������Ϣ
            foreach (DataColumn col in dt.Columns) 
            {
                //bw.Write(col.DataType.FullName
            }
        }


    }
}
