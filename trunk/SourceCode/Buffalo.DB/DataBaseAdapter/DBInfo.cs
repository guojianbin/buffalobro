using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalo.DB.DataBaseAdapter
{
    public class DBInfo
    {
        private string _dbName = null;
        private IDBAdapter _curDbAdapter = null;
        
        private IAggregateFunctions _curAggregateFunctions = null;

        private IMathFunctions _curMathFunctions = null;
        private IConvertFunction _curConvertFunctions = null;
        private ICommonFunction _curCommonFunctions = null;
#if DEBUG
        MessageOutput _sqlOutputer = new MessageOutput();
#endif
        private string _connectionString = null;
        private string _dbType = null;
        //private Dictionary<string, string> _extendDatabaseConnection = null;

        public DBInfo(string dbName,string connectionString, 
            string dbType
            ) 
        {
            _dbType = dbType;
            _connectionString = connectionString;
            _dbName = dbName;
            _curDbAdapter = LoadDBAdapter();
            _curAggregateFunctions = LoadAggregate();
            _curConvertFunctions = LoadConvert();
            _curCommonFunctions = LoadCommon();
           // _extendDatabaseConnection = extendDatabaseConnection;
        }

        /// <summary>
        /// �������ݿ�����
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate CreateOperate()
        {
            DataBaseOperate oper = new DataBaseOperate(this);
            return oper;
        }
#if DEBUG
        /// <summary>
        /// ���SQL������
        /// </summary>
        public MessageOutput SqlOutputer 
        {
            get 
            {
                return _sqlOutputer;
            }
        }
#endif
        /// <summary>
        /// ��ȡ��ǰ���ݿ������
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {

                return _dbName;
            }
        }

        /// <summary>
        /// ��ȡ��ǰ���ݿ��������
        /// </summary>
        /// <returns></returns>
        internal IDBAdapter CurrentDbAdapter
        {
            get
            {

                return _curDbAdapter;
            }
        }
        /// <summary>
        /// ��ȡ�ۺϺ����Ĵ���
        /// </summary>
        /// <returns></returns>
        internal IAggregateFunctions Aggregate
        {
            get
            {

                return _curAggregateFunctions;
            }
        }

        /// <summary>
        /// ��ȡ��ѧ�����Ĵ���
        /// </summary>
        /// <returns></returns>
        internal IMathFunctions Math
        {
            get
            {

                return _curMathFunctions;
            }
        }

        /// <summary>
        /// ��ȡת�������Ĵ���
        /// </summary>
        /// <returns></returns>
        internal IConvertFunction Convert
        {
            get
            {

                return _curConvertFunctions;
            }
        }

        /// <summary>
        /// ��ȡ���ú����Ĵ���Ĵ���
        /// </summary>
        /// <returns></returns>
        internal ICommonFunction Common
        {
            get
            {

                return _curCommonFunctions;
            }
        }

        /// <summary>
        /// ��ǰ���ݿ������
        /// </summary>
        public string DbType
        {
            get
            {
                //InitBaseConfig();
                return _dbType;
            }
        }



        /// <summary>
        /// ���ݿ�������ַ���
        /// </summary>
        public string ConnectionString 
        {
            get 
            {
                return _connectionString;
            }
            set 
            {
                _connectionString = value;
            }
        }

        /// <summary>
        /// �������ͻ�ȡ���ݿ��������
        /// </summary>
        /// <returns></returns>
        private IDBAdapter LoadDBAdapter()
        {
            IDBAdapter ret = null;
            string type = _dbType;
            if (type == null)
            {
                ret = new SqlServer2KAdapter.DBAdapter();
            }
            else if (type.Equals("sql2k",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.DBAdapter();
            }
            else if (type.Equals("sql2k5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2K5Adapter.DBAdapter();
            }
            else if (type.Equals("oracle9",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Oracle9Adapter.DBAdapter();
            }
            else if (type.Equals("mysql5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new MySQL5Adapter.DBAdapter();
            }
            else if (type.Equals( "sqlite",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SQLiteAdapter.DBAdapter();
            }
            else if (type.Equals("db2v9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new IBMDB2V9Adapter.DBAdapter();
            }
            return ret;
        }

        /// <summary>
        /// �������ͻ�ȡ���ݿ�ľۺϺ���������
        /// </summary>
        /// <returns></returns>
        private IAggregateFunctions LoadAggregate()
        {
            IAggregateFunctions ret = null;
            string type = _dbType;
            if (type == null)
            {
                ret = new SqlServer2KAdapter.AggregateFunctions();
            }
            else if (type.Equals("sql2k",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.AggregateFunctions();
            }
            else if (type.Equals("sql2k5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2K5Adapter.AggregateFunctions();
            }
            else if (type.Equals("oracle9",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Oracle9Adapter.AggregateFunctions();
            }
            else if (type.Equals("mysql5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new MySQL5Adapter.AggregateFunctions();
            }
            else if (type.Equals("sqlite",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SQLiteAdapter.AggregateFunctions();
            }
            else if (type.Equals("db2v9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new IBMDB2V9Adapter.AggregateFunctions();
            }
            return ret;
        }

        /// <summary>
        /// �������ͻ�ȡ���ݿ�ľۺϺ���������
        /// </summary>
        /// <returns></returns>
        private ICommonFunction LoadCommon()
        {
            ICommonFunction ret = null;
            string type = _dbType;
            if (type == null)
            {
                ret = new SqlServer2KAdapter.CommonFunction();
            }
            else if (type.Equals("sql2k",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.CommonFunction();
            }
            else if (type.Equals("sql2k5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.CommonFunction();
            }
            else if (type.Equals("oracle9",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Oracle9Adapter.CommonFunction();
            }
            else if (type.Equals("mysql5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new MySQL5Adapter.CommonFunction();
            }
            else if (type.Equals("sqlite",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SQLiteAdapter.CommonFunction();
            }
            else if (type.Equals("db2v9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new IBMDB2V9Adapter.CommonFunction();
            }
            return ret;
        }
        /// <summary>
        /// �������ͻ�ȡ���ݿ����ѧ����������
        /// </summary>
        /// <returns></returns>
        private IMathFunctions LoadMath()
        {
            IMathFunctions ret = null;
            string type = _dbType;
            if (type == null)
            {
                ret = new SqlServer2KAdapter.MathFunctions();
            }
            else if (type.Equals("sql2k",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.MathFunctions();
            }
            else if (type.Equals("sql2k5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.MathFunctions();
            }
            else if (type.Equals("oracle9",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Oracle9Adapter.MathFunctions();
            }
            else if (type.Equals("mysql5",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new MySQL5Adapter.MathFunctions();
            }
            else if (type.Equals("sqlite",StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SQLiteAdapter.MathFunctions();
            }
            else if (type.Equals("db2v9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new IBMDB2V9Adapter.MathFunctions();
            }
            return ret;
        }
        /// <summary>
        /// �������ͻ�ȡ���ݿ����ֵת������������
        /// </summary>
        /// <returns></returns>
        private IConvertFunction LoadConvert()
        {
            IConvertFunction ret = null;
            string type = _dbType;
            if (type == null)
            {
                ret = new SqlServer2KAdapter.ConvertFunction();
            }
            else if (type.Equals("sql2k", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.ConvertFunction();
            }
            else if (type.Equals("sql2k5", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SqlServer2KAdapter.ConvertFunction();
            }
            else if (type.Equals("oracle9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new Oracle9Adapter.ConvertFunction();
            }
            else if (type.Equals("mysql5", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new MySQL5Adapter.ConvertFunction();
            }
            else if (type.Equals("sqlite", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new SQLiteAdapter.ConvertFunction();
            }
            else if (type.Equals("db2v9", StringComparison.CurrentCultureIgnoreCase))
            {
                ret = new IBMDB2V9Adapter.ConvertFunction();
            }
            return ret;
        }


        private Dictionary<string, CsqlEntityTableHandle> _dicTables = new Dictionary<string, CsqlEntityTableHandle>();

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="table"></param>
        internal void AddToDB(CsqlEntityTableHandle table) 
        {
            string key = table.GetEntityInfo().EntityType.FullName;
            if (!_dicTables.ContainsKey(key))
            {
                _dicTables[key]=table;
            }
        }

        /// <summary>
        /// ͨ��ʵ�����Ͳ��Ҷ�Ӧ��CSQL����Ϣ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public CsqlEntityTableHandle FindTable(Type entityType)
        {
            CsqlEntityTableHandle ret = null;
            _dicTables.TryGetValue(entityType.FullName, out ret);
            return ret;
        }
    }
}
