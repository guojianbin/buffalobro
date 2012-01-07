using System;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;

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
		private IDbConnection _conn;
		private IDbCommand _comm;
		private IDbTransaction _tran;
		private IDbDataAdapter _sda;

        
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
                    
                    _conn.Open();

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
#if DEBUG
                        if (_db.SqlOutputer.HasOutput)
                        {
                            OutMessage("Closed DataBase");
                        }
#endif
                        _conn.Close();
                        _conn.Dispose();
                        //_comm = null;
                        //_conn = null;
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
#if DEBUG
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message"></param>
        private void OutMessage(params string[] messages) 
        {
            StringBuilder smsg = new StringBuilder(messages.Length * 20);
            foreach (string msg in messages) 
            {
                smsg.Append(msg);
                
                smsg.Append(";   ");
            }
            _db.SqlOutputer.OutPut("SQLCommon."+DBInfo.Name+":", smsg.ToString());
        }
#endif
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
			ParamList paramList)
		{

            return QueryDataSet(sql, paramList, CommandType.Text);
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
			CommandType queryCommandType
			)
		{
				
			//若连接数据库失败抛出错误
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接。"));
			}
			
			DataSet dataSet = new DataSet();
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
#if DEBUG
                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage("[DataSet]:" + sql, paramInfo);
                }
#endif
                _sda.Fill(dataSet);
                
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
            if (paramList != null)
            {
                paramList.ReturnParameterValue(_comm, _db);
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
			ParamList paramList)
		{
			return Query(sql,paramList,CommandType.Text);
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
			CommandType exeCommandType
			)
		{
				
			//若连接数据库失败抛出错误
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("没有建立数据库连接。"));
			}
			
			
			_comm.CommandType = exeCommandType;
			_comm.CommandText = sql;
            _comm.Parameters.Clear();
            string paramInfo = null;
			if(paramList!=null)
			{
                paramInfo=paramList.Fill(_comm, _db);
			}
			IDataReader reader;
            try
            {

                if ((_commitState == CommitState.AutoCommit) && !IsTran)
                {
#if DEBUG
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage("[AutoCloseReader]:" + sql, paramInfo);
                    }
#endif
                    reader = _comm.ExecuteReader(CommandBehavior.CloseConnection);
                    
                    _isConnected = false;
                    
                }
                else 
                {
#if DEBUG
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage("[Reader]:" + sql, paramInfo);
                    }
#endif
                    reader = _comm.ExecuteReader();
                }
                
            }
            catch (Exception e)
            {
                //如果正在执行事务，回滚
                //RoolBack();
                throw e;
            }
            
            if (paramList != null)
            {
                paramList.ReturnParameterValue(_comm, _db);
            }
			return reader;
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
#if DEBUG
                    if (_db.SqlOutputer.HasOutput)
                    {
                        OutMessage("[RollbackTransation]");
                    }
#endif
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
			ParamList paramList)
		{
			return Execute(sql,paramList,CommandType.Text);
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
			CommandType exeCommandType)
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
#if DEBUG
                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage("[NonQuery]:" + sql, paramInfo);
                }
#endif
                ret = _comm.ExecuteNonQuery();
                
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
            if (paramList != null)
            {
                paramList.ReturnParameterValue(_comm, _db);
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
#if DEBUG
                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage("[BeginTransation:Level=" + isolationLevel.ToString() + "]");
                }
#endif
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
		public bool Commit()
		{
			//如果没有开启事务处理功能，不做任何操作，直接返回成功
            if (!IsTran)
			{
				return false;
			}

            try
            {
#if DEBUG
                if (_db.SqlOutputer.HasOutput)
                {
                    OutMessage("[CommitTransation]");
                }
#endif
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
		
		~DataBaseOperate()
		{
			CloseDataBase(); 
		}

	}
}
