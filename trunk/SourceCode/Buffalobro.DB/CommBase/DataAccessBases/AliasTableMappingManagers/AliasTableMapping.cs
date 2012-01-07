using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon;
using Buffalo.Kernel;
using Buffalobro.DB.CsqlCommon.CsqlBaseFunction;
using System.Data;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using System.Collections;
using Buffalobro.DB.CsqlCommon;

namespace Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    public class AliasTableMapping
    {
        

        private CsqlAliasHandle _table;

        private Dictionary<string,CsqlAliasParamHandle> _dicParams;

        private Dictionary<string, AliasTableMapping> _dicChildTables = new Dictionary<string, AliasTableMapping>();

        TableAliasNameManager _belongManager;//所属的管理器

        EntityMappingInfo _mappingInfo;//所属的关联

        private EntityInfoHandle _entityInfo;

        private Dictionary<string, EntityPropertyInfo> _dicPropertyInfo = new Dictionary<string, EntityPropertyInfo>();

        private List<AliasReaderMapping> _lstReaderMapping =null;

        private AliasReaderMapping _primaryMapping = null;

        private Dictionary<string, EntityBase> _dicInstance = new Dictionary<string, EntityBase>();//已经实例化的实体

        private IList _baseList;

        /// <summary>
        /// 别名映射
        /// </summary>
        /// <param name="table"></param>
        /// <param name="aliasName"></param>
        public AliasTableMapping(CsqlEntityTableHandle table, TableAliasNameManager belongManager, EntityMappingInfo mappingInfo) 
        {
            _belongManager = belongManager;
            _entityInfo = table.GetEntityInfo();
            _table = new CsqlAliasHandle(table, _belongManager.NextTableAliasName());
            _mappingInfo = mappingInfo;
            InitParam(table);
        }

        /// <summary>
        /// 初始化跟Reader的映射信息
        /// </summary>
        /// <param name="reader"></param>
        public void InitReaderMapping(IDataReader reader) 
        {
            _lstReaderMapping = new List<AliasReaderMapping>(_dicPropertyInfo.Count);
            int fCount=reader.FieldCount;
            EntityPropertyInfo info=null;
            for (int i = 0; i < fCount; i++) 
            {
                string colName = reader.GetName(i);

                if (_dicPropertyInfo.TryGetValue(colName, out info)) 
                {
                    AliasReaderMapping aliasMapping = new AliasReaderMapping(i, info);
                    _lstReaderMapping.Add(aliasMapping);
                    if (info.IsPrimaryKey) 
                    {
                        _primaryMapping = aliasMapping;
                    }
                }
            }

            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)
            {
                keyPair.Value.InitReaderMapping(reader);
            }

            _baseList = new ArrayList();
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public EntityBase LoadFromReader(IDataReader reader, out bool hasValue) 
        {
            
            IDBAdapter dbAdapter = _entityInfo.DBInfo.CurrentDbAdapter;

            EntityBase objRet = null;
            string pk = null;
            hasValue = true;
            if (_primaryMapping != null)//如果有就返回null
            {


                if (reader.IsDBNull(_primaryMapping.ReaderIndex)) 
                {
                    return null;
                }
                object pkValue = reader[_primaryMapping.ReaderIndex];
                pk = pkValue.ToString();
                _dicInstance.TryGetValue(pk, out objRet);
            }

            
            if (objRet == null)
            {
                objRet = Activator.CreateInstance(_entityInfo.EntityType) as EntityBase;
                
                foreach (AliasReaderMapping readMapping in _lstReaderMapping)
                {
                    int index = readMapping.ReaderIndex;
                    EntityPropertyInfo info = readMapping.PropertyInfo;
                    if (!reader.IsDBNull(index) && info != null)
                    {

                        dbAdapter.SetObjectValueFromReader(reader, index, objRet, info);
                    }
                }
                if (!string.IsNullOrEmpty(pk))
                {
                    _dicInstance[pk.ToString()] = objRet;
                }
                objRet._search_baselist_ = _baseList;
                _baseList.Add(objRet);
                hasValue = false;
            }
            

            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)
            {
                AliasTableMapping childMapping = keyPair.Value;
                bool hValue = false;
                object child = childMapping.LoadFromReader(reader, out hValue);
                if (child != null)
                {
                    if (!childMapping.MappingInfo.IsPrimary)//填充父类
                    {
                        childMapping.MappingInfo.SetValue(objRet, child);
                    }
                    else //填充子类
                    {
                        IList lst = childMapping.MappingInfo.GetValue(objRet) as IList;
                        if (lst == null)
                        {
                            lst = Activator.CreateInstance(childMapping.MappingInfo.FieldType) as IList;
                            childMapping.MappingInfo.SetValue(objRet, lst);
                        }
                        
                        lst.Add(child);
                    }
                }
            }
            return objRet;
        }


        

        /// <summary>
        /// 实体信息
        /// </summary>
        /// <returns></returns>
        public EntityInfoHandle EntityInfo 
        {
            get 
            {
                return _entityInfo;
            }
        }

        /// <summary>
        /// 所有字段
        /// </summary>
        public Dictionary<string, AliasTableMapping> ChildTables
        {
            get 
            {

                return _dicChildTables;
            }
        }
        /// <summary>
        /// 映射信息
        /// </summary>
        public EntityMappingInfo MappingInfo 
        {
            get 
            {
                return _mappingInfo;
            }
        }
        /// <summary>
        /// 获取字段别名信息
        /// </summary>
        /// <param name="propertyName">所属的属性名</param>
        /// <returns></returns>
        public List<CsqlParamHandle> GetParamInfo(string propertyName) 
        {
            List<CsqlParamHandle> lstRet = new List<CsqlParamHandle>();

            if (propertyName != "*")
            {
                CsqlAliasParamHandle handle = null;
                if (_dicParams.TryGetValue(propertyName, out handle))
                {
                    lstRet.Add(handle);
                }
            }
            else 
            {
                foreach (KeyValuePair<string, CsqlAliasParamHandle> keyPair in _dicParams) 
                {
                    lstRet.Add(keyPair.Value);
                }
            }
            return lstRet;
        }

        /// <summary>
        /// 表信息
        /// </summary>
        public CsqlAliasHandle TableInfo
        {
            get 
            {
                return _table;
            }
        }

        /// <summary>
        /// 添加子表
        /// </summary>
        /// <param name="table">子表</param>
        public AliasTableMapping AddChildTable(CsqlEntityTableHandle table, List<KeyWordJoinItem> lstJoin) 
        {
            AliasTableMapping retTable = null;
            Stack<CsqlEntityTableHandle> stkTables = new Stack<CsqlEntityTableHandle>();
            CsqlEntityTableHandle curTable=table;
            do
            {
                stkTables.Push(curTable);
                curTable = curTable.GetParentTable();
            } while (!CommonMethods.IsNull(curTable));

            AliasTableMapping lastTable = null;//上一个表
            while (stkTables.Count > 0) 
            {
                CsqlEntityTableHandle cTable=stkTables.Pop();

                string pName = cTable.GetPropertyName();
                if (string.IsNullOrEmpty(pName))
                {
                    if (cTable.GetEntityInfo().EntityType == cTable.GetEntityInfo().EntityType)
                    {
                        lastTable = this;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (!lastTable._dicChildTables.ContainsKey(pName))
                    {
                        
                        EntityMappingInfo mapInfo = _entityInfo.MappingInfo[pName];
                        if (mapInfo != null)
                        {
                            retTable = new AliasTableMapping(cTable, _belongManager, mapInfo);
                            lastTable._dicChildTables[pName] = retTable;
                        }
                        else 
                        {
                            throw new MissingMemberException("实体:" + _entityInfo.EntityType.FullName + "中找不到属性:" + pName + "");
                        }
                    }
                }
            }
            return retTable;
        }

       
        /// <summary>
        /// 获取别名字段
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public CsqlParamHandle GetAliasParam(string propertyName) 
        {
            CsqlAliasParamHandle prm = null;
            CsqlParamHandle ret = null;
            if (_dicParams.TryGetValue(propertyName, out prm))
            {
                ret = CSQL.ToParam(prm.AliasName);
                ret.ValueDbType = prm.ValueDbType;
            }
            return ret;
        }

        /// <summary>
        /// 初始化字段
        /// </summary>
        /// <param name="table"></param>
        /// <param name="paramIndex"></param>
        private void InitParam(CsqlEntityTableHandle table) 
        {
            _dicParams = new Dictionary<string, CsqlAliasParamHandle>();

            foreach (EntityPropertyInfo info in table.GetEntityInfo().PropertyInfo) 
            {
                string prmAliasName=_belongManager.NextParamAliasName(_table.GetAliasName());
                CsqlAliasParamHandle prm = CSQL.Tables[_table.GetAliasName()][info.ParamName].As(prmAliasName);

                
                _dicPropertyInfo[prmAliasName] = info;
                prm.ValueDbType = info.SqlType;
                _dicParams[info.PropertyName]=prm;
            }
        }
        

    }
}
