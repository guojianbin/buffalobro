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
        /// 最后更新时间
        /// </summary>
        private DateTime _lastDate;
        /// <summary>
        /// 创建时间
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
        /// 最后更新时间
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
        /// 创建时间
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
