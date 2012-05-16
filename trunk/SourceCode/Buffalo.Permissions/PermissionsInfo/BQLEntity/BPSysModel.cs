using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.Permissions.PermissionsInfo.BQLEntity
{

    public partial class PermissionDB
    {
        private static PermissionDB_BPSysModel _BPSysModel = new PermissionDB_BPSysModel();
    
        public static PermissionDB_BPSysModel BPSysModel
        {
            get
            {
                return _BPSysModel;
            }
        }
    }

    /// <summary>
    ///  ϵͳģ��
    /// </summary>
    public partial class PermissionDB_BPSysModel : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _modelIdentify = null;
        /// <summary>
        /// ģ���ʶ
        /// </summary>
        public BQLEntityParamHandle ModelIdentify
        {
            get
            {
                return _modelIdentify;
            }
         }
        private BQLEntityParamHandle _modelName = null;
        /// <summary>
        /// ģ������
        /// </summary>
        public BQLEntityParamHandle ModelName
        {
            get
            {
                return _modelName;
            }
         }
        private BQLEntityParamHandle _modelDescription = null;
        /// <summary>
        /// ģ��ע��
        /// </summary>
        public BQLEntityParamHandle ModelDescription
        {
            get
            {
                return _modelDescription;
            }
         }
        private BQLEntityParamHandle _modelType = null;
        /// <summary>
        /// ģ�����
        /// </summary>
        public BQLEntityParamHandle ModelType
        {
            get
            {
                return _modelType;
            }
         }
        private BQLEntityParamHandle _defaultModelRight = null;
        /// <summary>
        /// Ĭ��Ȩ��
        /// </summary>
        public BQLEntityParamHandle DefaultModelRight
        {
            get
            {
                return _defaultModelRight;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public PermissionDB_BPSysModel(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPSysModel),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public PermissionDB_BPSysModel(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _modelIdentify=CreateProperty("ModelIdentify");
            _modelName=CreateProperty("ModelName");
            _modelDescription=CreateProperty("ModelDescription");
            _modelType=CreateProperty("ModelType");
            _defaultModelRight=CreateProperty("DefaultModelRight");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public PermissionDB_BPSysModel() 
            :this(null,null)
        {
        }
    }
}
