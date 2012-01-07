using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlBaseFunction;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.QueryConditions;
using System.Data;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.CsqlCommon.CsqlConditions;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.CommBase;
using Buffalobro.DB.DataBaseAdapter;
using Buffalo.Kernel;
using Buffalobro.DB.CommBase.BusinessBases;

namespace Buffalobro.DB.CsqlCommon
{
    /// <summary>
    /// ���ݲ�������
    /// </summary>
    public class CsqlDbBase
    {
        private DataBaseOperate oper;

        /// <summary>
        /// ���ݲ����
        /// </summary>
        ///  <param name="info">���ݿ���Ϣ</param>
        public CsqlDbBase(DBInfo info)
        {
            this.oper = StaticConnection.GetStaticOperate(info);
        }

        /// <summary>
        /// ���ݲ����
        /// </summary>
        /// <param name="entityType">����ʵ��</param>
        public CsqlDbBase(Type entityType) 
            :this(EntityInfoManager.GetEntityHandle(entityType).DBInfo)
        {

        }

        /// <summary>
        /// ���ݲ����
        /// </summary>
        /// <param name="oper"></param>
        public CsqlDbBase(DataBaseOperate oper) 
        {
            this.oper = oper;
            
        }


        /// <summary>
        /// ֱ�Ӳ�ѯ���ݿ���ͼ
        /// </summary>
        /// <param name="tableName">������</param>
        /// <param name="lstScope">����</param>
        /// <param name="vParams">�ֶ��б�</param>
        /// <returns></returns>
        public virtual DataSet SelectTable(string tableName, ScopeList lstScope)
        {
            return SelectTable(CSQL.ToTable(tableName), lstScope);
        }


        /// <summary>
        /// ��ѯ������
        /// </summary>
        /// <param name="lstScope"></param>
        /// <returns></returns>
        public virtual long SelectCount<E>(ScopeList lstScope) 
        {
            long ret = 0;
            Type eType = typeof(E);
            TableAliasNameManager aliasManager = new TableAliasNameManager(new CsqlEntityTableHandle(EntityInfoManager.GetEntityHandle(typeof(E))));
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                throw new Exception("�Ҳ����˱�");
            }

            CsqlCondition where = CsqlCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            CsqlQuery csql = CSQL.Select(CSQL.Count())
           .From(table)
           .Where(where);

            //if(lstScope.GroupBy

            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, aliasManager,true);
            
