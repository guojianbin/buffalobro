using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CacheManager;
using System.Reflection;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.BusinessBases;
using System.Xml;
using System.IO;
using System.Data;


namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ʵ�����Թ���
    /// </summary>
    public class EntityInfoManager
    {
        private static Dictionary<string, EntityInfoHandle> _dicClass = new Dictionary<string, EntityInfoHandle>();//��¼�Ѿ���ʼ����������

        /// <summary>
        /// ��ȡʵ������ߵ�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="isThrowException">�Ƿ��׳��쳣</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type,bool isThrowException)
        {
            string fullName = type.FullName;
            EntityInfoHandle classHandle = null;

            //if (!DataAccessLoader.HasInit)
            //{
            //    DataAccessLoader.InitConfig();
            //}
            if (isThrowException && (!_dicClass.TryGetValue(fullName, out classHandle)))
            {
                throw new Exception("�Ҳ���ʵ��" + fullName + "���������ļ�");
            }

            return classHandle;
        }
        /// <summary>
        /// ��ȡʵ������ߵ�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type)
        {
            return GetEntityHandle(type,true);
        }
        /// <summary>
        /// ����ʵ�����Ϣ
        /// </summary>
        internal static Dictionary<string, EntityInfoHandle> AllEntity
        {
            get 
            {
                return _dicClass;
            }
        }
        public readonly static Type EntityBaseType = typeof(EntityBase);
        public readonly static Type ThinEntityBaseType = typeof(ThinModelBase);
        /// <summary>
        /// �ж��Ƿ�ϵͳ����
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static bool IsSysBaseType(Type objType) 
        {

            if (objType.IsGenericType && objType == ThinEntityBaseType) 
            {
                return true;
            }
            if (objType == EntityBaseType) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="dicParam">�ֶ�</param>
        /// <param name="dicRelation">��ϵ</param>
        private static void FillEntityInfos(Dictionary<string, EntityParam> dicParam,
            Dictionary<string, TableRelationAttribute> dicRelation, Type type,
            TableAttribute tableAtt, Dictionary<string, EntityConfigInfo> dicConfigs) 
        {
            string key=type.FullName;

            Stack<XmlDocument> stkXml = new Stack<XmlDocument>();//����ջ

            EntityConfigInfo curConfig = null;
            if (!dicConfigs.TryGetValue(key, out curConfig))
            {
                throw new Exception("�Ҳ�����:"+key+"�����������ļ�");
            }
            XmlDocument docCur = curConfig.ConfigXML;
            FillEntityInfo(docCur, tableAtt);
            stkXml.Push(docCur);
            
            Type baseType = type.BaseType;
            
            while (baseType != null && !IsSysBaseType(baseType)) //��丸������
            {
                EntityConfigInfo config = null;
                string baseKey =null;
                if (baseType.IsGenericType)
                {
                    baseKey = baseType.GetGenericTypeDefinition().FullName;
                }
                else 
                {
                    baseKey = baseType.FullName;
                }
                if (dicConfigs.TryGetValue(baseKey, out config))
                {
                    stkXml.Push(config.ConfigXML);
                }
                baseType = baseType.BaseType;
            }


            while (stkXml.Count > 0)
            {
                XmlDocument doc = stkXml.Pop();
                //��ʼ������
                FillPropertyInfo(doc, dicParam);
                FillRelationInfo(doc, dicRelation);
            }
            
        }

        /// <summary>
        /// ���ʵ����Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dicRelation"></param>
        internal static void FillEntityInfo(XmlDocument doc, TableAttribute tableAtt)
        {
            XmlNodeList nodes = doc.GetElementsByTagName("class");
            if (nodes.Count > 0)
            {
                XmlNode node = nodes[0];
                XmlAttribute att = node.Attributes["TableName"];
                if (att != null)
                {
                    tableAtt.TableName = att.InnerText;
                }

                att = node.Attributes["BelongDB"];
                if (att != null)
                {
                    tableAtt.BelongDB = att.InnerText;
                }
            }
        }

        /// <summary>
        /// ���ӳ����Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        internal static void FillRelationInfo(XmlDocument doc, Dictionary<string, TableRelationAttribute> dicRelation)
        {
            XmlNodeList lstProperty = doc.GetElementsByTagName("relation");
            foreach (XmlNode node in lstProperty)
            {
                XmlAttribute att = node.Attributes["FieldName"];
                if (att == null)
                {
                    continue;
                }
                string fName = att.InnerText;
                if (string.IsNullOrEmpty(fName))
                {
                    continue;
                }
                TableRelationAttribute tr = new TableRelationAttribute();

                tr.FieldName = fName;

                att = node.Attributes["PropertyName"];
                if (att != null)
                {
                    tr.PropertyName = att.InnerText;
                }
                att = node.Attributes["SourceProperty"];
                if (att != null)
                {
                    tr.SourceName = att.InnerText;
                }

                att = node.Attributes["TargetProperty"];
                if (att != null)
                {
                    tr.TargetName = att.InnerText;
                }

                att = node.Attributes["IsParent"];
                if (att != null)
                {
                    tr.IsParent = att.InnerText == "1";
                }
                att = node.Attributes["IsToDB"];
                if (att != null)
                {
                    tr.IsToDB = att.InnerText == "1";
                }
                dicRelation[tr.FieldName] = tr;

            }
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        internal static void FillPropertyInfo(XmlDocument doc, Dictionary<string, EntityParam> dicParam)
        {
            XmlNodeList lstProperty = doc.GetElementsByTagName("property");
            foreach (XmlNode node in lstProperty)
            {
                XmlAttribute att = node.Attributes["FieldName"];
                if (att == null)
                {
                    continue;
                }
                string fName = att.InnerText;
                if (string.IsNullOrEmpty(fName))
                {
                    continue;
                }
                EntityParam ep = new EntityParam();
                ep.FieldName = fName;

                att = node.Attributes["PropertyName"];
                if (att != null)
                {
                    ep.PropertyName = att.InnerText;
                }
                att = node.Attributes["DbType"];
                if (att != null)
                {
                    ep.SqlType =(DbType)Enum.Parse(typeof(DbType),att.InnerText,true);
                }
                att = node.Attributes["Length"];
                if (att != null)
                {
                    long len = 0;
                    long.TryParse(att.InnerText, out len);
                    ep.Length = len;
                }
                att = node.Attributes["EntityPropertyType"];
                if (att != null)
                {
                    int type = 0;
                    int.TryParse(att.InnerText, out type);
                    ep.PropertyType = (EntityPropertyType)type;
                }
                att = node.Attributes["ParamName"];
                if (att != null)
                {
                    ep.ParamName = att.InnerText;
                }
                att = node.Attributes["ReadOnly"];
                if (att != null)
                {
                    ep.ReadOnly = att.InnerText=="1";
                }
                ep.AllowNull = true;

                dicParam[ep.FieldName] = ep;
            }
        }

        /// <summary>
        /// ��ʼ������ʵ��
        /// </summary>
        /// <param name="dicConfigs"></param>
        internal static void InitAllEntity(Dictionary<string, EntityConfigInfo> dicConfigs) 
        {
            foreach (KeyValuePair<string, EntityConfigInfo> item in dicConfigs) 
            {
                EntityConfigInfo info = item.Value;
                if (info.Type != null)
                {
                    InitEntityPropertyInfos(info.Type, dicConfigs);
                }
            }
        }

        /// <summary>
        /// ���û�ҵ����ֶκ͹�ϵ
        /// </summary>
        /// <param name="dicParams">�ֶ�����</param>
        /// <param name="dicRelation">��ϵ����</param>
        /// <param name="dicNotFindParam">û�ҵ����ֶ�</param>
        /// <param name="dicNotFindRelation">û�ҵ��Ĺ�ϵ</param>
        private static void FillNotFoundField(Dictionary<string, EntityParam> dicParams,
            Dictionary<string, TableRelationAttribute> dicRelation, Dictionary<string, bool> dicNotFoundParam,
            Dictionary<string, bool> dicNotFoundRelation) 
        {
            foreach (KeyValuePair<string, EntityParam> kvpPrm in dicParams) 
            {
                dicNotFoundParam[kvpPrm.Key] = true;
            }
            foreach (KeyValuePair<string, TableRelationAttribute> kvpTr in dicRelation)
            {
                dicNotFoundRelation[kvpTr.Key] = true;
            }
        }

        /// <summary>
        /// ��ʼ�����͵�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>����Ѿ���ʼ�����෵��false</returns>
        private static void InitEntityPropertyInfos(Type type,
            Dictionary<string, EntityConfigInfo> dicConfigs)
        {
            if (type == null)
            {
                return;
            }


            string fullName = type.FullName;
            TableAttribute tableAtt = new TableAttribute();
            CreateInstanceHandler createrHandle = null;
            //ʵ���������͵ľ��
            if (!type.IsGenericType)
            {
                createrHandle = FastValueGetSet.GetCreateInstanceHandlerWithOutCache(type);
            }
            Dictionary<string, EntityPropertyInfo> dicPropertys = new Dictionary<string, EntityPropertyInfo>();
            Dictionary<string, EntityMappingInfo> dicMapping = new Dictionary<string, EntityMappingInfo>();

            Dictionary<string, EntityParam> dicParamsInfo = new Dictionary<string, EntityParam>();
            Dictionary<string, TableRelationAttribute> dicRelationInfo = new Dictionary<string, TableRelationAttribute>();
            FillEntityInfos(dicParamsInfo, dicRelationInfo, type, tableAtt, dicConfigs);
            DBInfo db = DataAccessLoader.GetDBInfo(tableAtt.BelongDB);
            IDBAdapter idb = db.CurrentDbAdapter;
            EntityInfoHandle classInfo = new EntityInfoHandle(type, createrHandle, tableAtt.TableName, db);

            Dictionary<string, bool> dicNotFoundParam=new Dictionary<string,bool>();
            Dictionary<string, bool> dicNotFoundRelation = new Dictionary<string, bool>();
            FillNotFoundField(dicParamsInfo, dicRelationInfo, dicNotFoundParam, dicNotFoundRelation);

            //������Ϣ���
            List<FieldInfoHandle> lstFields=FieldInfoHandle.GetFieldInfos(type, FastValueGetSet.allBindingFlags, true);
            DataBaseOperate oper = db.DefaultOperate;
            
                ///��ȡ���Ա���
            foreach (FieldInfoHandle finf in lstFields)
            {

                ///ͨ������������
                EntityParam ep = null;


                if (dicParamsInfo.TryGetValue(finf.FieldName, out ep))
                {
                    //if (tableAtt.IsParamNameUpper)
                    //{
                    //    ep.ParamName = ep.ParamName.ToUpper();
                    //}
                    string proName = ep.PropertyName;
                    //GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                    //SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                    if (finf.HasGetHandle || finf.HasSetHandle)
                    {
                        EntityPropertyInfo entityProperty = new EntityPropertyInfo(classInfo, finf.GetHandle, finf.SetHandle, ep, finf.FieldType, finf.FieldName);
                        dicPropertys.Add(proName, entityProperty);
                        dicNotFoundParam.Remove(finf.FieldName);
                    }
                }
                else
                {
                    TableRelationAttribute tableMappingAtt = null;

                    if (dicRelationInfo.TryGetValue(finf.FieldName, out tableMappingAtt))
                    {
                        Type targetType = DefaultType.GetRealValueType(finf.FieldType);
                        tableMappingAtt.SetEntity(type, targetType);
                        //GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                        //SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                        EntityMappingInfo entityMappingInfo = new EntityMappingInfo(type, finf.GetHandle, finf.SetHandle, tableMappingAtt, finf.FieldName, finf.FieldType);
                        dicMapping.Add(tableMappingAtt.PropertyName, entityMappingInfo);
                        dicNotFoundRelation.Remove(finf.FieldName);
                    }

                }

            }
            

            if (dicNotFoundParam.Count > 0 || dicNotFoundRelation.Count > 0) 
            {
                StringBuilder message = new StringBuilder();
                
                foreach(KeyValuePair<string,bool> kvp in dicNotFoundParam)
                {
                    message.Append(kvp.Key + "��");
                }
                

                foreach (KeyValuePair<string, bool> kvp in dicNotFoundRelation)
                {
                    message.Append(kvp.Key + "��");
                }
                if (message.Length > 0)
                {
                    message.Remove(message.Length - 1, 1);
                }
                message.Insert(0, "��:" + type.FullName + " �Ҳ����ֶ�");
                throw new MissingFieldException(message.ToString());
            }
            classInfo.SetInfoHandles(dicPropertys, dicMapping);

            _dicClass[fullName] = classInfo;
        }



        /// <summary>
        /// ��ȡĳ������������
        /// </summary>
        /// <param name="finf"></param>
        /// <returns></returns>
        private static TableRelationAttribute GetMappingParam(FieldInfo finf)
        {
            object entityParam = FastInvoke.GetPropertyAttribute(finf, typeof(TableRelationAttribute));
            if (entityParam != null)
            {
                return (TableRelationAttribute)entityParam;
            }
            return null;
        }
    }
}
