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
    /// 实体属性管理
    /// </summary>
    public class EntityInfoManager
    {
        private static Dictionary<string, EntityInfoHandle> _dicClass = new Dictionary<string, EntityInfoHandle>();//记录已经初始化过的类型

        /// <summary>
        /// 获取实体类里边得属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="isThrowException">是否抛出异常</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type,bool isThrowException)
        {
            string fullName = type.FullName;
            EntityInfoHandle classHandle = null;

            if (!DataAccessLoader.HasInit)
            {
                DataAccessLoader.InitConfig();
            }
            if (isThrowException && (!_dicClass.TryGetValue(fullName, out classHandle)))
            {
                throw new Exception("找不到实体" + fullName + "请检查配置文件");
            }

            return classHandle;
        }
        /// <summary>
        /// 获取实体类里边得属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type)
        {
            return GetEntityHandle(type,true);
        }
        /// <summary>
        /// 所有实体的信息
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
        /// 判断是否系统类型
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
        /// 填充类信息
        /// </summary>
        /// <param name="dicParam">字段</param>
        /// <param name="dicRelation">关系</param>
        private static void FillEntityInfos(Dictionary<string, EntityParam> dicParam,
            Dictionary<string, TableRelationAttribute> dicRelation, Type type,
            TableAttribute tableAtt, Dictionary<string, EntityConfigInfo> dicConfigs) 
        {
            string key=type.FullName;

            Stack<XmlDocument> stkXml = new Stack<XmlDocument>();//配置栈

            EntityConfigInfo curConfig = null;
            if (!dicConfigs.TryGetValue(key, out curConfig))
            {
                throw new Exception("找不到类:"+key+"所属的配置文件");
            }
            XmlDocument docCur = curConfig.ConfigXML;
            XmlNodeList nodes = docCur.GetElementsByTagName("class");
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
            stkXml.Push(docCur);

            Type baseType = type.BaseType;
            
            while (baseType != null && !IsSysBaseType(baseType)) //填充父类配置
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
                //初始化属性
                FillPropertyInfo(doc, dicParam);
                FillRelationInfo(doc, dicRelation);
            }
            
        }


        /// <summary>
        /// 填充映射信息
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        private static void FillRelationInfo(XmlDocument doc, Dictionary<string, TableRelationAttribute> dicRelation)
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

                dicRelation[tr.FieldName] = tr;

            }
        }

        /// <summary>
        /// 填充属性信息
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        private static void FillPropertyInfo(XmlDocument doc, Dictionary<string, EntityParam> dicParam)
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
                    int len = 0;
                    int.TryParse(att.InnerText, out len);
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
                ep.AllowNull = true;

                dicParam[ep.FieldName] = ep;
            }
        }

        /// <summary>
        /// 初始化所有实体
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
        /// 初始化类型的属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>如果已经初始化过侧返回false</returns>
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
            //实例化本类型的句柄
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

            //属性信息句柄
            FieldInfo[] destproper = type.GetFields(FastValueGetSet.allBindingFlags);
            using (DataBaseOperate oper = new DataBaseOperate(db))
            {
                ///读取属性别名
                foreach (FieldInfo finf in destproper)
                {

                    ///通过属性来反射
                    EntityParam ep = null;


                    if (dicParamsInfo.TryGetValue(finf.Name, out ep))
                    {
                        //if (tableAtt.IsParamNameUpper)
                        //{
                        //    ep.ParamName = ep.ParamName.ToUpper();
                        //}

                        if (ep.Identity) //给Oracle的主键加序列
                        {
                            string seqName = idb.GetSequenceName(tableAtt.TableName, ep.ParamName);
                            if (seqName != null)//如果是Oracle的
                            {
                                idb.InitSequence(seqName, oper);
                            }
                        }
                        string proName = ep.PropertyName;
                        GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                        SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                        if (getHandle != null || setHandle != null)
                        {
                            EntityPropertyInfo entityProperty = new EntityPropertyInfo(classInfo, getHandle, setHandle, ep, finf.FieldType, finf.Name);
                            dicPropertys.Add(proName, entityProperty);
                        }
                    }
                    else
                    {
                        TableRelationAttribute tableMappingAtt = null;

                        if (dicRelationInfo.TryGetValue(finf.Name, out tableMappingAtt))
                        {
                            Type targetType = DefaultType.GetRealValueType(finf.FieldType);
                            tableMappingAtt.SetEntity(type, targetType);
                            GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                            SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                            EntityMappingInfo entityMappingInfo = new EntityMappingInfo(type, getHandle, setHandle, tableMappingAtt, finf.Name, finf.FieldType);
                            dicMapping.Add(tableMappingAtt.PropertyName, entityMappingInfo);
                        }

                    }

                }
            }
            classInfo.SetInfoHandles(dicPropertys, dicMapping);

            _dicClass[fullName] = classInfo;
        }

        

        /// <summary>
        /// 获取某个方法的属性
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
