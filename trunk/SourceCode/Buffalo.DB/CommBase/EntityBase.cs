using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Buffalo.DB.ListExtends;
using System.Reflection;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.DB.DataFillers;
using Buffalo.Kernel.Defaults;
using System.Data;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;
namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class EntityBase:ICloneable
    {
        private EntityInfoHandle _thisInfo = null;//当前类的信息
        internal Dictionary<string, bool> _dicUpdateProperty___ = null;//记录被修改过值的属性

        /// <summary>
        /// 修改过的属性名
        /// </summary>
        public List<string> GetChangedPropertyName()
        {
            List<string> pNames = new List<string>(_dicUpdateProperty___.Count);
            foreach (KeyValuePair<string, bool> kvp in _dicUpdateProperty___) 
            {
                pNames.Add(kvp.Key);
            }
            return pNames;
        }

        private IList _baseListInfo;
        /// <summary>
        /// 所属列表集合以便在延迟加载时候读出
        /// </summary>
        internal IList GetBaseList()
        {
             return _baseListInfo;
        }
        /// <summary>
        /// 所属列表集合以便在延迟加载时候读出
        /// </summary>
        internal void SetBaseList(IList lst)
        {
             _baseListInfo=lst;
        }

        /// <summary>
        /// 通知属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal void OnPropertyUpdated(string propertyName) 
        {
            if (_dicUpdateProperty___ == null) 
            {
                _dicUpdateProperty___ = new Dictionary<string, bool>();

            }
            _dicUpdateProperty___[propertyName] = true;
        }

        /// <summary>
        /// 通知映射属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal void OnMapPropertyUpdated(string propertyName) 
        {
            EntityInfoHandle entityInfo = GetEntityInfo();
            UpdatePropertyInfo updateInfo = entityInfo.GetUpdatePropertyInfo(propertyName);
            if (updateInfo != null)
            {
                string updatePropertyName = updateInfo.UpdateProperty(this);
                if (!string.IsNullOrEmpty(updatePropertyName))
                {
                    _dicUpdateProperty___[updatePropertyName] = true;
                }
            }
        }

        
        /// <summary>
        /// 通知属性已经被修改
        /// </summary>
        /// <param name="propertyName"></param>
        protected bool HasPropertyChange(string propertyName)
        {
            if (_dicUpdateProperty___ == null)
            {
                return false;

            }
            return _dicUpdateProperty___.ContainsKey(propertyName);
        }

        /// <summary>
        /// 获取当前实体的信息
        /// </summary>
        /// <returns></returns>
        private EntityInfoHandle GetEntityInfo()
        {
            if (_thisInfo == null)
            {
                _thisInfo = EntityInfoManager.GetEntityHandle(this.GetType());
            }
            return _thisInfo;
        }

        /// <summary>
        /// 根据属性名获取或设置属性的值
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                return GetEntityInfo().PropertyInfo[propertyName].GetValue(this);
            }
            set 
            {
                GetEntityInfo().PropertyInfo[propertyName].SetValue(this,value);
            }
        }

        /// <summary>
        /// 判断是否有此属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public bool HasProperty(string propertyName) 
        {
            return GetEntityInfo().PropertyInfo[propertyName] != null;
        }

        /// <summary>
        /// 是否可以拷贝属性的数值类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private bool IsAllowType(Type type)
        {
            return DefaultType.GetDefaultValue(type) != null;
        }

        /// <summary>
        /// 填充子类
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillChild(string propertyName)
        {
            MappingContorl.FillChildList(propertyName, this);
        }

        /// <summary>
        /// 填充父类
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillParent(string propertyName)
        {
            MappingContorl.FillParent(propertyName, this);
        }

        /// <summary>
        /// 当填充子类时候
        /// </summary>
        protected internal virtual void OnFillChild(string propertyName, ScopeList lstScope) 
        {

        }

        /// <summary>
        /// 当填充子类时候
        /// </summary>
        protected internal virtual void OnFillParent(string propertyName, ScopeList lstScope)
        {

        } 

        /// <summary>
        /// 从DataReader加载
        /// </summary>
        /// <param name="reader"></param>
        public void LoadFromReader(IDataReader reader)
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(this.GetType());
            CacheReader.FillInfoFromReader(reader, entityInfo, this);
        }

        /// <summary>
        /// 把本类的字段拷贝到目标类
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(object target) 
        {
            FieldCloneHelper.CopyTo(this, target);
        }

        

        #region ICloneable 成员

        public object Clone()
        {
            object target = Activator.CreateInstance(this.GetType());
            FieldCloneHelper.CopyTo(this, target);
            return target;
        }

        #endregion
    }
}
