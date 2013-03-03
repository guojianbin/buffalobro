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
    public partial class ScStudent:ScBase
    {
        /// <summary>
        /// ѧ����
        /// </summary>
        private string _name;

        /// <summary>
        /// ����
        /// </summary>
        private int _age;
        /// <summary>
        /// ѧ����
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name=value;
                OnPropertyUpdated("Name");
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age=value;
                OnPropertyUpdated("Age");
            }
        }
        private static ModelContext<ScStudent> _____baseContext=new ModelContext<ScStudent>();
        /// <summary>
        /// ��ȡ��ѯ������
        /// </summary>
        /// <returns></returns>
        public static ModelContext<ScStudent> GetContext() 
        {
            return _____baseContext;
        }
    }
}
