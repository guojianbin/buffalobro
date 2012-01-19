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

        private BQLEntityTableHandle _parentTable;//父表

        private string _propertyName;//关联的属性名

        private string _entityKey;//关联值路径

        private Dictionary<string, BQLEntityParamHandle> _dicParam=new Dictionary<string,BQLEntityParamHandle>();

        /// <summary>
        /// 所属的实体的信息
        /// </summary>
        internal EntityInfoHandle GetEntityInfo()
        {
            return _entityInfo; 
        }

        /// <summary>
        /// 父表关联的属性名
        /// </summary>
        internal string GetPropertyName()
        {
            return _propertyName;
        }
        /// <summary>
        /// 本表的关联键
        /// </summary>
        internal string GetEntityKey()
        {
            return _entityKey;
        }
        /// <summary>
        /// 所属父表信息
        /// </summary>
        internal BQLEntityTableHandle GetParentTable()
        {
            return _parentTable; 
        }

        public BQLEntityTableHandle(EntityInfoHandle entityInfo)
            :this(entityInfo,null,null)
        {
        }
        public BQLEntityTableHandle(EntityInfoHandle entityInfo, BQLEntityTableHandle parentTable,string propertyName)
        {
            this._entityInfo = entityInfo;
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
            //this.valueType = BQLValueType.Table;
        }
        /// <summary>
        /// 创建实体的属性
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
        /// 获取实体属性
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
        /// 获取实体属性
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
        /// 获取对应的实体属性
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
            if (info.Condition.PrimaryKey.Length <= 0)
            {
                info.Condition.PrimaryKey.Append(idba.FormatParam(_entityInfo.PrimaryProperty.ParamName));
            }


            return idba.FormatTableName(this._entityInfo.TableName);


        }

    }
}
