using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 生成器基类
    /// </summary>
    public class GrneraterBase
    {
        protected EntityConfig _entity;

        /// <summary>
        /// 实体信息
        /// </summary>
        public EntityConfig Entity
        {
            get
            {
                return _entity;
            }
        }

        /// <summary>
        /// 实体命名空间
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
        /// BQL实体的命名空间
        /// </summary>
        public string BQLEntityNamespace 
        {
            get 
            {
                return _BQLEntityNamespace;
            }
        }

        /// <summary>
        /// 实体备注
        /// </summary>
        public string Summary
        {
            get
            {
                return _entity.Summary;
            }
        }

        /// <summary>
        /// 实体名称
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
        /// 业务层命名空间
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
        /// 数据层命名空间
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
        /// 数据库名
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
