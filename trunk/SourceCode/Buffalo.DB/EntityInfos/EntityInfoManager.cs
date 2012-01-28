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

        /// <summary>
        /// 设置对象信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="db"></param>
        internal static void SetEntityHandle(Type type, DBInfo db) 
        {
            InitEntityPropertyInfos(type, db);
        }
        public readonly static Type EntityBaseType = typeof(EntityBase);
        public readonly static Type ThinEntityBaseType = typeof(ThinModelBase<>);
        /// <summary>
        /// 判断是否系统类型
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static bool IsSysBaseType(Type objType) 
        {
            
            if (objType.GetGenericTypeDefinition() == ThinEntityBaseType) 
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
        /// 获取资源的配置文件
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="dicConfigs"></param>
        /// <returns></returns>
        private static XmlDocument GetResourceDocument(Type objType,Dictionary<string,XmlDocument> dicConfigs)
        {
            string key=objType.FullName;
            XmlDocument docRet=null;
            if(!dicConfigs.TryGetValue(key,out docRet))
            {
                Assembly ass=objType.Assembly;
                string[] resourceNames=ass.GetManifestResourceNames();
                foreach(string name in resourceNames)
                {
                    if(name.EndsWith(".BEM.xml",StringComparison.CurrentCultureIgnoreCase))
                    {
                        try
                        {
                            Stream stm=ass.GetManifestResourceStream(name);
                            XmlDocument doc=new XmlDocument();
                            doc.Load(stm);

                            //获取类名
                            XmlNodeList lstNode= doc.GetElementsByTagName("class");
                            if(lstNode.Count>0)
                            {
                                XmlNode classNode=lstNode[0];
                                XmlAttribute att= classNode.Attributes["ClassName"];
                                if(att!=null)
                                {
                                    string className=att.InnerText;
                                    if(!string.IsNullOrEmpty(className))
                                    {
                                        dicConfigs[className]=doc;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }

            dicConfigs.TryGetValue(key,out docRet);

            return docRet;
        }

        /// <summary>
        /// 填充类信息
        /// </summary>
        /// <param name="dicParam">字段</param>
        /// <param name="dicRelation">关系</param>
        private static void FillEntityInfos(Dictionary<string, EntityParam> dicParam,
            Dictionary<string, TableRelationAttribute> dicRelation, Type type,TableAttribute tableAtt,
            Dictionary<string,XmlDocument> dicConfigs) 
        {
            string key=type.FullName;
            Assembly ass=type.Assembly;
            

            Type baseType = type.BaseType;
            EntityInfoHandle baseHandle = null;
            if (baseType != null && !IsSysBaseType(baseType)) 
            {
                baseHandle = GetEntityHandle(baseType, false);
                if (baseType == null)
                {
                    InitEntityPropertyInfos(baseType, db);
                }
                baseHandle = GetEntityHandle(baseType, false);
            }
            if(baseHandle!=null)//填充基类的配置信息
            {
                foreach(EntityPropertyInfo info in baseHandle.PropertyInfo)
                {
                    EntityParam ep=info.ParamInfo;
                    dicParam[ep.FieldName]=ep;
                }
                foreach(EntityMappingInfo mInfo in baseHandle.MappingInfo)
                {
                    TableRelationAttribute tr=mInfo.MappingInfo;
                    dicParam[tr.FieldName]=tr;
                }
            }
            XmlDocument doc=GetResourceDocument(type,dicConfigs);

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
            //初始化属性
            FillPropertyInfo(doc, dicParam);
            FillRelationInfo(doc, dicRelation);
            
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
                    ep.SqlType = att.InnerText;
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
                ep.AllowNull = true;

                dicParam[ep.FieldName] = ep;
            }
        }
        /// <summary>
        /// 初始化类型的属性信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>如果已经初始化过侧返回false</returns>
        private static void InitEntityPropertyInfos(Type type, DBInfo db, 
            Dictionary<string, XmlDocument> dicConfigs)
        {
            if(type==null)
            {
                return;
            }

            Assembly ass = type.Assembly;



            IDBAdapter idb = db.CurrentDbAdapter;
            string fullName = type.FullName;
            TableAttribute tableAtt = new TableAttribute();
            //实例化本类型的句柄
            CreateInstanceHandler createrHandle = FastValueGetSet.GetCreateInstanceHandlerWithOutCache(type);
            Dictionary<string, EntityPropertyInfo> dicPropertys = new Dictionary<string, EntityPropertyInfo>();
            Dictionary<string, EntityMappingInfo> dicMapping = new Dictionary<string, EntityMappingInfo>();
            EntityInfoHandle classInfo = new EntityInfoHandle(type, createrHandle,tableAtt.TableName, db);
            Dictionary<string,EntityParam> dicParamsInfo=new Dictionary<string,EntityParam>();
            Dictionary<string,TableRelationAttribute> dicRelationInfo=new Dictionary<string,TableRelationAttribute>();
            FillEntityInfos(dicParamsInfo, dicRelationInfo, type, tableAtt, dicConfigs);
            

            //属性信息句柄
            FieldInfo[] destproper = type.GetFields(FastValueGetSet.allBindingFlags);
            FieldInfoHandle baseListHandle = null;
            using (DataBaseOperate oper = new DataBaseOperate(db))
            {
                ///读取属性别名
                foreach (FieldInfo finf in destproper)
                {
                    if (finf.Name == "_search_baselist_")
                    {
                        GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                        SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                        baseListHandle = new FieldInfoHandle(classInfo.EntityType,getHandle, setHandle, finf.FieldType, finf.Name);
                    }
                    else
                    {
                        ///通过属性来反射
                        EntityParam ep = null ;
                        

                        if (dicParamsInfo.TryGetValue(finf.Name))
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
                                EntityPropertyInfo entityProperty = new EntityPropertyInfo(classInfo,getHandle, setHandle, ep, finf.FieldType, finf.Name);
                                dicPropertys.Add(proName, entityProperty);
                            }
                        }
                        else
                        {
                            TableRelationAttribute tableMappingAtt = null ;

                            if (dicRelationInfo.TryGetValue(finf.Name))
                            {
                                Type targetType=DefaultType.GetRealValueType(finf.FieldType);
                                tableMappingAtt.SetEntity(type, targetType);
                                GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                                SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                                EntityMappingInfo entityMappingInfo = new EntityMappingInfo(type,getHandle, setHandle, tableMappingAtt, finf.Name, finf.FieldType);
                                dicMapping.Add(tableMappingAtt.PropertyName, entityMappingInfo);
                            }
                            
                        }
                    }
                }
            }
            classInfo.SetInfoHandles(dicPropertys, dicMapping, baseListHandle);

            _dicClass[fullName]=classInfo;
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
