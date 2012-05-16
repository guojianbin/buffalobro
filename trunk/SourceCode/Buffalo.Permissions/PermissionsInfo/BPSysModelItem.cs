using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// 子模块权限
    /// </summary>
    public partial class BPSysModelItem:BPBase
    {
        /// <summary>
        /// 所属模块ID
        /// </summary>
        private int? _sysModelId;

        /// <summary>
        /// 子项名
        /// </summary>
        private string _itemName;

        /// <summary>
        /// 子项注释
        /// </summary>
        private string _itemDescription;

        /// <summary>
        /// 所属模块
        /// </summary>
        private BPSysModel _belongModel;

        /// <summary>
        /// 子项标识
        /// </summary>
        private string _itemIdentify;
        /// <summary>
        /// 项权限
        /// </summary>
        private BPModelItemRight _defaultRight;
        /// <summary>
        /// 所属模块ID
        /// </summary>
        public int? SysModelId
        {
            get
            {
                return _sysModelId;
            }
            set
            {
                _sysModelId=value;
                OnPropertyUpdated("SysModelId");
            }
        }
        /// <summary>
        /// 子项名
        /// </summary>
        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                _itemName=value;
                OnPropertyUpdated("ItemName");
            }
        }
        /// <summary>
        /// 子项注释
        /// </summary>
        public string ItemDescription
        {
            get
            {
                return _itemDescription;
            }
            set
            {
                _itemDescription=value;
                OnPropertyUpdated("ItemDescription");
            }
        }
        private static ModelContext<BPSysModelItem> _____baseContext=new ModelContext<BPSysModelItem>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPSysModelItem> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// 所属模块
        /// </summary>
        public BPSysModel BelongModel
        {
            get
            {
               if (_belongModel == null)
               {
                   FillParent("BelongModel");
               }
                return _belongModel;
            }
            set
            {
                _belongModel = value;
                OnPropertyUpdated("BelongModel");
            }
        }
        /// <summary>
        /// 子项标识
        /// </summary>
        public string ItemIdentify
        {
            get
            {
                return _itemIdentify;
            }
            set
            {
                _itemIdentify=value;
                OnPropertyUpdated("ItemIdentify");
            }
        }
        /// <summary>
        /// 项权限
        /// </summary>
        public BPModelItemRight DefaultRight
        {
            get
            {
                return _defaultRight;
            }
            set
            {
                _defaultRight=value;
                OnPropertyUpdated("DefaultRight");
            }
        }
    }
}
