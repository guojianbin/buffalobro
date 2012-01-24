using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
namespace TestAddIn
{
    public partial class ManBase:EntityBase
    {
        protected int? _id = DefaultValue.DefaultIntValue;
        /// <summary>
        /// 
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
                OnPropertyUpdated("_id");
            }
        }
        protected bool? _state = DefaultValue.DefaultBooleanValue;
        /// <summary>
        /// 
        /// </summary>
        public bool? State
        {
            get
            {
                return _state;
            }
            set
            {
                _state=value;
                OnPropertyUpdated("_state");
            }
        }
        protected DateTime? _lastUpdate = null;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
            set
            {
                _lastUpdate=value;
                OnPropertyUpdated("_lastUpdate");
            }
        }

        

    }
}
