using System;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;
using System.Data.Common;
using System.Collections.Generic;

///ͨ��SQL Server������v1.2

namespace Buffalo.DB.DbCommon
{
    /// <summary>
    /// �ύ��ʽ
    /// </summary>
    public enum CommitState 
    {
        /// <summary>
        /// �Զ��ύ(�ر�����)
        /// </summary>
        AutoCommit,
        /// <summary>
        /// �ֶ��ύ(�ر�����)
        /// </summary>
        UserCommit
    }

	/// <summary>
	/// ���ݿ������
	/// </summary>
	public class DataBaseOperate : IDisposable
	{
		
		
		// ���ݿ����ӱ�־
		private bool _isConnected;
		
		
		//���ݿ����Ӷ���
        private DbConnection _conn;

        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// �����Ƿ��Ѿ�����
        /// </summary>
        private bool IsTran 
        {
            get 
            {
                return _tran != null;
            }
        }

        //�Ƿ��Զ��ر�����
        private CommitState _commitState = CommitState.AutoCommit;

        string _databaseName;

        /// <summary>
        /// ��ȡ���ݿ�����
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
                        //���������ݿ�ʧ���׳�����
                        if (!ConnectDataBase())
                        {
                            throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// �Ƿ��Զ��ر�����
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
        /// �������ݿ���Ϣ
        /// </summary>
        public DBInfo DBInfo 
        {
            get 
            {
                return _db;
            }
        }
        /// <summary>
        /// �������������������
        /// </summary>
        /// <returns></returns>
        public BatchAction StarBatchAction() 
        {
            BatchAction action = new BatchAction(this);
            return action;
        }

		/// <summary>
		/// ʵ�������ݿ���ʶ���
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
        /// ���ش� System.Data.Common.DbConnection ������Դ�ļܹ���Ϣ
        /// </summary>
        /// <returns></returns>
        public DataTable GetSchema() 
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// ���ش� System.Data.Common.DbConnection ������Դ�ļܹ���Ϣ
        /// </summary>
        /// <param name="collectionName">ָ��Ҫ���صļܹ�������</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// ���ش� System.Data.Common.DbConnection ������Դ�ļܹ���Ϣ
        /// </summary>
        /// <param name="collectionName">ָ��Ҫ���صļܹ�������</param>
        /// <param name="restrictionValues">Ϊ����ļܹ�ָ��һ������ֵ</param>
        /// <returns></returns>
        public DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            DataTable dt = null;
            try
            {
                if (!ConnectDataBase())
                {
                    throw (new ApplicationException("û�н������ݿ����ӡ�"));
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
		/// �������ݿ⣬�������ݿ�����
		/// </summary>
		/// <returns>�ɹ�����true</returns>
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

		
		#region IDisposable ��Ա
		/// <summary>
		/// �ͷ�ռ����Դ
		/// </summary>
		public void Dispose()
		{
            CloseDataBase();
			GC.SuppressFinalize(this); 
		}

        /// <summary>
        /// �ر����ݿ�����
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
        /// ���ձ�ʶλ�Զ��ر�����
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
		#region ��ѯ������������
		/// <summary>
		/// ���в�ѯ�ķ���,����һ��DataSet
		/// </summary>
		/// <param name="sql">Ҫ��ѯ��SQL���</param>
		/// <param name="sTableName">��ѯ�����ı���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <returns>���ؽ����</returns>
        public DataSet QueryDataSet(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cacheTables)
		{

            return QueryDataSet(sql, paramList, CommandType.Text,cacheTables);
		}
		


		/// <summary>
		/// ���в�ѯ�ķ���,����һ��DataSet
		/// </summary>
		/// <param name="sql">Ҫ��ѯ��SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <param name="queryCommandType">SQL�������</param>
		/// <returns>���ؽ����</returns>
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
			//���������ݿ�ʧ���׳�����
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ����ӡ�"));
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
                //�������ִ�����񣬻ع�
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
		/// ���в�ѯ�ķ���������һ��DataReader���ʺ�С���ݵĶ�ȡ
		/// </summary>
		/// <param name="sql">Ҫ��ѯ��SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <returns>����DataReader</returns>
		public IDataReader Query(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Query(sql,paramList,CommandType.Text,cachetables);
		}

		/// <summary>
		/// ���в�ѯ�ķ���������һ��DataReader���ʺ�С���ݵĶ�ȡ
		/// </summary>
		/// <param name="sql">Ҫ��ѯ��SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <param name="queryCommandType">SQL�������</param>
		/// <returns>����DataReader</returns>
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

			//���������ݿ�ʧ���׳�����
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ����ӡ�"));
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

                //���뻺��
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
                //�������ִ�����񣬻ع�
                //RoolBack();
                throw e;
            }
            

			return reader;
		}


        /// <summary>
        /// ���Ӱ������
        /// </summary>
        public int LastAffectedRows
        {
            get { return _lastAffectedRows; }
        }

        /// <summary>
        /// �ع�����
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
		/// ִ���޸����ݿ�������޸ġ�ɾ�����޷���ֵ�Ĳ���
		/// </summary>
		/// <param name="sql">ִ�е�SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <returns>�ɹ�ִ�з���True</returns>
		public int Execute(
			string	sql,
            ParamList paramList, Dictionary<string, bool> cachetables)
		{
			return Execute(sql,paramList,CommandType.Text,cachetables);
		}

		/// <summary>
		/// ִ���޸����ݿ�������޸ġ�ɾ�����޷���ֵ�Ĳ���
		/// </summary>
		/// <param name="sql">ִ�е�SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <param name="queryCommandType">SQL�������</param>
		/// <returns>�ɹ�ִ�з���True</returns>
		public int Execute(
			string	sql,
			ParamList paramList,
			CommandType exeCommandType,
            Dictionary<string, bool> cacheTables)
		{
			
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ�����")); 
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
                //�������ִ�����񣬻ع�
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
        /// ��������
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        public DBTransation StartTransation()
        {
            return StartTransation(IsolationLevel.ReadCommitted);
        }

		/// <summary>
		/// ��ʼ�������ܣ�֮��ִ�е�ȫ�����ݿ���������Ҫ�����ύ������_commit����Ч
		/// </summary>
        internal bool StartTran(IsolationLevel isolationLevel)
		{
			//���������ݿ�ʧ���׳�����
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ����ӡ�"));
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
        /// ��ǰ�����������ύ��ʧ��ȫ���ع�
		/// </summary>
		/// <returns></returns>
		internal bool Commit()
		{
			//���û�п����������ܣ������κβ�����ֱ�ӷ��سɹ�
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
