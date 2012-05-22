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
    /// �û����ģ��Ȩ��
    /// </summary>
    public partial class BPGroupModelRight:BPBase
    {
        /// <summary>
        /// ��ģ��ID
        /// </summary>
        private int? _modelId;
        /// <summary>
        /// ��ID
        /// </summary>
        private int? _groupId;

        /// <summary>
        /// ģ��Ȩ��
        /// </summary>
        private BPModelRights _ModelRight;

        /// <summary>
        /// ����ģ��
        /// </summary>
        private BPSysModel _belongModel;
        /// <summary>
        /// ��������
        /// </summary>
        private BPGroup _belongGroup;

        /// <summary>
        /// ���������Ȩ��
        /// </summary>
        private List<BPGroupItemRight> _lstGroupItemRight;

        /// <summary>
        /// ��ģ��ID
        /// </summary>
        public int? ModelId
        {
            get
            {
                return _modelId;
            }
            set
            {
                _modelId=value;
                OnPropertyUpdated("ModelId");
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
        /// ģ��Ȩ��
        /// </summary>
        public BPModelRights ModelRight
        {
            get
            {
                return _ModelRight;
            }
            set
            {
                _ModelRight=value;
                OnPropertyUpdated("ModelRight");
            }
        }

        private static ModelContext<BPGroupModelRight> _____baseContext=new ModelContext<BPGroupModelRight>();
        /// <summary>
        /// ��ȡ��ѯ������
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPGroupModelRight> GetContext() 
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
        /// ���������Ȩ��
        /// </summary>
        public List<BPGroupItemRight> LstGroupItemRight
        {
            get
            {
               if (_lstGroupItemRight == null)
               {
                   FillChild("LstGroupItemRight");
               }
                return _lstGroupItemRight;
            }
        }
    }
}