            using (IDataReader reader = oper.Query(con.GetSql(), con.DbParamList)) 
            {
                if (reader.Read()) 
                {
                    ret = Convert.ToInt64(reader[0]);
                }
            }
            return ret;
        }

        /// <summary>
        /// ��ѯ��
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public List<E> SelectList<E>(ScopeList lstScope)
            where E : EntityBase, new()
        {
            Type eType = typeof(E);
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table)) 
            {
                throw new Exception("�Ҳ����˱�");
            }
           // List<CsqlParamHandle> lstParams = GetParam(table, lstScope);
           // CsqlCondition where = CsqlCondition.TrueValue;
           // where = FillCondition(where, table, lstScope);
           // CsqlQuery csql = CSQL.Select(lstParams.ToArray())
           //.From(table)
           //.Where(where)
           //.OrderBy(GetSort(lstScope.OrderBy, table));
            CsqlQuery csql = GetSelectSql(lstScope, table);
            if (!lstScope.HasPage)
            {
                return QueryList<E>(csql,lstScope.ShowEntity);
            }
            using (BatchAction ba = oper.StarBatchAction())
            {
                return QueryPageList<E>(csql, lstScope.PageContent, lstScope.ShowEntity);
            }
        }

        /// <summary>
        /// ��ȡҪ��ʾ���ֶ�
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="propertys"></param>
        /// <returns></returns>
        private List<CsqlParamHandle> GetParam(CsqlTableHandle handle, ScopeList lstScope) 
        {
            List<CsqlParamHandle> lstParams = lstScope.GetShowProperty(handle);

            
            return lstParams;
        }

        /// <summary>
        /// ֱ�Ӳ�ѯ���ݿ���ͼ
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="lstScope">����</param>
        /// <param name="vParams">�ֶ��б�</param>
        /// <param name="lstSort">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <returns></returns>
        public DataSet SelectTable(CsqlOtherTableHandle table, ScopeList lstScope)
        {
            List<CsqlParamHandle> lstParams = GetParam(table, lstScope);

            List<CsqlParamHandle> lstOrders = new List<CsqlParamHandle>();
            CsqlParamHandle order = null;
            foreach (Sort objSort in lstScope.OrderBy)
            {
                order = table[objSort.PropertyName];
                if (objSort.SortType == SortType.ASC)
                {
                    order = order.ASC;
                }
                else
                {
                    order = order.DESC;
                }
                lstOrders.Add(order);
            }

            CsqlCondition where = CsqlCondition.TrueValue;
            where = FillCondition(where, table, lstScope, null);

            CsqlQuery csql = CSQL.Select(lstParams.ToArray()).From(table).Where(where).OrderBy(lstOrders.ToArray());

            if (lstScope.HasPage)
            {
                using (BatchAction ba = oper.StarBatchAction())
                {
                    return QueryDataSet(csql, lstScope.PageContent);
                }
            }
            return QueryDataSet(csql);
        }
        /// <summary>
        /// ת��������Ϣ
        /// </summary>
        /// <param name="csql"></param>
        /// <param name="db"></param>
        /// <param name="aliasManager"></param>
        /// <returns></returns>
        private AbsCondition ToCondition(CsqlQuery csql, IEnumerable<CsqlEntityTableHandle> outPutTables, bool isPutPropertyName, Type entityType)
        {
            TableAliasNameManager aliasManager = new TableAliasNameManager(new CsqlEntityTableHandle(EntityInfoManager.GetEntityHandle(entityType)));
            if (outPutTables != null)
            {
                FillOutPutTables(outPutTables, aliasManager);
            }
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, aliasManager, isPutPropertyName);
            return con;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����List
        /// </summary>
        /// <typeparam name="E">ʵ������</typeparam>
        /// <param name="csql">Csql</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="outPutTables">�����</param>
        /// <returns></returns>
        public List<E> QueryPageList<E>(CsqlQuery csql, PageContent objPage,
            IEnumerable<CsqlEntityTableHandle> outPutTables)
            where E : EntityBase, new()
        {
            AbsCondition con = ToCondition(csql, outPutTables,false,typeof(E));
            con.PageContent = objPage;
            
            List<E> retlist = null;
            IDataReader reader = null;
            try
            {


                if (con.DbParamList != null)
                {
                    con.PageContent = objPage;
                    con.Oper = oper;
                    string sql = con.GetSql();
                    reader = oper.Query(sql, con.DbParamList);
                    
                }
                else
                {
                    SelectCondition sCon = con as SelectCondition;
                    reader = con.DBinfo.CurrentDbAdapter.Query(sCon.GetSelect(), objPage, oper);
                }
                retlist = LoadFromReader<E>(con.AliasManager, reader);
            }
            finally
            {
                reader.Close();
            }
            return retlist;
        }
        /// <summary>
        /// ִ��sql��䣬����List
        /// </summary>
        /// <typeparam name="E">ʵ������</typeparam>
        /// <param name="csql">Csql</param>
        /// <returns></returns>
        public List<E> QueryList<E>(CsqlQuery csql, IEnumerable<CsqlEntityTableHandle> outPutTables)
            where E : EntityBase, new()
        {
            AbsCondition con = ToCondition(csql, outPutTables, false, typeof(E));
            List<E> retlist = null;
            using (BatchAction ba = oper.StarBatchAction())
            {
                IDataReader reader = null;
                try
                {
                    con.Oper = oper;

                    if (con.DbParamList != null)
                    {
                        reader = oper.Query(con.GetSql(), con.DbParamList);
                    }
                    else
                    {
                        SelectCondition sCon = con as SelectCondition;
                        reader = con.DBinfo.CurrentDbAdapter.Query(sCon.GetSelect(), con.PageContent, oper);
                    }
                    retlist = LoadFromReader<E>(con.AliasManager, reader);
                }

                finally
                {
                    reader.Close();

                }
            }
            return retlist;
        }
        /// <summary>
        /// ���Ҫ����ı�
        /// </summary>
        /// <param name="outPutTables"></param>
        /// <param name="aliasManager"></param>
        private void FillOutPutTables(IEnumerable<CsqlEntityTableHandle> outPutTables, TableAliasNameManager aliasManager)
        {
            if (outPutTables == null) 
            {
                return;
            }
            foreach (CsqlEntityTableHandle table in outPutTables)
            {
                aliasManager.AddChildTable(table);
            }
        }

        private List<E> LoadFromReader<E>(TableAliasNameManager aliasManager, IDataReader reader)
        {
            List<E> lst = new List<E>();
            if (reader != null && !reader.IsClosed)
            {
                bool hasValue = false;
                while (reader.Read())
                {
                    object value = aliasManager.LoadFromReader(reader, out hasValue);
                    if (!hasValue && value != null)
                    {
                        lst.Add((E)value);
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// ��ȡ��Χ���Ӧ��CSQL
        /// </summary>
        /// <param name="lstScope"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        private CsqlQuery GetSelectSql(ScopeList lstScope, CsqlEntityTableHandle table) 
        {

            List<CsqlParamHandle> lstParams = GetParam(table, lstScope);
            CsqlCondition where = CsqlCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            CsqlQuery csql = CSQL.Select(lstParams.ToArray())
           .From(table)
           .Where(where);

            if (lstScope.GroupBy.Count > 0)
            {
                csql = new KeyWordGroupByItem(lstScope.GroupBy, csql);
            }
            if (lstScope.OrderBy != null && lstScope.OrderBy.Count > 0)
            {
                csql = new KeyWordOrderByItem(GetSort(lstScope.OrderBy, table), csql);
            }
            
            return csql;
        }

        /// <summary>
        /// ��ѯ������DataSet
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="lstScope">��������</param>
        /// <returns></returns>
        public DataSet SelectDataSet<E>(ScopeList lstScope) 
        {
            Type eType = typeof(E);
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                throw new Exception("�Ҳ����˱�");
            }
           // List<CsqlParamHandle> lstParams = GetParam(table, lstScope);
           // CsqlCondition where = CsqlCondition.TrueValue;
           // where = FillCondition(where, table, lstScope);
           // CsqlQuery csql = CSQL.Select(lstParams.ToArray())
           //.From(table)
           //.Where(where);

           // if (lstScope.GroupBy.Count > 0) 
           // {
           //     csql = new KeyWordGroupByItem(lstScope.GroupBy, csql);
           // }
           // if (lstScope.OrderBy != null && lstScope.OrderBy.Count > 0) 
           // {
           //     csql = new KeyWordOrderByItem(GetSort(lstScope.OrderBy, table), csql);
           // }
            CsqlQuery csql = GetSelectSql(lstScope, table);
           //.OrderBy(GetSort(lstScope.OrderBy, table));
            if (!lstScope.HasPage)
            {
                return QueryDataSet<E>(csql);
            }
            using (BatchAction ba = oper.StarBatchAction())
            {
                return QueryDataSet<E>(csql, lstScope.PageContent);
            }
        }


        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet
        /// </summary>
        /// <param name="csql">sql���</param>
        public DataSet QueryDataSet(CsqlQuery csql)
        {
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo,null,true);
            DataSet ds = null;

            con.Oper = oper;
            if (con.DbParamList != null)
            {
                ds = oper.QueryDataSet(con.GetSql(), con.DbParamList);
            }
            else
            {
                SelectCondition sCon = con as SelectCondition;
                DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), sCon.PageContent, oper, null);
                dt.TableName = "newTable";
                ds = new DataSet();
                ds.Tables.Add(dt);
            }

            return ds;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet
        /// </summary>
        /// <param name="csql">sql���</param>
        public DataSet QueryDataSet<E>(CsqlQuery csql)
        {
            AbsCondition con = ToCondition(csql, new CsqlEntityTableHandle[] { }, true, typeof(E));
            DataSet ds = null;

            con.Oper = oper;
            if (con.DbParamList != null)
            {
                ds = oper.QueryDataSet(con.GetSql(), con.DbParamList);
            }
            else
            {
                SelectCondition sCon = con as SelectCondition;
                DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), sCon.PageContent, oper, null);
                dt.TableName = "newTable";
                ds = new DataSet();
                ds.Tables.Add(dt);
            }

            return ds;
        }
        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet
        /// </summary>
        /// <param name="csql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public DataSet QueryDataSet<E>(CsqlQuery csql, PageContent objPage)
        {

            AbsCondition con = ToCondition(csql, new CsqlEntityTableHandle[] { }, true, typeof(E));
            DataSet ds = null;
            using (BatchAction ba = oper.StarBatchAction())
            {
                if (con.DbParamList != null)
                {
                    con.PageContent = objPage;
                    con.Oper = oper;
                    string sql = con.GetSql();
                    ds = oper.QueryDataSet(sql, con.DbParamList);
                }
                else
                {
                    SelectCondition sCon = con as SelectCondition;
                    DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), objPage, oper, null);
                    dt.TableName = "newTable";
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet
        /// </summary>
        /// <param name="csql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public DataSet QueryDataSet(CsqlQuery csql, PageContent objPage)
        {
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo,null,true);
            DataSet ds = null;
            using (BatchAction ba = oper.StarBatchAction())
            {
                if (con.DbParamList != null)
                {
                    con.PageContent = objPage;
                    con.Oper = oper;
                    string sql = con.GetSql();
                    ds = oper.QueryDataSet(sql, con.DbParamList);
                }
                else
                {
                    SelectCondition sCon = con as SelectCondition;
                    DataTable dt = con.DBinfo.CurrentDbAdapter.QueryDataTable(sCon.GetSelect(), objPage, oper, null);
                    dt.TableName = "newTable";
                    ds = new DataSet();
                    ds.Tables.Add(dt);
                }
            }
            return ds;
        }

        /// <summary>
        /// ִ��sql��䣬��ҳ����Reader
        /// </summary>
        /// <param name="csql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="tableType">���Ӧ��ʵ������</param>
        public IDataReader QueryReader(ScopeList lstScope, PageContent objPage,Type tableType)
        {
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(tableType);
            if (CommonMethods.IsNull(table))
            {
                throw new Exception("�Ҳ����˱�");
            }
            
            CsqlQuery csql = GetSelectSql(lstScope, table);
            return QueryReader(csql, objPage);
        }

        /// <summary>
        /// ִ��sql��䣬��ҳ����Reader
        /// </summary>
        /// <param name="csql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public IDataReader QueryReader(CsqlQuery csql, PageContent objPage)
        {
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, null,true);
            con.PageContent = objPage;
            IDataReader reader = null;

            con.PageContent = objPage;
            con.Oper = oper;
            string sql = con.GetSql();
            reader = oper.Query(sql, con.DbParamList);

            return reader;
        }
        /// <summary>
        /// ִ��sql��䣬����Reader
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public IDataReader QueryReader(ScopeList lstScope, Type tableType)
        {
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(tableType);
            if (CommonMethods.IsNull(table))
            {
                throw new Exception("�Ҳ����˱�");
            }
            
            CsqlQuery csql = GetSelectSql(lstScope, table);
            return QueryReader(csql);
        }

        /// <summary>
        /// ִ��sql��䣬����Reader
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public IDataReader QueryReader(CsqlQuery csql)
        {
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, null,true);
            IDataReader reader = null;
            con.Oper = oper;
            reader = oper.Query(con.GetSql(), con.DbParamList);

            return reader;
        }


        /// <summary>
        /// ִ��Sql����
        /// </summary>
        /// <param name="csql">sql���</param>
        public int ExecuteCommand(CsqlQuery csql)
        {
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, null, true);
            int ret = -1;
            con.Oper = oper;
            ret = oper.Execute(con.GetSql(), con.DbParamList);
            return ret;
        }

        /// <summary>
        /// ��ѯ�Ƿ���ڷ��������ļ�¼
        /// </summary>
        /// <param name="csql">sql���</param>
        /// <returns></returns>
        public bool ExistsRecord<E>(ScopeList lstScope)
            where E : EntityBase, new() 
        {
            Type eType = typeof(E);
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                throw new Exception("�Ҳ����˱�");
            }
            List<CsqlParamHandle> lstParams = new List<CsqlParamHandle>();
            lstParams.Add(table[table.GetEntityInfo().PrimaryProperty.PropertyName]);
                
            CsqlCondition where = CsqlCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            CsqlQuery csql = CSQL.Select(lstParams.ToArray())
           .From(table)
           .Where(where)
           .OrderBy(GetSort(lstScope.OrderBy, table));
            return ExistsRecord<E>(csql);
        }

        /// <summary>
        /// ��ѯ�Ƿ���ڷ��������ļ�¼
        /// </summary>
        /// <param name="csql">sql���</param>
        /// <returns></returns>
        public bool ExistsRecord<E>(CsqlQuery csql)
            where E : EntityBase, new()
        {
            TableAliasNameManager aliasManager = new TableAliasNameManager(new CsqlEntityTableHandle(EntityInfoManager.GetEntityHandle(typeof(E))));
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, aliasManager, true);
            string sql = con.DBinfo.CurrentDbAdapter.GetTopSelectSql(con as SelectCondition, 1);
            bool exists = false;
            IDataReader reader = null;
            try
            {
                con.Oper = oper;
                reader = oper.Query(sql, con.DbParamList);
                exists = reader.Read();
            }
            finally
            {
                reader.Close();
            }
            return exists;
        }


        /// <summary>
        /// ��ѯ��
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="lstScope">����</param>
        /// <returns></returns>
        public E GetUnique<E>(ScopeList lstScope)
            where E : EntityBase, new()
        {
            Type eType = typeof(E);
            CsqlEntityTableHandle table = oper.DBInfo.FindTable(eType);
            if (CommonMethods.IsNull(table))
            {
                throw new Exception("�Ҳ����˱�");
            }
            List<CsqlParamHandle> lstParams = GetParam(table, lstScope);
            CsqlCondition where = CsqlCondition.TrueValue;
            where = FillCondition(where, table, lstScope);
            CsqlQuery csql = CSQL.Select(lstParams.ToArray())
           .From(table)
           .Where(where)
           .OrderBy(GetSort(lstScope.OrderBy, table));
            return GetUnique<E>(csql);
        }

        /// <summary>
        /// ��ȡ��һ����¼
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="csql"></param>
        /// <returns></returns>
        public E GetUnique<E>(CsqlQuery csql)
            where E : EntityBase, new()
        {
            
            TableAliasNameManager aliasManager = new TableAliasNameManager(new CsqlEntityTableHandle(EntityInfoManager.GetEntityHandle(typeof(E))));
            AbsCondition con = CsqlKeyWordManager.ToCondition(csql, oper.DBInfo, aliasManager,false);
            string sql = con.DBinfo.CurrentDbAdapter.GetTopSelectSql(con as SelectCondition, 1);
            E ret = default(E);
            IDataReader reader = oper.Query(sql, con.DbParamList);
            try
            {
                con.Oper = oper;
                bool hasValue=true;
                if (reader.Read())
                {
                    ret = aliasManager.LoadFromReader(reader, out hasValue) as E;
                }
            }
            finally
            {
                reader.Close();
            }
            return ret;
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="table"></param>
        /// <param name="lstScope"></param>
        /// <param name="entityType"></param>
        public CsqlCondition FillCondition(CsqlCondition condition, CsqlTableHandle table, ScopeList lstScope, Type entityType)
        {
            CsqlCondition ret;
            EntityInfoHandle entityInfo = null;
            if (entityType != null)
            {
                entityInfo = EntityInfoManager.GetEntityHandle(entityType);
            }
            ret = CsqlConditionScope.FillCondition(condition, table, lstScope, entityInfo);
            return ret;
        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="table"></param>
        /// <param name="lstScope"></param>
        /// <param name="entityType"></param>
        public CsqlCondition FillCondition(CsqlCondition condition, CsqlEntityTableHandle table, ScopeList lstScope)
        {
            return CsqlConditionScope.FillCondition(condition, table, lstScope, table.GetEntityInfo());
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="lstScort"></param>
        /// <param name="table"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        protected CsqlParamHandle[] GetSort(SortList lstScort, CsqlTableHandle table, Type entityType)
        {
            EntityInfoHandle entityInfo = null;
            if (entityType != null)
            {
                entityInfo = EntityInfoManager.GetEntityHandle(entityType);
            }
            return CsqlConditionScope.GetSort(lstScort, table, entityInfo);
        }
        /// <summary>
        /// ת��������Ϣ
        /// </summary>
        /// <param name="lstScort"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        protected CsqlParamHandle[] GetSort(SortList lstScort, CsqlEntityTableHandle table)
        {
            return CsqlConditionScope.GetSort(lstScort, table, table.GetEntityInfo());
        }

        /// <summary>
        /// ���GroupBy
        /// </summary>
        /// <param name="csql"></param>
        /// <param name="groupBy"></param>
        /// <returns></returns>
        protected static CsqlQuery FillGroupBy(CsqlQuery csql, ScopePropertyCollection groupBy)
        {
            if (groupBy == null || groupBy.Count > 0) 
            {
                return csql;
            }

            return new KeyWordGroupByItem(groupBy.ToArray(), csql);
            
        }

    }
}
