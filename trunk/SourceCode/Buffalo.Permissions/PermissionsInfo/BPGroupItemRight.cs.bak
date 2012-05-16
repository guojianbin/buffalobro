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
    /// 模块子项权限
    /// </summary>
    public partial class BPGroupItemRight:BPBase
    {
        /// <summary>
        /// 子模块ID
        /// </summary>
        private int? _modelItemId;
        /// <summary>
        /// 组ID
        /// </summary>
        private int? _groupId;

        /// <summary>
        /// 所属的模块权限记录ID
        /// </summary>
        private int? _groupModelRightId;

        /// <summary>
        /// 项权限
        /// </summary>
        private BPModelItemRight _itemRight;

        /// <summary>
        /// 所属子项
        /// </summary>
        private BPSysModelItem _belongItem;
        /// <summary>
        /// 所属分组
        /// </summary>
        private BPGroup _belongGroup;

        /// <summary>
        /// 所属的模块权限记录
        /// </summary>
        private BPGroupModelRight _belongGroupModelRight;
        /// <summary>
        /// 子模块ID
        /// </summary>
        public int? ModelItemId
        {
            get
            {
                return _modelItemId;
            }
            set
            {
                _modelItemId=value;
                OnPropertyUpdated("ModelItemId");
            }
        }
        /// <summary>
        /// 组ID
        /// </summary>
        public int? GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId=value;
                OnPropertyUpdated("GroupId");
            }
        }
        /// <summary>
        /// 项权限
        /// </summary>
        public BPModelItemRight ItemRight
        {
            get
            {
                return _itemRight;
            }
            set
            {
                _itemRight=value;
                OnPropertyUpdated("ItemRight");
            }
        }
        private static ModelContext<BPGroupItemRight> _____baseContext=new ModelContext<BPGroupItemRight>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPGroupItemRight> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// 所属子项
        /// </summary>
        public BPSysModelItem BelongItem
        {
            get
            {
               if (_belongItem == null)
               {
                   FillParent("BelongItem");
               }
                return _belongItem;
            }
            set
            {
                _belongItem = value;
                OnPropertyUpdated("BelongItem");
            }
        }
        /// <summary>
        /// 所属分组
        /// </summary>
        public BPGroup BelongGroup
        {
            get
            {
               if (_belongGroup == null)
               {
                   FillParent("BelongGroup");
               }
                return _belongGroup;
            }
            set
            {
                _belongGroup = value;
                OnPropertyUpdated("BelongGroup");
            }
        }
        /// <summary>
        /// 所属的模块权限记录
        /// </summary>
        public int? GroupModelRightId
        {
            get
            {
                return _groupModelRightId;
            }
            set
            {
                _groupModelRightId=value;
                OnPropertyUpdated("GroupModelRightId");
            }
        }
        /// <summary>
        /// 所属的模块权限记录
        /// </summary>
        public BPGroupModelRight BelongGroupModelRight
        {
            get
            {
               if (_belongGroupModelRight == null)
               {
                   FillParent("BelongGroupModelRight");
               }
                return _belongGroupModelRight;
            }
            set
            {
                _belongGroupModelRight = value;
                OnPropertyUpdated("BelongGroupModelRight");
            }
        }
    }
}
