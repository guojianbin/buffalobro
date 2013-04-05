using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestPerLib
{
    public partial class ScClass:ScBase
    {
        private string _className;
        /// <summary>
        /// 
        /// </summary>
        public string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                _className=value;
                OnPropertyUpdated("ClassName");
            }
        }

        public override string ToString()
        {
            return ClassName;
        }


    }
}
