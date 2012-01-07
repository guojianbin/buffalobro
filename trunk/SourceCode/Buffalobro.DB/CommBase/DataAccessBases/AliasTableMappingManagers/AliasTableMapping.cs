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

        TableAliasNameManager _belongManager;//�����Ĺ�����

        EntityMappingInfo _mappingInfo;//�����Ĺ���

        private EntityInfoHandle _entityInfo;

        private Dictionary<string, EntityPropertyInfo> _dicPropertyInfo = new Dictionary<string, EntityPropertyInfo>();

        private List<AliasReaderMapping> _lstReaderMapping =null;

        private AliasReaderMapping _primaryMapping = null;

        private Dictionary<string, EntityBase> _dicInstance = new Dictionary<string, EntityBase>();//�Ѿ�ʵ������ʵ��

        private IList _baseList;

        /// <summary>
        /// ����ӳ��
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
        /// ��ʼ����Reader��ӳ����Ϣ
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
        /// ��ȡ��Ϣ
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public EntityBase LoadFromReader(IDataReader reader, out bool hasValue) 
        {
            
            IDBAdapter dbAdapter = _entityInfo.DBInfo.CurrentDbAdapter;

            EntityBase objRet = null;
            string pk = null;
            hasValue = true;
            if (_primaryMapping != null)//����оͷ���null
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
                    if (!childMapping.MappingInfo.IsPrimary)//��丸��
                    {
                        childMapping.MappingInfo.SetValue(objRet, child);
                    }
                    else //�������
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
        /// ʵ����Ϣ
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
        /// �����ֶ�
        /// </summary>
        public Dictionary<string, AliasTableMapping> ChildTables
        {
            get 
            {

                return _dicChildTables;
            }
        }
        /// <summary>
        /// ӳ����Ϣ
        /// </summary>
        public EntityMappingInfo MappingInfo 
        {
            get 
            {
                return _mappingInfo;
            }
        }
        /// <summary>
        /// ��ȡ�ֶα�����Ϣ
        /// </summary>
        /// <param name="propertyName">������������</param>
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
        /// ����Ϣ
        /// </summary>
        public CsqlAliasHandle TableInfo
        {
            get 
            {
                return _table;
            }
        }

        /// <summary>
        /// ����ӱ�
        /// </summary>
        /// <param name="table">�ӱ�</param>
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

            AliasTableMapping lastTable = null;//��һ����
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
                            throw new MissingMemberException("ʵ��:" + _entityInfo.EntityType.FullName + "���Ҳ�������:" + pName + "");
                        }
                    }
                }
            }
            return retTable;
        }

       
        /// <summary>
        /// ��ȡ�����ֶ�
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
        /// ��ʼ���ֶ�
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
