using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;

namespace Buffalo.DB.CsqlCommon.CsqlConditionCommon
{
    public class CsqlEntityParamHandle:CsqlParamHandle
    {
        private EntityInfoHandle entityInfo;
        private EntityPropertyInfo pinfo;
        private CsqlEntityTableHandle _belongTable;//����

        
        /// <summary>
        /// ʵ��������Ϣ
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="propertyName">������</param>
        internal CsqlEntityParamHandle(EntityInfoHandle entityInfo, string propertyName, CsqlEntityTableHandle belongTable)
        {
            this.entityInfo = entityInfo;
            if (propertyName != "*")
            {
                pinfo = entityInfo.PropertyInfo[propertyName];
                if (pinfo == null) 
                {
                    throw new MissingMemberException(entityInfo.EntityType.FullName + "���в���������:" + propertyName);
                }
                this._valueDbType = pinfo.SqlType;
            }
            _belongTable = belongTable;
            
        }
        /// <summary>
        /// �����ı�
        /// </summary>
        public CsqlEntityTableHandle BelongEntity
        {
            get { return _belongTable; }
        }
        /// <summary>
        /// ʵ��������Ϣ
        /// </summary>
        internal EntityInfoHandle EntityInfo
        {
            get
            {
                return entityInfo;
            }
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        internal EntityPropertyInfo PInfo
        {
            get
            {
                return pinfo;
            }
        }

        /// <summary>
        /// ����������DataSet���ֶ���Ϣ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal string DisplayDataSetValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            StringBuilder sbRet = new StringBuilder();

            string tableName = null;
            if (!CommonMethods.IsNull(_belongTable))
            {
                if (info.AliasManager != null)
                {
                    string aliasName = info.AliasManager.GetTableAliasName(_belongTable);
                    if (!string.IsNullOrEmpty(aliasName))
                    {
                        tableName = aliasName;
                    }
                }
                else
                {
                    tableName = _belongTable.DisplayValue(info);
                }
            }
            else
            {
                tableName = entityInfo.TableName;
            }

            if (pinfo == null)//��ѯȫ���ֶ�ʱ��
            {

                foreach (EntityPropertyInfo eInfo in entityInfo.PropertyInfo)
                {
                    if (info.Infos.IsShowTableName)
                    {

                        sbRet.Append(idba.FormatTableName(tableName));
                    }
                    sbRet.Append(idba.FormatParam(eInfo.ParamName) + " as " + idba.FormatParam(eInfo.PropertyName) + ",");
                }
                if (sbRet.Length > 0)
                {
                    sbRet.Remove(sbRet.Length - 1, 1);
                }
            }
            else
            {
                if (info.Infos.IsShowTableName)
                {
                    sbRet.Append(idba.FormatTableName(tableName));
                    sbRet.Append(".");
                }
                sbRet.Append(idba.FormatParam(pinfo.ParamName) + " as " + idba.FormatParam(pinfo.PropertyName));
            }
            return sbRet.ToString();
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //if (info.Infos.IsPutPropertyName && !info.IsWhere)
            //{
            //    return DisplayDataSetValue(info);
            //}
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            StringBuilder sbRet = new StringBuilder();
            if (info.Infos.IsShowTableName)
            {
                if (info.AliasManager != null && !CommonMethods.IsNull(_belongTable))
                {
                    string aliasName = info.AliasManager.GetTableAliasName(_belongTable);
                    if (!string.IsNullOrEmpty(aliasName))
                    {
                        sbRet.Append(idba.FormatTableName(aliasName));
                        sbRet.Append(".");
                    }
                }
                else
                {
                    sbRet.Append(idba.FormatTableName(entityInfo.TableName));
                    sbRet.Append(".");
                }
            }

            if (pinfo == null)//��ѯȫ���ֶ�ʱ��
            {
                sbRet.Append("*");
            }
            else
            {
                sbRet.Append(idba.FormatParam(pinfo.ParamName));
            }
            return sbRet.ToString();
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            if (info.AliasManager != null)
            {
                _belongTable.FillInfo(info);
                
            }
        }
    }
}
