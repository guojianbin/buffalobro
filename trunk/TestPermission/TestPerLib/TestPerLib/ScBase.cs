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
    public partial class ScBase : EntityBase
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
        /// 状态
        /// </summary>
        private int? _state;
        /// <summary>
        /// ID
        /// </summary>
        public virtual int Id
        {
            get
            {
                return _id;
            }
            set
            {
                
                _id=value;
            }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastDate
        {
            get
            {
                return _lastDate;
            }
            set
            {
                _lastDate=value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate=value;
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int? State
        {
            get
            {
                return _state;
            }
            set
            {
                _state=value;
            }
        }
    }
}
