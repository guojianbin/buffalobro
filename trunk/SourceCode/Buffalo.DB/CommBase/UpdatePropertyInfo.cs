using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase
{
    /// <summary>
    /// ���ԵĹ���������Ϣ
    /// </summary>
    public class UpdatePropertyInfo
    {
        public UpdatePropertyInfo() { }

        /// <summary>
        /// ���ԵĹ���������Ϣ
        /// </summary>
        /// <param name="mapInfo"></param>
        /// <param name="isEntityProperty"></param>
        public UpdatePropertyInfo(EntityMappingInfo mapInfo, bool isEntityProperty) 
        {
            _mapInfo = mapInfo;
            _isEntityProperty = isEntityProperty;
        }


        private bool _isEntityProperty;

        /// <summary>
        /// �Ƿ�ʵ������
        /// </summary>
        public bool IsEntityProperty
        {
            get { return _isEntityProperty; }
            set { _isEntityProperty = value; }
        }

        

        private EntityMappingInfo _mapInfo;
        /// <summary>
        /// ������ӳ��
        /// </summary>
        public EntityMappingInfo MapInfo
        {
            get { return _mapInfo; }
            set { _mapInfo = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="entity"></param>
        public string UpdateProperty(EntityBase entity) 
        {
            if (_isEntityProperty) 
            {
                return UpdateChildProperty(entity);
            }
            return ClearParentProperty(entity);
        }

        /// <summary>
        /// ��ո�����
        /// </summary>
        private string ClearParentProperty(EntityBase entity)
        {
            _mapInfo.SetValue(entity, null);
            return null;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="mapInfo"></param>
        private string UpdateChildProperty(EntityBase entity)
        {

            object parentObject = _mapInfo.GetValue(entity);
            object pkValue = _mapInfo.TargetProperty.GetValue(parentObject);//��ȡID
            _mapInfo.SourceProperty.SetValue(entity, pkValue);
            return _mapInfo.TargetProperty.PropertyName;

        }
    }
}
