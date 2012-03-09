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
        /// 已经存在的数据库值
        /// </summary>
        private Dictionary<string, DBParameter> _dicExistsValue = new Dictionary<string, DBParameter>();

        /// <summary>
        /// 新的键名
        /// </summary>
        /// <returns></returns>
        private string NewKeyName(DBInfo db) 
        {
            string pName = "P" + this.Count;
            return db.CurrentDbAdapter.FormatParamKeyName(pName);
        }

        /// <summary>
        /// 新的值名
        /// </summary>
        /// <returns></returns>
        private string NewValueKeyName(DBInfo db)
        {
            string pName = "P" + this.Count;
            return db.CurrentDbAdapter.FormatValueName(pName);
        }

        /// <summary>
        /// 新的数据库值
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <param name="paramValue">值类型</param>
        /// <returns></returns>
        public DBParameter NewParameter(DbType type, object paramValue,DBInfo db) 
        {
            
            if (paramValue is byte[])
            {
                string pKeyName = NewKeyName(db);
                return AddNew(pKeyName, type, paramValue);
            }
            else
            {
                string strValue = paramValue as string;
                if (strValue != null && strValue.Length > 5000)
                {
                    string pKeyName = NewKeyName(db);
                    return AddNew(pKeyName, type, paramValue);
                }
            }

            DBParameter prm=null;
            string key =((int)type).ToString()+":"+DataAccessCommon.FormatValue(paramValue, type, db);
            if (!_dicExistsValue.TryGetValue(key, out prm)) 
            {
                string pKeyName = NewKeyName(db);
                string valueName = NewValueKeyName(db);
                prm = AddNew(pKeyName, type, paramValue);
                prm.ValueName = valueName;
                _dicExistsValue[key] = prm;
            }
            return prm;
        }

		/// <summary>
        /// 创建一个新的IDataParameter并添加到列表
		/// </summary>
        /// <param name="paramName">IDataParameter要映射参数的名字</param>
		/// <param name="type">参数类型</param>
		/// <param name="paramValue">参数的值</param>
        public DBParameter AddNew(string paramName, DbType type, object paramValue)
		{
			return AddNew(paramName,type,paramValue,ParameterDirection.Input);
        }
        /// <summary>
        /// 创建一个新的IDataParameter并添加到列表
		/// </summary>
        /// <param name="paramName">IDataParameter要映射参数的名字</param>
		/// <param name="type">参数类型</param>
		/// <param name="paramValue">参数的值（如果为输入的参数，此项可为null）</param>
		/// <param name="paramDir">参数的输入类型</param>
        public DBParameter AddNew(string paramName, DbType type, object paramValue, ParameterDirection paramDir)
		{
            DBParameter newParam = new DBParameter(paramName, type, paramValue, paramDir);
            this.Add (newParam);
            return newParam;
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
                DBParameter prm = this[i];
                IDataParameter dPrm = GetParameter(prm,db);
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
        /// 获取实际数据库的字段变量
        /// </summary>
        /// <param name="prm"></param>
        /// <returns></returns>
        private IDataParameter GetParameter(DBParameter prm, DBInfo db)
        {
            object value = prm.Value;
            if (value is Guid)
            {
                value = Buffalo.Kernel.CommonMethods.GuidToString((Guid)value);
            }
            DbType dbType = prm.DbType;
            if (dbType == DbType.Guid) 
            {
                dbType = DbType.AnsiString;
            }
            return db.CurrentDbAdapter.GetDataParameter(prm.ParameterName, dbType, value, prm.Direction);
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
