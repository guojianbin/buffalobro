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
    /// ģ������Ȩ��
    /// </summary>
    public partial class BPGroupItemRight:BPBase
    {
        /// <summary>
        /// ��ģ��ID
        /// </summary>
        private int? _modelItemId;
        /// <summary>
        /// ��ID
        /// </summary>
        private int? _groupId;

        /// <summary>
        /// ������ģ��Ȩ�޼�¼ID
        /// </summary>
        private int? _groupModelRightId;

        /// <summary>
        /// ��Ȩ��
        /// </summary>
        private BPModelItemRight _itemRight;

        /// <summary>
        /// ��������
        /// </summary>
        private BPSysModelItem _belongItem;
        /// <summary>
        /// ��������
        /// </summary>
        private BPGroup _belongGroup;

        /// <summary>
        /// ������ģ��Ȩ�޼�¼
        /// </summary>
        private BPGroupModelRight _belongGroupModelRight;
        /// <summary>
        /// ��ģ��ID
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
        /// ��ID
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
        /// ��Ȩ��
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
        /// ��ȡ��ѯ������
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPGroupItemRight> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// ��������
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
        /// ��������
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
        /// ������ģ��Ȩ�޼�¼
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
        /// ������ģ��Ȩ�޼�¼
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
