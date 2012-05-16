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
    ///  系统模块
    /// </summary>
    public partial class PermissionDB_BPSysModel : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _modelIdentify = null;
        /// <summary>
        /// 模块标识
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
        /// 模块名称
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
        /// 模块注释
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
        /// 模块分类
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
        /// 默认权限
        /// </summary>
        public BQLEntityParamHandle DefaultModelRight
        {
            get
            {
                return _defaultModelRight;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPSysModel(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPSysModel),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
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
        /// 初始化本类的信息
        /// </summary>
        public PermissionDB_BPSysModel() 
            :this(null,null)
        {
        }
    }
}
