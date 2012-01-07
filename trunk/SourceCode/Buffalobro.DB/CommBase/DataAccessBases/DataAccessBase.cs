using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Buffalobro.DB.PropertyAttributes;
using System.Collections;
using Buffalobro.DB.CacheManager;
using Buffalobro.DB.ContantSearchs;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.DataFillers;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;
using Buffalobro.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalobro.DB.CsqlCommon;
using Buffalobro.DB.CommBase.BusinessBases;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// ���ݲ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataAccessBase<T> where T : EntityBase, new()
    {
        private DataBaseOperate oper;
        
        /// <summary>
        /// ��ǰ���͵���Ϣ
        /// </summary>
        protected internal readonly static EntityInfoHandle CurEntityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
        private static string allParams=null;

        private CsqlDbBase _cdal;//Csql���ݲ���

        /// <summary>
        /// Csql������
        /// </summary>
        internal CsqlDbBase ContextDAL 
        {
            get 
            {
                return _cdal;
            }
        }

        /// <summary>
        /// �����ֶ���
        /// </summary>
        protected static string AllParamNames 
        {
            get 
            {
                if (allParams == null) 
                {
                    StringBuilder ret = new StringBuilder();
                    foreach (EntityPropertyInfo info in CurEntityInfo.PropertyInfo) 
                    {
                        ret.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName) + ",");
                    }
                    if (ret.Length > 0)
                    {
                        allParams = ret.ToString(0, ret.Length - 1);
                    }
                }
                return allParams;
            }
        }
        /// <summary>
        /// ���ݿ����Ӷ���
        /// </summary>
        protected internal DataBaseOperate Oper
        {
            get
            {
                return oper;
            }
            set
            {
                oper = value;
                _cdal = new CsqlDbBase(oper);
            }
        }

        
        #region ��������

        #region ���ݶ���

        /// <summary>
        /// ��ȡ���������϶�Ӧ���ֶ�������
        /// </summary>
        /// <param name="propertyNames">����������</param>
        /// <returns></returns>
        private static List<PropertyParamMapping> GetParamNameList(List<string> propertyNames)
        {
            
            List<PropertyParamMapping> lstParamMapping = new List<PropertyParamMapping>(propertyNames.Count);
            foreach (string propertyName in propertyNames)
            {
                EntityPropertyInfo info = CurEntityInfo.PropertyInfo[propertyName];
                if (info!=null)
                {
                    PropertyParamMapping objMapping = new PropertyParamMapping();
                    objMapping.PropertyName = propertyName;
                    objMapping.ParamName = info.ParamName;
                    objMapping.DataType = info.SqlType;
                    lstParamMapping.Add(objMapping);
                }
            }
            return lstParamMapping;
        }
        #endregion



        #region �������

        

        
        /// <summary>
        /// ��ȡ����������SQL���(����OrderBy)
        /// </summary>
        /// <param name="lstSort">������������</param>
        /// <returns></returns>
        protected static string GetSortCondition(SortList lstSort)
        {
            if (lstSort == null)
            {
                return "";
            }
            //����ʽ
            StringBuilder orderBy = new StringBuilder(500);
            foreach (Sort objSort in lstSort)
            {
                EntityPropertyInfo info = CurEntityInfo.PropertyInfo[objSort.PropertyName];
                if (info!=null)
                {
                    string strSort = "ASC";
                    if (objSort.SortType == SortType.DESC)
                    {
                        strSort = "DESC";
                    }
                    orderBy.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                    orderBy.Append(" ");
                    orderBy.Append(strSort);
                    orderBy.Append(",");
                }
                else
                {
                    throw new Exception("�����������������Ҳ������ԣ�" + objSort.PropertyName);
                }
            }
            if (orderBy.Length>0)
            {
                orderBy.Remove(orderBy.Length - 1, 1);
                //orderBy = orderBy.Substring(0, orderBy.Length - 1);
            }
            return orderBy.ToString();
        }
        
        /// <summary>
        /// ��ȡ���β�ѯ��Ҫ��ʾ���ֶμ���
        /// </summary>
        /// <param name="lstScope">��Χ��ѯ����</param>
        /// <returns></returns>
        private static string GetSelectParams(ScopeList lstScope) 
        {
            
            if (lstScope == null) 
            {
                return AllParamNames;
            }
            
            StringBuilder ret = new StringBuilder();
            
            List<CsqlParamHandle> propertyNames = lstScope.GetShowProperty(CurEntityInfo.DBInfo.FindTable(typeof(T)));


            if (propertyNames.Count > 0)
            {
                foreach (CsqlParamHandle property in propertyNames)
                {
                    CsqlEntityParamHandle eproperty = property as CsqlEntityParamHandle;
                    if (CommonMethods.IsNull(eproperty))
                    {
                        continue;
                    }
                    EntityPropertyInfo info = eproperty.PInfo;
                    if (info != null)
                    {
                        ret.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName) + ",");
                    }
                }
            }
            else 
            {
                foreach (EntityPropertyInfo info in CurEntityInfo.PropertyInfo)
                {
                    if (info != null)
                    {
                        ret.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName) + ",");
                    }
                }
            }


            if (ret.Length > 0) 
            {
                return ret.ToString(0, ret.Length - 1);
            }
            return AllParamNames;
        }

        /// <summary>
        /// ��ȡȫ����ѯ������
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="scopeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        protected string GetSelectPageContant(ParamList list,  ScopeList scopeList)
        {

            SelectCondition condition = new SelectCondition(CurEntityInfo.DBInfo);
            condition.Oper = this.oper;
            condition.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));
            condition.SqlParams.Append(GetSelectParams(scopeList)) ;
            condition.Condition.Append("1=1");
            condition.PrimaryKey.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(CurEntityInfo.PrimaryProperty.ParamName));
            string conditionWhere = "";

            SortList sortList = scopeList.OrderBy;
            

            if (scopeList != null)
            {
                condition.Condition.Append(DataAccessCommon.FillCondition(CurEntityInfo,list, scopeList));
            }

            if (conditionWhere.Length > 0)
            {
                condition.Condition.Append(conditionWhere);
            }
            //����ʽ
            if (sortList != null && sortList.Count>0)
            {
                string orderBy = GetSortCondition(sortList);
                if (orderBy != "")
                {
                    if (condition.Orders.Length>0)
                    {
                        condition.Orders.Append( "," + orderBy);
                    }
                    else
                    {
                        condition.Orders.Append(orderBy);
                    }
                }
            }
            condition.PageContent = scopeList.PageContent;
            //throw new Exception("");
            condition.DbParamList = list;
            return condition.GetSql();
        }

        

        /// <summary>
        /// ��ȡȫ����ѯ������
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="scopeList">��Χ���ҵļ���</param>
        /// <param name="param">����ֶ�</param>
        /// <returns></returns>
        protected SelectCondition GetSelectContant(ParamList list, ScopeList scopeList, string param)
        {
            string conditionWhere = "";
            string orderBy = "";
            SelectCondition condition = new SelectCondition(CurEntityInfo.DBInfo);
            if (condition.SqlParams.Length > 0) 
            {
                condition.SqlParams.Append(",");
            }
            condition.SqlParams.Append(param);

            condition.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));

            condition.Condition.Append("1=1");
            if (scopeList != null)
            {
                condition.Condition.Append(DataAccessCommon.FillCondition(CurEntityInfo,list, scopeList));
            }
            if (conditionWhere.Length > 0)
            {
                condition.Condition.Append(conditionWhere);
            }
            SortList sortList = scopeList.OrderBy;
            if (sortList != null && sortList.Count>0)
            {
                if (orderBy != "")
                {
                    orderBy = orderBy + "," + GetSortCondition(sortList);
                }
                else
                {
                    orderBy = GetSortCondition(sortList);
                }
            }

            if (orderBy != "")
            {
                condition.Orders.Append(orderBy);
            }
            condition.DbParamList = list;
            return condition;
        }
        #endregion
        #region ���ݼ����
        /// <summary>
        /// ��Reader��߶�ȡ����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        protected T LoadFromReader(IDataReader reader)
        {
            return CacheReader.LoadFormReader<T>(reader, CurEntityInfo); ;
        }

        /// <summary>
        /// ��Reader��߶�ȡ����
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        protected List<T> LoadFromReaderList(IDataReader reader)
        {
            return CacheReader.LoadFormReaderList<T>(reader);
        }

        
        #endregion
        #region ִ�в���
        /// <summary>
        /// ִ��Sql����
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="list">�����б�</param>
        /// <param name="commandType">��������</param>
        public int ExecuteCommand(string sql, ParamList list, CommandType commandType)
        {
            int ret = -1;
            ret = oper.Execute(sql, list, commandType);
            return ret;
        }

        /// <summary>
        /// ִ��sql��䣬����DataSet
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="list">�����б�</param>
        /// <param name="commandType">�������</param>
        public DataSet QueryDataSet(string sql, ParamList list, CommandType commandType)
        {
            DataSet ds =null;
            ds = oper.QueryDataSet(sql, list, commandType);
            return ds;
        }
        
        /// <summary>
        /// ִ��sql��䣬����List
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="list">�����б�</param>
        /// <param name="commandType">�������</param>
        public List<T> QueryList(string sql, ParamList list, CommandType commandType)
        {
            List<T> retlist = null;
            using(IDataReader reader = oper.Query(sql, list, commandType))
            {
            
                retlist = LoadFromReaderList(reader);
            }
        
            return retlist;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����List(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public List<T> QueryList(string sql, PageContent objPage)
        {
            List<T> retlist = null;

            
            using (IDataReader reader = CurEntityInfo.DBInfo.CurrentDbAdapter.Query(sql, objPage, oper))
            {

                retlist = LoadFromReaderList(reader);
            }
            return retlist;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public DataSet QueryDataSet(string sql, PageContent objPage)
        {
            DataSet ds = new DataSet();
            DataTable retDt = CurEntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, objPage, oper, null);
            ds.Tables.Add(retDt);
            return ds;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����������������ӳ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        protected DataSet QueryMappingDataSet(string sql, PageContent objPage)
        {
            DataSet ds = new DataSet();

            DataTable retDt = CurEntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, objPage, oper, typeof(T));
            ds.Tables.Add(retDt);
            return ds;
        }

        /// <summary>
        /// ִ��sql��䣬��ҳ����List(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        public List<T> QueryList(string sql, ParamList lstParam, PageContent objPage)
        {
            List<T> retlist = null;
            using (IDataReader reader = CurEntityInfo.DBInfo.CurrentDbAdapter.Query(sql, lstParam, objPage, oper))
            {

                retlist = LoadFromReaderList(reader);
            }
            return retlist;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        public DataSet QueryDataSet(string sql, ParamList lstParam, PageContent objPage)
        {
            DataSet ds = new DataSet();
            DataTable retDt = CurEntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, lstParam, objPage, oper, null);
            ds.Tables.Add(retDt);
            return ds;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����������������ӳ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        protected DataSet QueryMappingDataSet(string sql, ParamList lstParam, PageContent objPage)
        {
            DataSet ds = new DataSet();
            DataTable retDt = CurEntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, lstParam, objPage, oper, typeof(T));
            ds.Tables.Add(retDt);
            return ds;
        }
        #endregion

        #endregion


       /// <summary>
        /// ����ID��ȡ��¼
       /// </summary>
       /// <param name="id">ID</param>
       /// <param name="isSearchByCache">�Ƿ񻺴�����</param>
       /// <returns></returns>
        public T GetObjectById(object id)
        {
            
            ParamList list = null;
            T ret = default(T);
            list = new ParamList();
            string tabName = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName);
            StringBuilder sql = new StringBuilder(500);
            //string sql = "SELECT " + AllParamNames + " FROM " + tabName + " WHERE 1=1";
            sql.Append("SELECT ");
            sql.Append(AllParamNames);
            sql.Append(" FROM ");
            sql.Append(tabName);
            sql.Append(" WHERE 1=1");
            ScopeList lstScope = new ScopeList();
            lstScope.AddEqual(CurEntityInfo.PrimaryProperty.PropertyName, id);
            sql.Append( DataAccessCommon.FillCondition(CurEntityInfo,list, lstScope));
            
            
            using (IDataReader reader = oper.Query(sql.ToString(), list))
            {
                if (reader.Read())
                {
                    ret = LoadFromReader(reader);
                }
            }
            
            return ret;
        }
        /// <summary>
        /// ����������ȡ��һ����¼
        /// </summary>
        /// <param name="scopeList">��ѯ��Ϣ</param>
        /// <returns></returns>
        public T GetUnique(ScopeList scopeList)
        {
            if (scopeList.HasInner)
            {
                
                return _cdal.GetUnique<T>(scopeList);
            }
            ParamList list = null;
            T ret = default(T);
            list = new ParamList();

            string sql = null;

            SelectCondition sc = GetSelectContant(list, scopeList, GetSelectParams(scopeList));
            sql = CurEntityInfo.DBInfo.CurrentDbAdapter.GetTopSelectSql(sc, 1);
            using (IDataReader reader = oper.Query(sql, list))
            {
                if (reader.Read())
                {
                    ret = LoadFromReader(reader);

                }
            }

            return ret;
        }
        /// <summary>
        /// �޸ļ�¼
        /// </summary>
        /// <param name="obj">�޸ĵĶ���</param>
        /// <param name="scopeList">�����б�</param>
        /// <param name="optimisticConcurrency">�Ƿ���в�������</param>
        /// <returns></returns>
        public int Update(T obj, ScopeList scopeList, bool optimisticConcurrency)
        {
            StringBuilder sql = new StringBuilder(500);
            ParamList list = new ParamList();
            StringBuilder where = new StringBuilder(500);
            //where.Append("1=1");
            Type type = CurEntityInfo.EntityType;
            List<VersionInfo> lstVersionInfo =null;
            int index = 0;
            ///��ȡ���Ա���
            foreach (EntityPropertyInfo info in CurEntityInfo.PropertyInfo)
            {
                object curValue = info.GetValue(obj);
                if (optimisticConcurrency == true && info.IsVersion) //��������
                {
                    
                    object newValue=FillUpdateConcurrency(sql, info, list, curValue, ref index);
                    FillWhereConcurrency(where, info, list, curValue, ref index);
                    if (lstVersionInfo == null) 
                    {
                        lstVersionInfo = new List<VersionInfo>();
                    }
                    lstVersionInfo.Add(new VersionInfo(info, curValue, newValue));//�����Ϣ
                }
                else
                {
                    string paramVal = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                    string paramKey = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));
                    if (info.IsNormal)
                    {
                        if (obj._dicUpdateProperty___ == null || obj._dicUpdateProperty___.Count == 0)
                        {
                            if (DefaultType.IsDefaultValue(curValue))
                            {
                                continue;
                            }
                        }
                        else if (!obj._dicUpdateProperty___.ContainsKey(info.PropertyName))
                        {
                            continue;
                        }

                        sql.Append(",");
                        sql.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sql.Append("=");
                        if (curValue != null)
                        {
                            sql.Append(paramVal);
                            list.AddNew(paramKey, info.SqlType, curValue);
                        }
                        else
                        {
                            sql.Append("null");
                        }
                    }
                    else if (info.IsPrimaryKey)
                    {
                        if (DefaultType.IsDefaultValue(curValue))
                        {
                            continue;
                        }
                        where.Append(" and ");
                        where.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        where.Append("=");
                        where.Append(paramVal);
                        list.AddNew(paramKey, info.SqlType, curValue);
                        //primaryKeyValue=curValue;
                    }
                    index++;
                }


            }

            where.Append(DataAccessCommon.FillCondition(CurEntityInfo, list, scopeList));
            if (sql.Length <= 0)
            {
                return -1;
            }
            else
            {
                sql.Remove(0, 1);

            }
            UpdateCondition con = new UpdateCondition(CurEntityInfo.DBInfo);
            con.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));
            con.UpdateSetValue.Append(sql);
            con.Condition.Append("1=1");
            con.Condition.Append(where);

            int ret = -1;
            ret = ExecuteCommand(con.GetSql(), list, CommandType.Text);
            if (obj._dicUpdateProperty___ != null)
            {
                obj._dicUpdateProperty___.Clear();
            }
            if (lstVersionInfo!=null && lstVersionInfo.Count > 0) 
            {
                foreach (VersionInfo info in lstVersionInfo) 
                {
                    info.Info.SetValue(obj, info.NewValue);
                }
            }

            return ret;
        }


        #region �汾����

        private object FillUpdateConcurrency(StringBuilder sql,
            EntityPropertyInfo info, ParamList list, object curValue, ref int index)
        {
            object newValue = NewConcurrencyValue(curValue);
            if (newValue != null)
            {
                string paramValV = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                string paramKeyV = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));

                sql.Append(",");
                sql.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                sql.Append("=");
                sql.Append(paramValV);
                list.AddNew(paramKeyV, info.SqlType, newValue);
                index++;
            }
            return newValue;
        }

        private object GetDefaultConcurrency(
            EntityPropertyInfo info)
        {
            DbType type = info.SqlType;
            if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
            {
                return DateTime.Now;
            }
            else if (type == DbType.Int64 ||
               type == DbType.Decimal || type == DbType.Double || type == DbType.Int32 ||
           type == DbType.Int16 || type == DbType.Double ||
           type == DbType.SByte || type == DbType.Byte || type == DbType.Currency || type == DbType.UInt16
           || type == DbType.UInt32 || type == DbType.UInt64 || type == DbType.VarNumeric
               )
            {
                return 1;
            }
            return null;
        }

        /// <summary>
        /// ���汾���Ƶ���Ϣ
        /// </summary>
        /// <param name="where"></param>
        /// <param name="where"></param>
        /// <param name="info"></param>
        /// <param name="list"></param>
        /// <param name="curValue"></param>
        protected void FillWhereConcurrency( StringBuilder where, 
            EntityPropertyInfo info, ParamList list, object curValue,ref int index) 
        {
            

            string paramValW = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
            string paramKeyW = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));
            index++;
            if (DefaultType.IsDefaultValue(curValue))
            {
                throw new Exception("�汾�����ֶ�:" + info.PropertyName + " �����е�ǰ�汾ֵ");
            }
            where.Append(" and ");
            where.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
            where.Append("=");
            where.Append(paramValW);
            list.AddNew(paramKeyW, info.SqlType, curValue);
        }

        /// <summary>
        /// �µİ汾ֵ
        /// </summary>
        /// <param name="val">��ǰֵ</param>
        /// <returns></returns>
        private object NewConcurrencyValue(object val)
        {
            Type objType = val.GetType();
            if (DefaultType.EqualType(objType,DefaultType.IntType))
            {
                return (int)val + 1;
            }
            if (DefaultType.EqualType(objType ,DefaultType.DoubleType))
            {
                return (double)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.FloatType))
            {
                return (float)val + 1;
            }
            if (DefaultType.EqualType(objType ,DefaultType.DateTimeType))
            {
                return DateTime.Now;
            }
            if (DefaultType.EqualType(objType , DefaultType.DecimalType))
            {
                return (decimal)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.ByteType))
            {
                return (byte)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.SbyteType))
            {
                return (sbyte)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.ShortType))
            {
                return (short)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.LongType))
            {
                return (long)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.ULongType))
            {
                return (ulong)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.UShortType))
            {
                return (ushort)val + 1;
            }
            if (DefaultType.EqualType(objType , DefaultType.UIntType))
            {
                return (uint)val + 1;
            }

            return null;
        }
        
        #endregion





        /// <summary>
        /// ���в������
        /// </summary>
        /// <param name="obj">Ҫ����Ķ���</param>
        /// <returns></returns>
        protected int DoInsert(T obj, bool returnIdentity)
        {
            StringBuilder sqlParams = new StringBuilder(1000);
            StringBuilder sqlValues = new StringBuilder(1000);
            ParamList list = new ParamList();
            int index = 0;
            string paramVal = null;
            string paramKey = null;
            string param = null;
            string svalue = null;

            EntityPropertyInfo identityInfo = null;

            foreach (EntityPropertyInfo info in CurEntityInfo.PropertyInfo)
            {
                //EntityPropertyInfo info = enums.Current.Value;
                object curValue = info.GetValue(obj);
                paramVal = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                paramKey = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));

                if (DefaultType.IsDefaultValue(curValue)) //�����������û�г�ʼֵ�����Զ�����
                {
                    if (info.Identity)
                    {
                        if (info.SqlType == DbType.Guid)
                        {
                            curValue = Guid.NewGuid();
                            info.SetValue(obj, curValue);
                        }
                        else
                        {
                            if (returnIdentity)
                            {
                                string idenSQL = CurEntityInfo.DBInfo.CurrentDbAdapter.GetIdentityValueSQL(CurEntityInfo);
                                if (!string.IsNullOrEmpty(idenSQL))
                                {
                                    using (IDataReader reader = oper.Query(idenSQL, null))
                                    {
                                        if (reader.Read())
                                        {
                                            //object value = reader[0];

                                            CurEntityInfo.DBInfo.CurrentDbAdapter.SetObjectValueFromReader(reader, 0, obj, info);
                                            curValue = info.GetValue(obj);
                                        }
                                    }
                                }
                                else
                                {
                                    identityInfo = info;
                                    continue;
                                }
                            }
                            else
                            {
                                param = CurEntityInfo.DBInfo.CurrentDbAdapter.GetIdentityParamName(info);
                                if (!string.IsNullOrEmpty(param))
                                {
                                    sqlParams.Append(",");
                                    sqlParams.Append(param);
                                }
                                svalue = CurEntityInfo.DBInfo.CurrentDbAdapter.GetIdentityParamValue(CurEntityInfo, info);
                                if (!string.IsNullOrEmpty(svalue))
                                {
                                    sqlValues.Append(",");
                                    sqlValues.Append(svalue);
                                }
                                continue;
                            }
                        }
                    }
                    else if (info.IsVersion) //�汾��ʼֵ
                    {
                        object conValue = GetDefaultConcurrency(info);
                        if (conValue != null)
                        {
                            sqlParams.Append(",");
                            sqlParams.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                            sqlValues.Append(",");
                            sqlValues.Append(paramVal);
                            list.AddNew(paramKey, info.SqlType, conValue);
                            index++;
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                sqlParams.Append(",");
                sqlParams.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                sqlValues.Append(",");
                sqlValues.Append(paramVal);
                list.AddNew(paramKey, info.SqlType, curValue);
                index++;
            }
            if (sqlParams.Length > 0)
            {
                sqlParams.Remove(0, 1);
            }
            if (sqlValues.Length > 0)
            {
                sqlValues.Remove(0, 1);
            }

            InsertCondition con = new InsertCondition(CurEntityInfo.DBInfo);
            con.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));
            con.SqlParams.Append(sqlParams.ToString());
            con.SqlValues.Append(sqlValues.ToString());
            int ret = -1;
            con.DbParamList = list;
            string sql = con.GetSql();

            if (identityInfo != null && returnIdentity)
            {
                sql += ";" + CurEntityInfo.DBInfo.CurrentDbAdapter.GetIdentitySQL(CurEntityInfo);
                using (IDataReader reader = oper.Query(sql, list))
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            CurEntityInfo.DBInfo.CurrentDbAdapter.SetObjectValueFromReader(reader, 0, obj, identityInfo);
                            ret = 1;
                        }
                    }
                }
            }
            else
            {
                ret = ExecuteCommand(sql, list, CommandType.Text);
            }
            return ret;
        }

        /// <summary>
        /// �����¼������¼�Զ�������ID
        /// </summary>
        /// <param name="obj">ʵ��</param>
        /// <returns></returns>
        public int IdentityInsert(T obj) 
        {
            int ret = -1;
                
                ret = DoInsert(obj, true);
            return ret;
        }

        /// <summary>
        /// ����һ����¼
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(T obj)
        {
            int ret = -1;
            ret = DoInsert(obj,false);
            return ret;
        }
        /// <summary>
        /// ɾ��ָ������
        /// </summary>
        /// <param name="obj">Ҫɾ����ʵ��</param>
        /// <param name="scopeList">Ҫɾ��������</param>
        /// <param name="isConcurrency">�Ƿ�汾����ɾ��</param>
        /// <returns></returns>
        public int Delete(T obj,ScopeList scopeList, bool isConcurrency)
        {
            DeleteCondition con = new DeleteCondition(CurEntityInfo.DBInfo);
            con.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));
            ParamList list = new ParamList();
            Type type = CurEntityInfo.EntityType;
            con.Condition.Append("1=1");
            
            if(obj!=null)
            {
                if(scopeList==null)//ͨ��IDɾ��
                {
                    scopeList=new ScopeList();
                    scopeList.AddEqual(CurEntityInfo.PrimaryProperty.PropertyName,CurEntityInfo.PrimaryProperty.GetValue(obj));

                }
            }
            con.Condition.Append(DataAccessCommon.FillCondition(CurEntityInfo,list, scopeList));
            
            
            if(isConcurrency)
            {
                int index=0;
                foreach(EntityPropertyInfo pInfo in CurEntityInfo.PropertyInfo)
                {
                    if(pInfo.IsVersion)
                    {
                        FillWhereConcurrency(con.Condition,pInfo,list,pInfo.GetValue(obj),ref index);
                    }
                }
            }
            

            int ret = -1;
            ret = ExecuteCommand(con.GetSql(), list, CommandType.Text);

            return ret;
        }

        /// <summary>
        /// ����IDɾ����¼
        /// </summary>
        /// <param name="id">Ҫɾ���ļ�¼ID</param>
        /// <returns></returns>
        public int DeleteById(object id)
        {
            int ret = -1;

            DeleteCondition con = new DeleteCondition(CurEntityInfo.DBInfo);
            con.Tables.Append(CurEntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(CurEntityInfo.TableName));
            ParamList list = new ParamList();

            ScopeList lstScope = new ScopeList();
            lstScope.AddEqual(CurEntityInfo.PrimaryProperty.PropertyName, id);
            con.Condition.Append("1=1");
            con.Condition.Append(DataAccessCommon.FillCondition(CurEntityInfo,list, lstScope));
            ret = ExecuteCommand(con.GetSql(), list, CommandType.Text);
            return ret;

        }

        #region Select




        /// <summary>
        /// ��ѯ��
        /// </summary>
        /// <param name="scopeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public DataSet Select(ScopeList scopeList)
        {
            if (scopeList.HasInner)
            {
                if (scopeList.OrderBy.Count <= 0)
                {
                    scopeList.OrderBy.Add(CurEntityInfo.PrimaryProperty.PropertyName, SortType.ASC);
                }
                return _cdal.SelectDataSet<T>(scopeList);
            }

            ParamList list = null;

            list = new ParamList();

            string sql = null;
            PageContent objPage = scopeList.PageContent;
            using (BatchAction ba = Oper.StarBatchAction())
            {
                if (objPage == null)//�ж��Ƿ��ҳ��ѯ
                {
                    sql = GetSelectContant(list, scopeList, GetSelectParams(scopeList)).GetSql();
                }
                else
                {
                    sql = GetSelectPageContant(list, scopeList);
                }


                DataSet ds = null;

                ds = oper.QueryDataSet(sql, list, CommandType.Text);

                return ds;
            }
        }

       

        /// <summary>
        /// ��ҳ��ѯ��(����List)
        /// </summary>
        /// <param name="scopeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public List<T> SelectList(ScopeList scopeList)
        {
            if (scopeList.HasPage) 
            {
                if (!scopeList.HasSort) 
                {
                    scopeList.OrderBy.Add(CurEntityInfo.PrimaryProperty.PropertyName, SortType.ASC);
                }
            }

            if (scopeList.HasInner)
            {
                return _cdal.SelectList<T>(scopeList);
            }

            ParamList list = null;

            list = new ParamList();
            string sql = null;
            using (BatchAction ba = Oper.StarBatchAction())
            {
                if (!scopeList.HasPage)//�ж��Ƿ��ҳ��ѯ
                {
                    sql = GetSelectContant(list, scopeList, GetSelectParams(scopeList)).GetSql();
                }
                else
                {

                    sql = GetSelectPageContant(list, scopeList);
                }

                List<T> retlist = null;


                retlist = QueryList(sql, list, CommandType.Text);
                return retlist;
            }
        }
        #endregion
        #region SelectCount
        /// <summary>
        /// ��ѯ����ָ�������ļ�¼����
        /// </summary>
        /// <param name="scopeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public long SelectCount(ScopeList scopeList)
        {
            if (scopeList.HasInner)
            {
                return _cdal.SelectCount<T>(scopeList);
            }
            ParamList list = null;
                list = new ParamList();
            
            string sql = GetSelectContant(list,scopeList, "count(*)").GetSql();
            long count = 0;
            
            //try
            //{
                using (IDataReader reader = oper.Query(sql, list))
                {
                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            count = Convert.ToInt64(reader[0]);
                        }
                    }
                }
            return count;
        }

        #endregion

        #region SelectExists
        /// <summary>
        /// ��ѯ�Ƿ���ڷ��������ļ�¼
        /// </summary>
        /// <param name="scopeList">��Χ���ҵļ���</param>
        /// <returns></returns>
        public bool ExistsRecord(ScopeList scopeList)
        {
            if (scopeList.HasInner)
            {
                return _cdal.ExistsRecord<T>(scopeList);
            }
            ParamList list = null;
            SelectCondition sc = GetSelectContant(list, scopeList, CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParam(CurEntityInfo.PrimaryProperty.ParamName));
            string sql = CurEntityInfo.DBInfo.CurrentDbAdapter.GetTopSelectSql(sc, 1);
            bool exists = false;
            using (IDataReader reader = oper.Query(sql, list))
            {
                if (reader.Read())
                {
                    exists = true;
                }
            }
            return exists;
        }

        #endregion

    }
}
