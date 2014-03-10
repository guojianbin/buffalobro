using System;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;
using System.Data.Common;
using System.Collections.Generic;

///通用SQL Server访问类v1.2

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// 提交方式
    /// </summary>
    public enum CommitState 
    {
        /// <summary>
        /// 自动提交(关闭连接)
        /// </summary>
        AutoCommit,
        /// <summary>
        /// 手动提交(关闭连接)
        /// </summary>
        UserCommit
    }

	/// <summary>
	/// 数据库访问类
	/// </summary>
	public class DataBaseOperate : IDisposable
	{
		
		
		// 数据库连接标志
		private bool _isConnected;
		
		
		//数据库连接对象
        private DbConnection _conn;

        /// <summary>
        /// 数据库连接
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                return _conn;
            }
        }
		private IDbCommand _comm;
		private IDbTransaction _tran;
		private IDbDataAdapter _sda;

        private int _lastAffectedRows;


        private IDBAdapter _dbAdapter = null;
        
        DBInfo _db = null;

        /// <summary>
        /// 事务是否已经开启
        /// </summary>
        private bool IsTran 
        {
            get 
            {
                return _tran != null;
            }
        }

        //是否自动关闭连接
        private CommitState _commitState = CommitState.AutoCommit;

        string _databaseName;

        /// <summary>
        /// 获取数据库名字
        /// </summary>
        /// <returns></returns>
        public string DataBaseName
        {
            get
            {
                if (_databaseName == null)
                {
                    try
                    {
                        //若连接数据库失败抛出错误
                        if (!ConnectDataBase())
                        {
                            throw (new ApplicationException("没有建立数据库连接。"));
                        }
                        _databaseName = _conn.Database;
                    }
                    finally
                    {

                        AutoClose();

                    }
                }
                return _databaseName;
            }
        }

        /// <summary>
        /// 是否自动关闭连接
        /// </summary>
        public CommitState CommitState 
        {
            get 
            {
                return _commitState;
            }
            set 
            {
                _commitState = value;
            }
        }

        

        /// <summary>
        /// 关联数据库信息
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _db;
            }
        }
        /// <summary>
        /// 开启非事务的批量操作
        /// </summary>
        /// <returns></returns>
        public BatchAction StarBatchAction() 
        {
            BatchAction action = new BatchAction(this);
            return action;
        }

		/// <summary>
		/// 实例化数据库访问对象
		/// </summary>
		internal DataBaseOperate(DBInfo db,bool isAutoClose)
		{
			_isConnected = false;
            _db = db;
            _dbAdapter = db.CurrentDbAdapter;
            if (!isAutoClose) 
            {
                _commitState = CommitState.UserCommit;
            }
		}


        internal DataBaseOperate(DBInfo db)
            :this(db,true)
        { 
        }

        /// <summary>
        /// 返回此 System.Data.Common.DbConnection 的数据源的架构信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema() 
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                dt = _conn.GetSchema();
            }
            finally 
            {
                AutoClose();
            }
            return dt;
        }
        /// <summary>
        /// 返回此 System.Data.Common.DbConnection 的数据源的架构信息
        /// </summary>
        /// <param name="collectionName">指定要返回的架构的名称</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                dt = _conn.GetSchema(collectionName);
            }
            finally
            {
                AutoClose();
            }
            return dt;
        }
        /// <summary>
        /// 返回此 System.Data.Common.DbConnection 的数据源的架构信息
        /// </summary>
        /// <param name="collectionName">指定要返回的架构的名称</param>
        /// <param name="restrictionValues">为请求的架构指定一组限制值</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("没有建立数据库连接。"));
                }
                dt = _conn.GetSchema(collectionName, restrictionValues);
            }
            finally
            {
                AutoClose();
            }
            return dt;
        }

		/// <summary>
		/// 连接数据库，并打开数据库连接
		/// </summary>
		/// <returns>成功返回true</returns>
		public bool ConnectDataBase()
		{
			if (!_isConnected)
			{
				try
				{
					if (_conn == null || _conn.State==ConnectionState.Closed)
					{
                        _conn = DBConn.GetConnection(_db);
						
					}
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }

                    if (_comm == null)
                    {
                        _comm = _dbAdapter.GetCommand();//**
                    }
                    
					_isConnected = true;
					_comm.Connection = _conn;
				}
				catch(Exception e)
				{
					throw e;
				}
			}
			return true;
		}

		
		#region IDisposable 成员
		/// <summary>
		/// 释放占用资源
		/// </summary>
		public void Dispose()
		{
            CloseDataBase();
			GC.SuppressFinalize(this); 
		}

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public bool CloseDataBase() 
        {
            if (_isConnected)
            {
                if (_conn != null && _conn.State != ConnectionState.Closed)
                {
                    try
                    {

                        
                           _db.OutMessage(MessageType.OtherOper,"Closed DataBase",null,"");
                        

                        
                        _conn.Close();
                        
                        if (_comm != null) 
                        {
                            _comm.Dispose();
                        }
                        _dbAdapter.OnConnectionClosed(_conn, _db);
                        _comm = null;
                        _conn = null;
                        _tran = null;
                        
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    _isConnected = false;
                }
            }
            return true;
        }

        

        /// <summary>
        /// 按照标识位自动关闭连接
        /// </summary>
        /// <returns></returns>
        internal bool AutoClose() 
        {
            if ((_commitState == CommitState.AutoCommit) && !IsTran) 
            {
                
                CloseDataBase();
                return true;
            }
            return false;
        }
        
		#endregion
		#region 查询返回所有数据
		/// <summary>
		/// 运行查询的方法,返回一个DataSet
		/// </summary>
		/// <param name="sql">要查询的SQL语句</param>
		/// <param name="sTableName">查询出来的表名</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <returns>返回结果集</returns>
        public DataSet QueryDataSet(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cacheTables)
		{

            return QueryDataSet(sql, paramList, CommandType.Text,cacheTables);
		}
		


		/// <summary>
		/// 运行查询的方法,返回一个DataSet
		/// </summary>
		/// <param name="sql">要查询的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <param name="queryCommandType">SQL语句类型</param>
		/// <returns>返回结果集</returns>
		public DataSet QueryDataSet(
			string	sql,
			ParamList paramList,
			CommandType queryCommandType,
            Dictionary<string, bool> cacheTables
			)
		{
            DataSet dataSet = null;
            if (cacheTables != null && cacheTables.Count > 0)
            {
                dataSet = _db.QueryCache.GetDataSet(cacheTables, sql, paramList);
                if (dataSet != null)
                {
                    return dataSet;
                }
            }
			//若连接数据库失败抛出错误
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接。"));
			}
			
			dataSet = new DataSet();
			_comm.CommandType = queryCommandType;
			_comm.CommandText = sql;
            _sda = _dbAdapter.GetAdapter();
			_sda.SelectCommand = _comm;
            string paramInfo = null;
			if(paramList!=null)
			{
                paramInfo=paramList.Fill(_comm, _db);
			}

            try
            {


                _db.OutMessage(MessageType.Query, "DataSet", null, sql + "," + paramInfo);
                

                _sda.Fill(dataSet);
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    _db.QueryCache.SetDataSet(dataSet, cacheTables, sql, paramList);

                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw e;
            }
            finally 
            {
                AutoClose();
            }

            return dataSet;
		}
		#endregion



		/// <summary>
		/// 运行查询的方法，返回一个DataReader，适合小数据的读取
		/// </summary>
		/// <param name="sql">要查询的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <returns>返回DataReader</returns>
		public IDataReader Query(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Query(sql,paramList,CommandType.Text,cachetables);
		}

		/// <summary>
		/// 运行查询的方法，返回一个DataReader，适合小数据的读取
		/// </summary>
		/// <param name="sql">要查询的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <param name="queryCommandType">SQL语句类型</param>
		/// <returns>返回DataReader</returns>
        public IDataReader Query(
			string	sql,
			ParamList paramList,
			CommandType exeCommandType,
            Dictionary<string, bool> cacheTables
			)
		{
            IDataReader reader;

            if (cacheTables != null && cacheTables.Count>0)
            {
                reader = _db.QueryCache.GetReader(cacheTables, sql, paramList);
                if (reader != null)
                {
                    return reader;
                }
            }

			//若连接数据库失败抛出错误
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接。"));
			}
			
			
			_comm.CommandType = exeCommandType;
			_comm.CommandText = sql;
            _comm.Parameters.Clear();
            string paramInfo = null;
            if (paramList != null)
			{
                paramInfo=paramList.Fill(_comm, _db);
			}
			

            
            try
            {

                if ((_commitState == CommitState.AutoCommit) && !IsTran)
                {


                    _db.OutMessage(MessageType.Query, "AutoCloseReader", null, sql + "," + paramInfo);
                    

                    reader = _comm.ExecuteReader(CommandBehavior.CloseConnection);
                    
                    _isConnected = false;
                    
                }
                else 
                {


                    _db.OutMessage(MessageType.Query, "Reader", null, sql + "," + paramInfo);
                    

                    reader = _comm.ExecuteReader();
                }
                if (paramList != null)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                //读入缓存
                if (cacheTables != null && cacheTables.Count > 0)
                {
                    IDataReader nreader=_db.QueryCache.SetReader(reader, cacheTables, sql, paramList);
                    if (nreader != null)
                    {
                        reader.Close();
                        reader = nreader;
                    }
                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw e;
            }
            

			return reader;
		}


        /// <summary>
        /// 最后影响行数
        /// </summary>
        public int LastAffectedRows
        {
            get { return _lastAffectedRows; }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        internal bool RoolBack() 
        {
            try
            {
                if (IsTran)
                {


                    _db.OutMessage(MessageType.OtherOper, "RollbackTransation", null, "");
                       
                    

                    _tran.Rollback();
                    _tran = null;
                    return true;
                }
            }
            finally
            {
                AutoClose();
            }

            return false;

        }

		/// <summary>
		/// 执行修改数据库操作，修改、删除等无返回值的操作
		/// </summary>
		/// <param name="sql">执行的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <returns>成功执行返回True</returns>
		public int Execute(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Execute(sql,paramList,CommandType.Text,cachetables);
		}

		/// <summary>
		/// 执行修改数据库操作，修改、删除等无返回值的操作
		/// </summary>
		/// <param name="sql">执行的SQL语句</param>
		/// <param name="paramList">SqlParameter的列表</param>
		/// <param name="queryCommandType">SQL语句类型</param>
		/// <returns>成功执行返回True</returns>
		public int Execute(
			string	sql,
			ParamList paramList,
			CommandType exeCommandType,
            Dictionary<string, bool> cacheTables)
		{
			
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接")); 
			}
			
			_comm.CommandType = exeCommandType;
			_comm.CommandText = sql;
            int ret = -1;
            _comm.Parameters.Clear();

            string paramInfo=null;
			if(paramList!=null)
			{
				paramInfo=paramList.Fill(_comm,_db);
			}

            try
            {


                _db.OutMessage(MessageType.Execute, "NonQuery", null, sql + "," + paramInfo);
                

                ret = _comm.ExecuteNonQuery();
                _lastAffectedRows = ret;
                if (paramList != null && _comm.CommandType == CommandType.StoredProcedure)
                {
                    paramList.ReturnParameterValue(_comm, _db);
                }

                if (cacheTables != null && cacheTables.Count > 0)
                {
                    _db.QueryCache.ClearTableCache(cacheTables);
                }
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw e;
            }
            finally 
            {
                AutoClose();
            }

            return ret;			
		}

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public DBTransation StartTransation(IsolationLevel isolationLevel)
        {
            bool runnow = StartTran(isolationLevel);
            if (runnow)
            {
                return new DBTransation(this);
            }
            return new DBTransation(null);
        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public DBTransation StartTransation()
        {
            return StartTransation(IsolationLevel.ReadCommitted);
        }

		/// <summary>
		/// 开始事务处理功能，之后执行的全部数据库操作语句需要调用提交函数（_commit）生效
		/// </summary>
        internal bool StartTran(IsolationLevel isolationLevel)
		{
			//若连接数据库失败抛出错误
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接。"));
			}


            if (!IsTran)
            {


                _db.OutMessage(MessageType.OtherOper, "BeginTransation", null, "Level=" + isolationLevel.ToString());
                

                _tran = _conn.BeginTransaction(isolationLevel);
                _comm.Transaction = _tran;
                
                return true;
            }
            return false;

		}

		/// <summary>
        /// 当前待处理事务提交，失败全部回滚
		/// </summary>
		/// <returns></returns>
		internal bool Commit()
		{
			//如果没有开启事务处理功能，不做任何操作，直接返回成功
            if (!IsTran)
			{
				return false;
			}

            try
            {


                _db.OutMessage(MessageType.OtherOper, "BeginTransation", null, "");
                

                _tran.Commit();
                _tran = null;
                
            }
            catch (Exception e)
            {
                //RoolBack();
                throw e;
            }
            finally 
            {
                AutoClose();
            }
			return true;
		}
		
        //~DataBaseOperate()
        //{
        //    CloseDataBase(); 
        //}

	}
}
