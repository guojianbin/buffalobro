using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.Kernel;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLEntityTableHandle:BQLTableHandle
    {
        private EntityInfoHandle _entityInfo;

        private BQLEntityTableHandle _parentTable;//����

        private string _propertyName;//������������

        private string _entityKey;//����ֵ·��

        private Dictionary<string, BQLEntityParamHandle> _dicParam=new Dictionary<string,BQLEntityParamHandle>();

        /// <summary>
        /// ��ȡ������ʵ�����Ϣ
        /// </summary>
        public EntityInfoHandle GetEntityInfo()
        {
            return _entityInfo; 
        }


        /// <summary>
        /// ����
        /// </summary>
        public override List<string> GetPrimaryParam()
        {
            List<string> paramName = new List<string>();
            foreach(EntityPropertyInfo ep in _entityInfo.PrimaryProperty)
            {
                paramName.Add(ep.ParamName);
            }
            return paramName;
        }

        /// <summary>
        /// ����������ʵ�����Ϣ
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <returns></returns>
        protected void SetEntityInfo(EntityInfoHandle entityInfo, BQLEntityTableHandle parentTable, string propertyName)
        {
            _entityInfo=entityInfo;
            _parentTable = parentTable;
            _propertyName = propertyName;
            if (string.IsNullOrEmpty(propertyName))
            {
                _entityKey = entityInfo.EntityType.Name;
            }
            else
            {
                if (!CommonMethods.IsNull(parentTable))
                {
                    StringBuilder sb = new StringBuilder(50);
                    sb.Append(parentTable.GetEntityKey());
                    sb.Append(".");
                    sb.Append(propertyName);
                    _entityKey = sb.ToString();
                }

            }
        }

        /// <summary>
        /// ���������������
        /// </summary>
        internal string GetPropertyName()
        {
            return _propertyName;
        }
        /// <summary>
        /// ����Ĺ�����
        /// </summary>
        internal string GetEntityKey()
        {
            return _entityKey;
        }
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        internal BQLEntityTableHandle GetParentTable()
        {
            return _parentTable; 
        }

        public BQLEntityTableHandle()
        {
        }

        public BQLEntityTableHandle(EntityInfoHandle entityInfo)
            :this(entityInfo,null,null)
        {
        }
        public BQLEntityTableHandle(Type entityType, BQLEntityTableHandle parentTable, string propertyName)
        {
            SetEntityInfo(EntityInfoManager.GetEntityHandle(entityType), parentTable, propertyName);
        }
        public BQLEntityTableHandle(EntityInfoHandle entityInfo, BQLEntityTableHandle parentTable,string propertyName)
        {
            SetEntityInfo(entityInfo, parentTable, propertyName);
        }
        /// <summary>
        /// ����ʵ�������
        /// </summary>
        /// <param name="entityInfo"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected BQLEntityParamHandle CreateProperty(string propertyName) 
        {
            BQLEntityParamHandle prm = new BQLEntityParamHandle(_entityInfo, propertyName, this);
            _dicParam[propertyName] = prm;
            return prm;
        }
        /// <summary>
        /// ��ȡʵ������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public override BQLParamHandle this[string propertyName]
        {
            get
            {
                BQLEntityParamHandle prm = null;
                if (_dicParam.TryGetValue(propertyName, out prm))
                {
                    return prm;
                }
                return base[propertyName, DbType.Object];
            }
        }
        /// <summary>
        /// ��ȡʵ������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public override BQLParamHandle this[string propertyName,DbType type]
        {
            get
            {
                BQLEntityParamHandle prm = null;
                if (_dicParam.TryGetValue(propertyName, out prm))
                {
                    return prm;
                }
                return base[propertyName, type];
            }
        }
        internal override void FillInfo(KeyWordInfomation info)
        {
            if (info.DBInfo.CurrentDbAdapter.IsSaveIdentityParam)
            {
                foreach (EntityPropertyInfo entityProInfo in _entityInfo.PropertyInfo)
                {
                    if (entityProInfo.Identity)
                    {
                        IdentityInfo idnInfo = new IdentityInfo(_entityInfo, entityProInfo);
                        info.IdentityInfos.Add(idnInfo);
                    }
                }
            }

            if (info.AliasManager != null) 
            {
                info.AliasManager.AddChildTable(this);
            }
        }



        /// <summary>
        /// ��ȡ��Ӧ��ʵ������
        /// </summary>
        /// <returns></returns>
        internal override List<ParamInfo> GetParamInfoHandle() 
        {
            List<ParamInfo> lst = new List<ParamInfo>(_entityInfo.PropertyInfo.Count);
            foreach (EntityPropertyInfo pinfo in _entityInfo.PropertyInfo) 
            {
                lst.Add(new ParamInfo(pinfo.PropertyName, pinfo.ParamName,pinfo.FieldType));
            }
            return lst;
        }

        public override BQLParamHandle _
        {
            get
            {
                if (CommonMethods.IsNull(__))
                {
                    __ = new BQLEntityParamHandle(_entityInfo, "*",this);
                }
                return __;
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {

            //FillInfo(info);
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            if (info.Condition.PrimaryKey.Count <= 0)
            {
                foreach (string pkep in GetPrimaryParam())
                {
                    info.Condition.PrimaryKey.Add(idba.FormatTableName(this._entityInfo.TableName) +
                        "." + idba.FormatParam(pkep));
                }
            }


            return idba.FormatTableName(this._entityInfo.TableName);


        }

    }
}
