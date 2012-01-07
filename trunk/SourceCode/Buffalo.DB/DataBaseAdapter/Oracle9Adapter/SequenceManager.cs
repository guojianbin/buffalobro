using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// �������й���
    /// </summary>
    public class SequenceManager
    {
        //private static Dictionary<string, bool> dicSequence = new Dictionary<string, bool>();//�����Ѿ���ʼ���ļ���

        /// <summary>
        /// ��ȡ�����Ե�����
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetSequenceName(string tableName,string paramName) 
        {
            StringBuilder sbSeqName = new StringBuilder(20);
            sbSeqName.Append("seq_");
            sbSeqName.Append(tableName);
            sbSeqName.Append("_");
            sbSeqName.Append(paramName);
            sbSeqName.Replace(" ", "");

            return sbSeqName.ToString() ;
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="seqName"></param>
        public static void InitSequence(string seqName, DataBaseOperate oper) 
        {
            //DataBaseOperate oper = new DataBaseOperate(db);
            //try 
            //{
                if (!IsSequenceExists(seqName, oper)) //�ж��Ƿ��Ѿ���������
                {
                    CreateSequence(seqName, oper);//��������
                }
            //}
            //finally 
            //{
            //    oper.CloseDataBase();
            //}

        }

        /// <summary>
        /// ��ȡ�����Ƿ����
        /// </summary>
        /// <param name="seqName">������</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        private static bool IsSequenceExists(string seqName, DataBaseOperate oper)
        {
            string sql = "select count(*) from user_sequences where SEQUENCE_NAME='" + seqName+ "'";
            
            IDataReader reader = null;
            int count = 0;
            try
            {
                reader = oper.Query(sql, null);
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("��ѯ����ʱ����ִ���:" + ex.Message);
            }
            finally 
            {
                reader.Close();
            }
            return count > 0;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="seqName">������</param>
        /// <param name="oper">���ݿ�����</param>
        private static void CreateSequence(string seqName, DataBaseOperate oper) 
        {
            string sql = "CREATE SEQUENCE \""+seqName+"\" INCREMENT BY 1 START WITH 1  NOMAXVALUE  NOCYCLE  NoCACHE";
            
            oper.Execute(sql, null);
        }
    }
}
