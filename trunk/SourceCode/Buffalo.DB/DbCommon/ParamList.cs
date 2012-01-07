using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Text;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.MessageOutPuters;
//ParamList v1.1
namespace Buffalo.DB.DbCommon
{
	
	/// <summary>
	/// SqlParameter���б�
	/// </summary>
	public class ParamList:List<DBParameter>
	{
		/// <summary>
        /// ����һ���µ�IDataParameter����ӵ��б�
		/// </summary>
        /// <param name="paramName">IDataParameterҪӳ�����������</param>
		/// <param name="type">��������</param>
		/// <param name="paramValue">������ֵ</param>
		public void AddNew(string paramName,DbType type,object paramValue)
		{
			AddNew(paramName,type,paramValue,ParameterDirection.Input);
        }
        /// <summary>
        /// ����һ���µ�IDataParameter����ӵ��б�
		/// </summary>
        /// <param name="paramName">IDataParameterҪӳ�����������</param>
		/// <param name="type">��������</param>
		/// <param name="paramValue">������ֵ�����Ϊ����Ĳ����������Ϊnull��</param>
		/// <param name="paramDir">��������������</param>
		public void AddNew(string paramName,DbType type,object paramValue,ParameterDirection paramDir)
		{
            DBParameter newParam = new DBParameter(paramName, type, paramValue, paramDir);
            this.Add (newParam);
		}
		
		/// <summary>
		/// ���б���ߵ�SqlParameter�ӵ�SqlCommand���
		/// </summary>
		/// <param name="comm">Ҫ�ӽ�ȥ��SqlCommand</param>
		public string Fill(IDbCommand comm,DBInfo db)
		{
            StringBuilder ret = new StringBuilder(500);
            comm.Parameters.Clear();
#if DEBUG
            bool isOutput=db.SqlOutputer.HasOutput;
#endif
            for (int i = 0; i < this.Count; i++)
            {
                DBParameter prm=this[i];
                IDataParameter dPrm=db.CurrentDbAdapter.GetDataParameter(prm.ParameterName, prm.DbType, prm.Value, prm.Direction);
                comm.Parameters.Add(dPrm);
#if DEBUG
                if (isOutput)
                {
                    ret.Append(dPrm.ParameterName + "=" + DataAccessCommon.FormatValue(dPrm.Value, dPrm.DbType, db));
                    if (i < this.Count - 1)
                    {
                        ret.Append(" , ");
                    }
                }
#endif
            }
            return ret.ToString();
		}

        /// <summary>
        /// ���ر�����ֵ
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="db"></param>
        public void ReturnParameterValue(IDbCommand comm, DBInfo db) 
        {
            for (int i = 0; i < this.Count; i++)
            {
                DBParameter prm = this[i];
                if (prm.Direction != ParameterDirection.Input) 
                {
                    IDataParameter dPrm = comm.Parameters[i] as IDataParameter;
                    if (dPrm != null) 
                    {
                        prm.Value = dPrm.Value;
                    }
                }
            }
        }

		/// <summary>
		/// ��ȡ�ڼ���SqlParameter
		/// </summary>
        public DBParameter this[string paramName]
		{
			get
			{
                for (int i = 0; i < this.Count; i++) 
                {
                    DBParameter tmpParam = this[i];
                    if(tmpParam.ParameterName==paramName)
                    {
                        return tmpParam;
                    }
                }
                return null;
			}
			
		}

        


	}
}
