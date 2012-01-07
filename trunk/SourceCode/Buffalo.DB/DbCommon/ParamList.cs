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
	/// SqlParameter的列表
	/// </summary>
	public class ParamList:List<DBParameter>
	{
		/// <summary>
        /// 创建一个新的IDataParameter并添加到列表
		/// </summary>
        /// <param name="paramName">IDataParameter要映射参数的名字</param>
		/// <param name="type">参数类型</param>
		/// <param name="paramValue">参数的值</param>
		public void AddNew(string paramName,DbType type,object paramValue)
		{
			AddNew(paramName,type,paramValue,ParameterDirection.Input);
        }
        /// <summary>
        /// 创建一个新的IDataParameter并添加到列表
		/// </summary>
        /// <param name="paramName">IDataParameter要映射参数的名字</param>
		/// <param name="type">参数类型</param>
		/// <param name="paramValue">参数的值（如果为输入的参数，此项可为null）</param>
		/// <param name="paramDir">参数的输入类型</param>
		public void AddNew(string paramName,DbType type,object paramValue,ParameterDirection paramDir)
		{
            DBParameter newParam = new DBParameter(paramName, type, paramValue, paramDir);
            this.Add (newParam);
		}
		
		/// <summary>
		/// 把列表里边的SqlParameter加到SqlCommand里边
		/// </summary>
		/// <param name="comm">要加进去的SqlCommand</param>
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
        /// 返回变量的值
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
		/// 获取第几个SqlParameter
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
