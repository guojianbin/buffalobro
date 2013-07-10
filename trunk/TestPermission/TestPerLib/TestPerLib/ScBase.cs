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
        /// ������ʱ��
        /// </summary>
        private DateTime _lastDate;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        private DateTime _createDate;
        /// <summary>
        /// ״̬
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
        /// ������ʱ��
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
        /// ����ʱ��
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
        /// ״̬
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
