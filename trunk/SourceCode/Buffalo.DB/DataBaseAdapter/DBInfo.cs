using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.MessageOutPuters;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.CommBase;
using Buffalo.DB.DBCheckers;

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
        /// 初始化适配器命名空间列表
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, IAdapterLoader> InitAdapterLoaderName() 
        {
            Dictionary<string, IAdapterLoader> dic = new Dictionary<string, IAdapterLoader>();
            dic["Sql2K"] = new Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AdapterLoader();
            dic["Sql2K5"] = new Buffalo.DB.DataBaseAdapter.SqlServer2K5Adapter.AdapterLoader();
            dic["Sql2K8"] = new Buffalo.DB.DataBaseAdapter.SqlServer2K8Adapter.AdapterLoader();
            dic["Oracle9"] =new Buffalo.DB.DataBaseAdapter.Oracle9Adapter.AdapterLoader();
            dic["MySQL5"] = new Buffalo.DB.DataBaseAdapter.MySQL5Adapter.AdapterLoader();
            dic["SQLite"] = new Buffalo.DB.DataBaseAdapter.SQLiteAdapter.AdapterLoader();
            dic["DB2v9"] = new Buffalo.DB.DataBaseAdapter.IBMDB2V9Adapter.AdapterLoader();
            dic["Psql9"] = new Buffalo.DB.DataBaseAdapter.PostgreSQL9Adapter.AdapterLoader();
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
        /// 检查数据库结构
        /// </summary>
        /// <returns></returns>
        public List<string> CheckDataBase() 
        {
            return DBChecker.CheckDataBase(this);
        }

        /// <summary>
        /// 检查并更新数据库结构
        /// </summary>
        /// <returns></returns>
        public string UpdateDataBase() 
        {
            List<string> sql=DBChecker.CheckDataBase(this);
            List<string> res=DBChecker.ExecuteSQL(DefaultOperate, sql);
            StringBuilder sbRet = new StringBuilder();
            foreach (string str in res)
            {
                sbRet.AppendLine(str);
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// 抛出找不到表的异常
        /// </summary>
        /// <param name="eType"></param>
        internal void ThrowNotFondTable(Type eType)
        {
            throw new Exception("找不到实体：" + eType.Name + " 对应的表，请检查程序启动时候数据库：" + _dbName + " 是否已经调用InitDB()方法进行初始化");
        }

        /// <summary>
        /// 初始化数据库适配器
        /// </summary>
        private void InitAdapters() 
        {

            IAdapterLoader loader = null;
            if (!_dicAdapterLoaderName.TryGetValue(DbType, out loader)) 
            {
                throw new Exception("不支持数据库类型:" + DbType);
            }

            _curAggregateFunctions = loader.AggregateFunctions;
            _curCommonFunctions = loader.CommonFunctions;
            _curConvertFunctions = loader.ConvertFunctions;
            _curDbAdapter = loader.DbAdapter;
            _curDBStructure = loader.DBStructure;
            _curMathFunctions = loader.MathFunctions;

        }


        /// <summary>
        /// 默认连接
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate DefaultOperate 
        {
            get
            {
                return StaticConnection.GetStaticOperate(this);
            }
        }

        /// <summary>
        /// 创新数据库连接
        /// </summary>
        /// <returns></returns>
        public DataBaseOperate CreateOperate()
        {
            DataBaseOperate oper = new DataBaseOperate(this);
            return oper;
        }
#if DEBUG
        /// <summary>
        /// 输出SQL语句的类
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
        /// 获取当前数据库的名字
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
        /// 数据层名称
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
        /// 获取当前数据库的适配器
        /// </summary>
        /// <returns></returns>
        public IDBAdapter CurrentDbAdapter
        {
            get
            {

                return _curDbAdapter;
            }
        }
        /// <summary>
        /// 获取聚合函数的处理
        /// </summary>
        /// <returns></returns>
        public IAggregateFunctions Aggregate
        {
            get
            {

                return _curAggregateFunctions;
            }
        }

        /// <summary>
        /// 获取数学函数的处理
        /// </summary>
        /// <returns></returns>
        public IMathFunctions Math
        {
            get
            {

                return _curMathFunctions;
            }
        }

        /// <summary>
        /// 获取转换函数的处理
        /// </summary>
        /// <returns></returns>
        public IConvertFunction Convert
        {
            get
            {

                return _curConvertFunctions;
            }
        }

        /// <summary>
        /// 获取常用函数的处理的处理
        /// </summary>
        /// <returns></returns>
        public ICommonFunction Common
        {
            get
            {

                return _curCommonFunctions;
            }
        }
        /// <summary>
        /// 数据库结构特性
        /// </summary>
        /// <returns></returns>
        public IDBStructure DBStructure
        {
            get
            {

                return _curDBStructure;
            }
        }
        /// <summary>
        /// 当前数据库的类型
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
        /// 数据库的连接字符串
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
        /// 添加到库信息
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
        /// 获取所有表
        /// </summary>
        /// <returns></returns>
        public List<BQLEntityTableHandle> GetAllTables() 
        {
            List<BQLEntityTableHandle> allTable = new List<BQLEntityTableHandle>(_dicTables.Count);
            foreach (KeyValuePair<string, BQLEntityTableHandle> kvp in _dicTables) 
            {
                allTable.Add(kvp.Value);
            }
            return allTable;
        }

        /// <summary>
        /// 通过实体类型查找对应的BQL表信息
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
