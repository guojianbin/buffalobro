using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DBTools.ROMHelper;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ����������
    /// </summary>
    public class GrneraterBase
    {
        //protected EntityConfig _entity;

        ///// <summary>
        ///// ʵ����Ϣ
        ///// </summary>
        //public EntityConfig Entity
        //{
        //    get
        //    {
        //        return _entity;
        //    }
        //}

        private KeyWordTableParamItem _table;

        /// <summary>
        /// ����Ϣ
        /// </summary>
        public KeyWordTableParamItem Table
        {
            get { return _table; }
        }

        private Project _currentProject;

        /// <summary>
        /// ��ǰ��Ŀ
        /// </summary>
        public Project CurrentProject 
        {
            get 
            {
                return _currentProject;
            }
        }

        private string _entityBaseTypeName;

        /// <summary>
        /// ʵ��Ļ���
        /// </summary>
        public string EntityBaseTypeName 
        {
            get 
            {
                return _entityBaseTypeName;
            }
        }
        private string _entityBaseTypeShortName;

        /// <summary>
        /// ʵ��Ļ�����
        /// </summary>
        public string EntityBaseTypeShortName 
        {
            get 
            {
                return _entityBaseTypeShortName;
            }
        }
        private string _entityFileName;

        /// <summary>
        /// ʵ���ļ���
        /// </summary>
        public string EntityFileName 
        {
            get
            {
                return _entityFileName;
            }
        }

        private string _entityNamespace;

        /// <summary>
        /// ʵ�������ռ�
        /// </summary>
        public string EntityNamespace
        {
            get
            {
                return _entityNamespace;
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

        private string _className;
        /// <summary>
        /// ʵ������
        /// </summary>
        public string ClassName
        {
            get
            {
                return _className;
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

        public GrneraterBase(DBEntityInfo entity,Project curProject) 
        {
            _table = entity.ToTableInfo();
            _className = entity.ClassName;
            _currentProject = curProject;
            _entityBaseTypeName = entity.BaseType;
            _entityBaseTypeShortName = entity.BaseType;
            int lastDot = _entityBaseTypeShortName.LastIndexOf('.');
            if (lastDot >= 0)
            {
                _entityBaseTypeShortName = _entityBaseTypeShortName.Substring(lastDot + 1, _entityBaseTypeShortName.Length - lastDot - 1);
            }
            _entityFileName = entity.FileName;
            _entityNamespace = entity.EntityNamespace;
            _BQLEntityNamespace = entity.EntityNamespace + ".BQLEntity";
            _businessNamespace = entity.EntityNamespace + ".Business";
            _dataAccessNamespace = entity.EntityNamespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName; 
        }

        public GrneraterBase(EntityConfig entity) 
        {
            //_entity = entity;
            //_tableName = entity.TableName;
            _table = entity.ToTableInfo();
            _currentProject = entity.CurrentProject;
            _entityBaseTypeName = entity.BaseTypeName;

            _entityBaseTypeShortName = _entityBaseTypeName;
            int lastDot = _entityBaseTypeShortName.LastIndexOf('.');
            if (lastDot >= 0)
            {
                _entityBaseTypeShortName = _entityBaseTypeShortName.Substring(lastDot + 1, _entityBaseTypeShortName.Length-lastDot - 1);
            }
            _entityFileName = entity.FileName;
            _entityNamespace = entity.Namespace;
            //_summary = entity.Summary;
            _className = entity.ClassName;
            _BQLEntityNamespace = entity.Namespace + ".BQLEntity";
            _businessNamespace = entity.Namespace + ".Business";
            _dataAccessNamespace = entity.Namespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName; 
        }

        
    }
}
