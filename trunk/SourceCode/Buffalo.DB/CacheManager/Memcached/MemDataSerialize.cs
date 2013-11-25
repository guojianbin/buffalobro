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
        
        #region д��
        /// <summary>
        /// ����д����ֽ�����
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
                    bw.Write(ds.Tables.Count);//д��������
                    foreach (DataTable dt in ds.Tables) 
                    {
                        WriteDataTable(dt, bw);
                    }
                }
                ret = ms.ToArray();
            }
            
            return ret;
        }

        /// <summary>
        /// д�����ݱ���Ϣ
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bw"></param>
        private static void WriteDataTable(DataTable dt, BinaryWriter bw) 
        {

            MemTypeManager.WriteString(bw, dt.TableName);
            
            //д������
            bw.Write(dt.Columns.Count);
            List<MemTypeItem> lstItem = new List<MemTypeItem>(dt.Columns.Count);//�е���Ϣ
            MemTypeItem item = null;
            //д������Ϣ
            foreach (DataColumn col in dt.Columns) 
            {
                MemTypeManager.WriteString(bw,col.DataType.FullName);//����

                //������ID
                item = MemTypeManager.GetTypeInfo(col.DataType);
                if (item != null) 
                {
                    bw.Write(item.TypeID);
                    lstItem.Add(item);
                }
            }

            //����
            bw.Write(dt.Rows.Count);

            //д������
            dt.Reset();
            foreach(DataRow row in dt.Rows)
            {
                for (int i = 0; i < lstItem.Count; i++) 
                {

                    if (row.IsNull(i)) 
                    {
                        lstItem[i].WriterHandle(bw, null);
                        continue;
                    }
                    object value = row[i];
                    lstItem[i].WriterHandle(bw, value);
                }
            }
        }
        #endregion

         /// <summary>
        /// �����ݴ����м��س���
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
                    ReadDataTable(br);
                }
            }
        }

        private static DataTable ReadDataTable(BinaryReader br) 
        {
            DataTable dt = new DataTable();
            
        }

        /// <summary>
        /// �ж�����ͷ�Ƿ��Ӧ
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
/*
 *�ļ��ṹ:
 * MDATA+���ݱ�����(int)+���ݱ�����(DataTable)
 *  DataTable�ṹ��
 *      ����+����Ϣ(����+�����ͱ�ʶ)
 *      ����+������
 *      �����ݣ�
 *          ��ͨ���ݣ��Ƿ��+����
 *          �������ݣ��Ƿ��+����+����
 */