using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ����������
    /// </summary>
    public class GrneraterBase
    {
        protected EntityConfig _entity;

        /// <summary>
        /// ʵ����Ϣ
        /// </summary>
        public EntityConfig Entity
        {
            get
            {
                return _entity;
            }
        }

        /// <summary>
        /// ʵ�������ռ�
        /// </summary>
        public string EntityNamespace
        {
            get
            {
                return _entity.Namespace;
            }
        }

        private string _BQLEntityNamespace;
        /// <summary>
        /// BQLʵ��������ռ�
        /// </summary>
        public string BQLEntityNamespace 
        {
            get 
            {
                return _BQLEntityNamespace;
            }
        }

        /// <summary>
        /// ʵ�屸ע
        /// </summary>
        public string Summary
        {
            get
            {
                return _entity.Summary;
            }
        }

        /// <summary>
        /// ʵ������
        /// </summary>
        public string ClassName
        {
            get
            {
                return _entity.ClassName;
            }
        }


        private string _businessNamespace;
        /// <summary>
        /// ҵ��������ռ�
        /// </summary>
        public string BusinessNamespace
        {
            get
            {
                return _businessNamespace;
            }
        }

        private string _dataAccessNamespace;
        /// <summary>
        /// ���ݲ������ռ�
        /// </summary>
        public string DataAccessNamespace
        {
            get
            {
                return _dataAccessNamespace;
            }
        }

        private string _DBName;

        /// <summary>
        /// ���ݿ���
        /// </summary>
        public string DBName 
        {
            get 
            {
                return _DBName;
            }
        }

        public GrneraterBase(EntityConfig entity) 
        {
            _entity = entity;
            _BQLEntityNamespace = entity.Namespace + ".BQLEntity";
            _businessNamespace = entity.Namespace + ".Business";
            _dataAccessNamespace = entity.Namespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName; ;
        }
    }
}
