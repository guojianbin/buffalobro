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
namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// ʵ�����
    /// </summary>
    public class EntityBase
    {
        private EntityInfoHandle _thisInfo = null;//��ǰ�����Ϣ
        internal Dictionary<string, bool> _dicUpdateProperty___ = null;//��¼���޸Ĺ�ֵ������

        /// <summary>
        /// ֪ͨ�����Ѿ����޸�
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyUpdated(string propertyName) 
        {
            if (_dicUpdateProperty___ == null) 
            {
                _dicUpdateProperty___ = new Dictionary<string, bool>();

            }
            EntityInfoHandle entityInfo = GetEntityInfo();
            EntityMappingInfo mapInfo = entityInfo.MappingInfo[propertyName];
            if (mapInfo != null && mapInfo.IsParent) //����Ǹ���ʵ��
            {
                UpdateChildProperty(mapInfo);
            }

            mapInfo = entityInfo.GetMappingBySourceName(propertyName);
            if (mapInfo != null && mapInfo.IsParent) //����Ǹ���ʵ��
            {
                ClearParentProperty(mapInfo);
            }

            _dicUpdateProperty___[propertyName] = true;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="mapInfo"></param>
        private void UpdateChildProperty(EntityMappingInfo mapInfo)
        {

            object parentObject = mapInfo.GetValue(this);
            object pkValue = mapInfo.TargetProperty.GetValue(parentObject);//��ȡID
            mapInfo.SourceProperty.SetValue(this, pkValue);
            _dicUpdateProperty___[mapInfo.TargetProperty.PropertyName] = true;

        }

        /// <summary>
        /// ��ո�����
        /// </summary>
        private void ClearParentProperty(EntityMappingInfo mapInfo) 
        {
            mapInfo.SetValue(this, null);
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
        protected void FillChild(string propertyName)
        {
            MappingContorl.FillChildList(propertyName, this);
        }

        /// <summary>
        /// ��丸��
        /// </summary>
        /// <param name="propertyName"></param>
        protected void FillParent(string propertyName)
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
        public int CopyTo(object target) 
        {
            return ClassInfoManager.ObjectCopy(this, target);
        }
    }
}
