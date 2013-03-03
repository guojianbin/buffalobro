using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase.BusinessBases;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
namespace TestPerLib
{
    public partial class ScBase : ThinModelBase
    {
        /// <summary>
        /// ID
        /// </summary>
        private int _id;
        /// <summary>
        /// ������ʱ��
        /// </summary>
        private DateTime _lastDate;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        private DateTime _createDate;
        /// <summary>
        /// ID
        /// </summary>
        public int Id
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
        /// ������ʱ��
        /// </summary>
        public DateTime LastDate
        {
            get
            {
                return _lastDate;
            }
            set
            {
                _lastDate=value;
                OnPropertyUpdated("LastDate");
            }
        }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateDate
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
    }
}
