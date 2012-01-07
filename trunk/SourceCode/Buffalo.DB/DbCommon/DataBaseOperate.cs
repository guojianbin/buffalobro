using System;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.MessageOutPuters;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;

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
		private IDbConnection _conn;
		private IDbCommand _comm;
		private IDbTransaction _tran;
		private IDbDataAdapter _sda;

        
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
        /// �����Ϣ
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
			ParamList paramList)
		{

            return QueryDataSet(sql, paramList, CommandType.Text);
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
			CommandType queryCommandType
			)
		{
				
			//���������ݿ�ʧ���׳�����
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ����ӡ�"));
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
                //�������ִ�����񣬻ع�
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
		/// ���в�ѯ�ķ���������һ��DataReader���ʺ�С���ݵĶ�ȡ
		/// </summary>
		/// <param name="sql">Ҫ��ѯ��SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <returns>����DataReader</returns>
		public IDataReader Query(
			string	sql,
			ParamList paramList)
		{
			return Query(sql,paramList,CommandType.Text);
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
			CommandType exeCommandType
			)
		{
				
			//���������ݿ�ʧ���׳�����
			if (!ConnectDataBase())
			{
				throw(new ApplicationException("û�н������ݿ����ӡ�"));
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
                //�������ִ�����񣬻ع�
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
        /// �ع�����
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
		/// ִ���޸����ݿ�������޸ġ�ɾ�����޷���ֵ�Ĳ���
		/// </summary>
		/// <param name="sql">ִ�е�SQL���</param>
		/// <param name="paramList">SqlParameter���б�</param>
		/// <returns>�ɹ�ִ�з���True</returns>
		public int Execute(
			string	sql,
			ParamList paramList)
		{
			return Execute(sql,paramList,CommandType.Text);
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
			CommandType exeCommandType)
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
                //�������ִ�����񣬻ع�
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
        /// ��ǰ�����������ύ��ʧ��ȫ���ع�
		/// </summary>
		/// <returns></returns>
		public bool Commit()
		{
			//���û�п����������ܣ������κβ�����ֱ�ӷ��سɹ�
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
