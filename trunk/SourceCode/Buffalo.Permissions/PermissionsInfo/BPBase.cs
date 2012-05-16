using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// Ȩ�޻���
    /// </summary>
    public partial class BPBase:ThinModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        private int? _id;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        private DateTime? _createDate;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        private DateTime? _laseUpdate;
        /// <summary>
        /// ID
        /// </summary>
        public int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id=value;
                OnPropertyUpdated("Id");
            }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate=value;
                OnPropertyUpdated("CreateDate");
            }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime? LaseUpdate
        {
            get
            {
                return _laseUpdate;
            }
            set
            {
                _laseUpdate=value;
                OnPropertyUpdated("LaseUpdate");
            }
        }
    }
}
