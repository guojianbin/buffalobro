using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.CommBase;

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
        private IDBStructure _curDBStructure = null;

        private static Dictionary<string, IAdapterLoader> _dicAdapterLoaderName = InitAdapterLoaderName();

        /// <summary>
        /// ��ʼ�������������ռ��б�
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, IAdapterLoader> InitAdapterLoaderName() 
        {
            Dictionary<string, IAdapterLoader> dic = new Dictionary<string, IAdapterLoader>();
            dic["Sql2K"] = new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AdapterLoader();
            dic["Sql2K5"] = new Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter.AdapterLoader();
            dic["Oracle9"] =new Buffalo.DB.DataBaseAdapter.Oracle9Adapter.AdapterLoader();
            dic["MySQL5"] = new Buffalo.DB.DataBaseAdapter.MySQL5Adapter.AdapterLoader();
            dic["SQLite"] = new Buffalo.DB.DataBaseAdapter.SQLiteAdapter.AdapterLoader();
            dic["DB2v9"] = new Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter.AdapterLoader();
            return dic;
        }

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
            InitAdapters();
        }
        /// <summary>
        /// �׳��Ҳ�������쳣
        /// </summary>
        /// <param name="eType"></param>
        internal void ThrowNotFondTable(Type eType)
        {
            throw new Exception("�Ҳ���ʵ�壺" + eType.Name + " ��Ӧ�ı������������ʱ�����ݿ⣺" + _dbName + " �Ƿ��Ѿ�����InitDB()�������г�ʼ��");
        }

        /// <summary>
        /// ��ʼ�����ݿ�������
        /// </summary>
        private void InitAdapters() 
        {

            IAdapterLoader loader = null;
            if (!_dicAdapterLoaderName.TryGetValue(DbType, out loader)) 
            {
                throw new Exception("��֧�����ݿ�����:" + DbType);
            }

            _curAggregateFunctions = loader.AggregateFunctions;
            _curCommonFunctions = loader.CommonFunctions;
            _curConvertFunctions = loader.ConvertFunctions;
            _curDbAdapter = loader.DbAdapter;
            _curDBStructure = loader.DBStructure;
            _curMathFunctions = loader.MathFunctions;

        }

        DataBaseOperate _defaultOper;

        /// <summary>
        /// Ĭ������
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate DefaultOperate 
        {
            get
            {
                if (_defaultOper == null)
                {
                    _defaultOper = StaticConnection.GetStaticOperate(this);
                }
                return _defaultOper;
            }
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
        private string[] _dataaccessNamespace;

        /// <summary>
        /// ���ݲ�����
        /// </summary>
        public string[] DataaccessNamespace
        {
            get
            {
                return _dataaccessNamespace;
            }
            set
            {
                _dataaccessNamespace = value;
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
        /// ���ݿ�ṹ����
        /// </summary>
        /// <returns></returns>
        internal IDBStructure DBStructure
        {
            get
            {

                return _curDBStructure;
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

        


        private Dictionary<string, BQLEntityTableHandle> _dicTables = new Dictionary<string, BQLEntityTableHandle>();

        /// <summary>
        /// ��ӵ�����Ϣ
        /// </summary>
        /// <param name="table"></param>
        internal void AddToDB(BQLEntityTableHandle table) 
        {
            string key = table.GetEntityInfo().EntityType.FullName;
            if (!_dicTables.ContainsKey(key))
            {
                _dicTables[key]=table;
            }
        }

        /// <summary>
        /// ͨ��ʵ�����Ͳ��Ҷ�Ӧ��BQL����Ϣ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public BQLEntityTableHandle FindTable(Type entityType)
        {
            BQLEntityTableHandle ret = null;
            _dicTables.TryGetValue(entityType.FullName, out ret);
            return ret;
        }
    }
}
