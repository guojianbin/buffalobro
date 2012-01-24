using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
namespace TestAddIn
{
    public partial class ManClass:TestAddIn.ManBase
    {
        /// <summary>
        /// ����
        /// </summary>
        protected string _manName = null;
        /// <summary>
        /// ����
        /// </summary>
        public string ManName
        {
            get
            {
                return _manName;
            }
            set
            {
                _manName=value;
                OnPropertyUpdated("_manName");
            }
        }
    }
}
