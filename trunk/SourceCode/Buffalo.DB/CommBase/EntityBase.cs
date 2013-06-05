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
    /// ʵ�����
    /// </summary>
    public class EntityBase:ICloneable
    {
        private EntityInfoHandle _thisInfo = null;//��ǰ�����Ϣ
        internal Dictionary<string, bool> _dicUpdateProperty___ = null;//��¼���޸Ĺ�ֵ������

        /// <summary>
        /// �޸Ĺ���������
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
        /// �����б����Ա����ӳټ���ʱ�����
        /// </summary>
        internal IList GetBaseList()
        {
             return _baseListInfo;
        }
        /// <summary>
        /// �����б����Ա����ӳټ���ʱ�����
        /// </summary>
        internal void SetBaseList(IList lst)
        {
             _baseListInfo=lst;
        }

        /// <summary>
        /// ֪ͨ�����Ѿ����޸�
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
        /// ֪ͨӳ�������Ѿ����޸�
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
        /// ֪ͨ�����Ѿ����޸�
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
        /// ��ȡ��ǰʵ�����Ϣ
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
        /// ������������ȡ���������Ե�ֵ
        /// </summary>
        /// <param name="propertyName">������</param>
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
        /// �ж��Ƿ��д�����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public bool HasProperty(string propertyName) 
        {
            return GetEntityInfo().PropertyInfo[propertyName] != null;
        }

        /// <summary>
        /// �Ƿ���Կ������Ե���ֵ����
        /// </summary>
        /// <param name="type">����</param>
        /// <returns></returns>
        private bool IsAllowType(Type type)
        {
            return DefaultType.GetDefaultValue(type) != null;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillChild(string propertyName)
        {
            MappingContorl.FillChildList(propertyName, this);
        }

        /// <summary>
        /// ��丸��
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void FillParent(string propertyName)
        {
            MappingContorl.FillParent(propertyName, this);
        }

        /// <summary>
        /// ���������ʱ��
        /// </summary>
        protected internal virtual void OnFillChild(string propertyName, ScopeList lstScope) 
        {

        }

        /// <summary>
        /// ���������ʱ��
        /// </summary>
        protected internal virtual void OnFillParent(string propertyName, ScopeList lstScope)
        {

        } 

        /// <summary>
        /// ��DataReader����
        /// </summary>
        /// <param name="reader"></param>
        public void LoadFromReader(IDataReader reader)
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(this.GetType());
            CacheReader.FillInfoFromReader(reader, entityInfo, this);
        }

        /// <summary>
        /// �ѱ�����ֶο�����Ŀ����
        /// </summary>
        /// <param name="target"></param>
        public void CopyTo(object target) 
        {
            FieldCloneHelper.CopyTo(this, target);
        }

        

        #region ICloneable ��Ա

        public object Clone()
        {
            object target = Activator.CreateInstance(this.GetType());
            FieldCloneHelper.CopyTo(this, target);
            return target;
        }

        #endregion
    }
}
