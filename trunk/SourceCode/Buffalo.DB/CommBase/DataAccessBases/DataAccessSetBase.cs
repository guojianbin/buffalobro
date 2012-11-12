using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase.BusinessBases;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    public class DataAccessSetBase
    {
        public DataAccessSetBase(EntityInfoHandle entityInfo) 
        {
            _entityInfo = entityInfo;
            
        }


        private EntityInfoHandle _entityInfo;

        /// <summary>
        /// ��ǰʵ����Ϣ
        /// </summary>
        public virtual EntityInfoHandle EntityInfo
        {
            get { return _entityInfo; }
        }
        

        protected DataBaseOperate _oper;

        protected BQLDbBase _cdal;//BQL���ݲ���

        /// <summary>
        /// BQL������
        /// </summary>
        protected internal BQLDbBase ContextDAL
        {
            get
            {
                return _cdal;
            }
        }

        /// <summary>
        /// �޸ļ�¼
        /// </summary>
        /// <param name="obj">�޸ĵĶ���</param>
        /// <param name="scopeList">�����б�</param>
        /// <param name="optimisticConcurrency">�Ƿ���в�������</param>
        /// <returns></returns>
        public int Update(EntityBase obj, ScopeList scopeList, bool optimisticConcurrency)
        {
            StringBuilder sql = new StringBuilder(500);
            ParamList list = new ParamList();
            StringBuilder where = new StringBuilder(500);
            //where.Append("1=1");
            Type type = EntityInfo.EntityType;
            List<VersionInfo> lstVersionInfo = null;
            int index = 0;
            ///��ȡ���Ա���
            foreach (EntityPropertyInfo info in EntityInfo.PropertyInfo)
            {


                object curValue = info.GetValue(obj);
                if (!info.ReadOnly)
                {
                    if (optimisticConcurrency == true && info.IsVersion) //��������
                    {

                        object newValue = FillUpdateConcurrency(sql, info, list, curValue, ref index);
                        FillWhereConcurrency(where, info, list, curValue, ref index);
                        if (lstVersionInfo == null)
                        {
                            lstVersionInfo = new List<VersionInfo>();
                        }
                        lstVersionInfo.Add(new VersionInfo(info, curValue, newValue));//�����Ϣ
                    }
                    else
                    {
                        //string paramVal = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                        //string paramKey = CurEntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));
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
                            sql.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                            sql.Append("=");
                            if (curValue != null)
                            {
                                DBParameter dbPrm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);
                                sql.Append(dbPrm.ValueName);
                            }
                            else
                            {
                                sql.Append("null");
                            }
                        }
                    }
                }

                if (info.IsPrimaryKey)
                {
                    if (DefaultType.IsDefaultValue(curValue))
                    {
                        continue;
                    }
                    DBParameter dbPrm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);
                    where.Append(" and ");
                    where.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                    where.Append("=");
                    where.Append(dbPrm.ValueName);
                    //primaryKeyValue=curValue;
                }
                index++;



            }

            where.Append(DataAccessCommon.FillCondition(EntityInfo, list, scopeList));
            if (sql.Length <= 0)
            {
                return 0;
            }
            else
            {
                sql.Remove(0, 1);

            }
            UpdateCondition con = new UpdateCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            con.UpdateSetValue.Append(sql);
            con.Condition.Append("1=1");
            con.Condition.Append(where);

            int ret = -1;
            ret = ExecuteCommand(con.GetSql(), list, CommandType.Text);
            if (obj._dicUpdateProperty___ != null)
            {
                obj._dicUpdateProperty___.Clear();
            }
            if (lstVersionInfo != null && lstVersionInfo.Count > 0)
            {
                foreach (VersionInfo info in lstVersionInfo)
                {
                    info.Info.SetValue(obj, info.NewValue);
                }
            }

            return ret;
        }

        /// <summary>
        /// ���汾���Ƶ���Ϣ
        /// </summary>
        /// <param name="where"></param>
        /// <param name="where"></param>
        /// <param name="info"></param>
        /// <param name="list"></param>
        /// <param name="curValue"></param>
        protected internal void FillWhereConcurrency(StringBuilder where,
            EntityPropertyInfo info, ParamList list, object curValue, ref int index)
        {


            string paramValW = EntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
            string paramKeyW = EntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));
            index++;
            if (DefaultType.IsDefaultValue(curValue))
            {
                throw new Exception("�汾�����ֶ�:" + info.PropertyName + " �����е�ǰ�汾ֵ");
            }
            where.Append(" and ");
            where.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
            where.Append("=");
            where.Append(paramValW);
            list.AddNew(paramKeyW, info.SqlType, curValue);
        }

        #region �汾����

        private object FillUpdateConcurrency(StringBuilder sql,
            EntityPropertyInfo info, ParamList list, object curValue, ref int index)
        {
            object newValue = NewConcurrencyValue(curValue);
            if (newValue != null)
            {
                string paramValV = EntityInfo.DBInfo.CurrentDbAdapter.FormatValueName(DataAccessCommon.FormatParam(info.ParamName, index));
                string paramKeyV = EntityInfo.DBInfo.CurrentDbAdapter.FormatParamKeyName(DataAccessCommon.FormatParam(info.ParamName, index));

                sql.Append(",");
                sql.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
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
        /// �µİ汾ֵ
        /// </summary>
        /// <param name="val">��ǰֵ</param>
        /// <returns></returns>
        private object NewConcurrencyValue(object val)
        {
            Type objType = val.GetType();
            if (DefaultType.EqualType(objType, DefaultType.IntType))
            {
                return (int)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.DoubleType))
            {
                return (double)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.FloatType))
            {
                return (float)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.DateTimeType))
            {
                return DateTime.Now;
            }
            if (DefaultType.EqualType(objType, DefaultType.DecimalType))
            {
                return (decimal)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.ByteType))
            {
                return (byte)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.SbyteType))
            {
                return (sbyte)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.ShortType))
            {
                return (short)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.LongType))
            {
                return (long)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.ULongType))
            {
                return (ulong)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.UShortType))
            {
                return (ushort)val + 1;
            }
            if (DefaultType.EqualType(objType, DefaultType.UIntType))
            {
                return (uint)val + 1;
            }

            return null;
        }

        #endregion



        /// <summary>
        /// ���ݿ����Ӷ���
        /// </summary>
        protected internal DataBaseOperate Oper
        {
            get
            {
                return _oper;
            }
            set
            {
                _oper = value;
                _cdal = new BQLDbBase(_oper);
            }
        }

        /// <summary>
        /// ���в������
        /// </summary>
        /// <param name="obj">Ҫ����Ķ���</param>
        /// <param name="fillIdentity">�Ƿ�Ҫ���ղ����ʵ���ID</param>
        /// <returns></returns>
        protected internal int DoInsert(EntityBase obj, bool fillIdentity)
        {
            StringBuilder sqlParams = new StringBuilder(1000);
            StringBuilder sqlValues = new StringBuilder(1000);
            ParamList list = new ParamList();
            string param = null;
            string svalue = null;
            
            List<EntityPropertyInfo> identityInfo = new List<EntityPropertyInfo>();
            
            foreach (EntityPropertyInfo info in EntityInfo.PropertyInfo)
            {
                //EntityPropertyInfo info = enums.Current.Value;
                object curValue = info.GetValue(obj);
                
                if (info.Identity)
                {
                    if (info.SqlType == DbType.Guid && info.FieldType==DefaultType.GUIDType)
                    {
                        curValue = Guid.NewGuid();
                        info.SetValue(obj, curValue);

                        DBParameter prm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);

                        sqlParams.Append(",");
                        sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sqlValues.Append(",");
                        sqlValues.Append(prm.ValueName);
                        continue;
                    }
                    else
                    {
                        if (fillIdentity)
                        {
                            identityInfo.Add(info);
                        }
                        param = EntityInfo.DBInfo.CurrentDbAdapter.GetIdentityParamName(info);
                        if (!string.IsNullOrEmpty(param))
                        {
                            sqlParams.Append(",");
                            sqlParams.Append(param);
                        }
                        svalue = EntityInfo.DBInfo.CurrentDbAdapter.GetIdentityParamValue(EntityInfo, info);
                        if (!string.IsNullOrEmpty(svalue))
                        {
                            sqlValues.Append(",");
                            sqlValues.Append(svalue);
                        }
                        continue;
                    }


                }
                else if (info.IsVersion) //�汾��ʼֵ
                {
                    object conValue = GetDefaultConcurrency(info);
                    if (conValue != null)
                    {
                        DBParameter prm = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);

                        sqlParams.Append(",");
                        sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                        sqlValues.Append(",");
                        sqlValues.Append(prm.ValueName);
                        
                        continue;
                    }
                }
                else if (curValue == null)
                {
                    continue;
                }
                DBParameter prmValue = list.NewParameter(info.SqlType, curValue, EntityInfo.DBInfo);
                sqlParams.Append(",");
                sqlParams.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatParam(info.ParamName));
                sqlValues.Append(",");
                sqlValues.Append(prmValue.ValueName);
                
            }
            if (sqlParams.Length > 0)
            {
                sqlParams.Remove(0, 1);
            }
            else 
            {
                return 0;
            }
            if (sqlValues.Length > 0)
            {
                sqlValues.Remove(0, 1);
            }
            else
            {
                return 0;
            }

            InsertCondition con = new InsertCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            con.SqlParams.Append(sqlParams.ToString());
            con.SqlValues.Append(sqlValues.ToString());
            int ret = -1;
            con.DbParamList = list;
            using (BatchAction ba = _oper.StarBatchAction())
            {
                string sql = con.GetSql();
                ret = ExecuteCommand(sql, list, CommandType.Text);
                if (identityInfo.Count>0 && fillIdentity)
                {
                    foreach (EntityPropertyInfo pkInfo in identityInfo)
                    {
                        sql = EntityInfo.DBInfo.CurrentDbAdapter.GetIdentitySQL(pkInfo);
                        using (IDataReader reader = _oper.Query(sql, new ParamList()))
                        {
                            if (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                {
                                    EntityInfo.DBInfo.CurrentDbAdapter.SetObjectValueFromReader(reader, 0, obj, pkInfo);
                                    ret = 1;
                                }
                            }
                        }
                    }
                }
            }
            
            return ret;
        }

        /// <summary>
        /// ɾ��ָ������
        /// </summary>
        /// <param name="obj">Ҫɾ����ʵ��</param>
        /// <param name="scopeList">Ҫɾ��������</param>
        /// <param name="isConcurrency">�Ƿ�汾����ɾ��</param>
        /// <returns></returns>
        public int Delete(EntityBase obj, ScopeList scopeList, bool isConcurrency)
        {
            DeleteCondition con = new DeleteCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            ParamList list = new ParamList();
            Type type = EntityInfo.EntityType;
            con.Condition.Append("1=1");

            if (obj != null)
            {
                if (scopeList == null)//ͨ��IDɾ��
                {
                    scopeList = new ScopeList();
                    foreach (EntityPropertyInfo info in EntityInfo.PrimaryProperty)
                    {
                        scopeList.AddEqual(info.PropertyName, info.GetValue(obj));
                    }
                }
            }
            con.Condition.Append(DataAccessCommon.FillCondition(EntityInfo, list, scopeList));


            if (isConcurrency)
            {
                int index = 0;
                foreach (EntityPropertyInfo pInfo in EntityInfo.PropertyInfo)
                {
                    if (pInfo.IsVersion)
                    {
                        FillWhereConcurrency(con.Condition, pInfo, list, pInfo.GetValue(obj), ref index);
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

            DeleteCondition con = new DeleteCondition(EntityInfo.DBInfo);
            con.Tables.Append(EntityInfo.DBInfo.CurrentDbAdapter.FormatTableName(EntityInfo.TableName));
            ParamList list = new ParamList();

            ScopeList lstScope = new ScopeList();
            PrimaryKeyInfo pkInfo = id as PrimaryKeyInfo;
            if (pkInfo == null)
            {
                lstScope.AddEqual(EntityInfo.PrimaryProperty[0].PropertyName, id);
            }
            else
            {

                pkInfo.FillScope(EntityInfo.PrimaryProperty, lstScope, true);

            }
            con.Condition.Append("1=1");
            con.Condition.Append(DataAccessCommon.FillCondition(EntityInfo, list, lstScope));
            ret = ExecuteCommand(con.GetSql(), list, CommandType.Text);
            return ret;

        }
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
            ret = _oper.Execute(sql, list, commandType);
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
            DataSet ds = null;
            ds = _oper.QueryDataSet(sql, list, commandType);
            return ds;
        }

 
        /// <summary>
        /// ִ��sql��䣬��ҳ����DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳ����</param>
        public DataSet QueryDataSet(string sql, PageContent objPage)
        {
            DataSet ds = new DataSet();
            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, objPage, _oper, null);
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

            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, objPage, _oper, EntityInfo.EntityType);
            ds.Tables.Add(retDt);
            return ds;
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
            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, lstParam, objPage, _oper, null);
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
            DataTable retDt = EntityInfo.DBInfo.CurrentDbAdapter.QueryDataTable(sql, lstParam, objPage, _oper, EntityInfo.EntityType);
            ds.Tables.Add(retDt);
            return ds;
        }
        #endregion

    }
}
