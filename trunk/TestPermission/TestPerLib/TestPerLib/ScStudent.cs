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
        /// <summary>
        /// �����༶ID
        /// </summary>
        private int _classId;

        /// <summary>
        /// �����༶
        /// </summary>
        private ScClass _belongClass;
        /// <summary>
        /// �����༶ID
        /// </summary>
        public int ClassId
        {
            get
            {
                return _classId;
            }
            set
            {
                _classId=value;
                OnPropertyUpdated("ClassId");
            }
        }



        /// <summary>
        /// �����༶
        /// </summary>
        public ScClass BelongClass
        {
            get
            {
               if (_belongClass == null)
               {
                   FillParent("BelongClass");
               }
                return _belongClass;
            }
            set
            {
                _belongClass = value;
                OnPropertyUpdated("BelongClass");
            }
        }

    }
}
