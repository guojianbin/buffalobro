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
    /// ��ģ��Ȩ��
    /// </summary>
    public partial class BPSysModelItem:BPBase
    {
        /// <summary>
        /// ����ģ��ID
        /// </summary>
        private int? _sysModelId;

        /// <summary>
        /// ������
        /// </summary>
        private string _itemName;

        /// <summary>
        /// ����ע��
        /// </summary>
        private string _itemDescription;

        /// <summary>
        /// ����ģ��
        /// </summary>
        private BPSysModel _belongModel;

        /// <summary>
        /// �����ʶ
        /// </summary>
        private string _itemIdentify;
        /// <summary>
        /// ��Ȩ��
        /// </summary>
        private BPModelItemRight _defaultRight;
        /// <summary>
        /// ����ģ��ID
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
        /// ������
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
        /// ����ע��
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
        /// ��ȡ��ѯ������
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPSysModelItem> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// ����ģ��
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
        /// �����ʶ
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
        /// ��Ȩ��
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
