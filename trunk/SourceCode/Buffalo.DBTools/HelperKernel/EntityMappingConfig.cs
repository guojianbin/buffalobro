using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DBTools.ROMHelper;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ʵ��ӳ���ļ�����
    /// </summary>
    public class EntityMappingConfig
    {
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <param name="entity"></param>
        public static void LoadConfigInfo(EntityConfig entity) 
        {
            FileInfo classFile = new FileInfo(entity.FileName);
            string fileName = classFile.DirectoryName + "\\BEM\\" + entity.ClassName + ".BEM.xml";
            if (!File.Exists(fileName)) 
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            XmlNodeList classNodes = doc.GetElementsByTagName("class");
            if (classNodes.Count > 0) 
            {
                XmlNode classNode = classNodes[0];
                XmlAttribute att = classNode.Attributes["TableName"];
                if (att != null)
                {
                    entity.TableName = att.InnerText;
                }
                att = classNode.Attributes["IsTable"];
                if (att != null)
                {
                    entity.IsTable = att.InnerText=="1";
                }
            }

            FillPropertyInfo(doc, entity);
            FillRelationInfo(doc, entity);
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        private static void FillPropertyInfo(XmlDocument doc, EntityConfig entity) 
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
                foreach (EntityParamField filed in entity.EParamFields)
                {
                    if (filed.FieldName == fName)
                    {
                        filed.IsGenerate = true;
                        att = node.Attributes["PropertyName"];
                        if (att != null)
                        {
                            filed.PropertyName = att.InnerText;
                        }
                        att = node.Attributes["DbType"];
                        if (att != null)
                        {
                            filed.DbType = att.InnerText;
                        }
                        att = node.Attributes["Length"];
                        if (att != null)
                        {
                            int len = 0;
                            int.TryParse(att.InnerText, out len);
                            filed.Length = len;
                        }
                        att = node.Attributes["EntityPropertyType"];
                        if (att != null)
                        {
                            int type = 0;
                            int.TryParse(att.InnerText, out type);
                            filed.EntityPropertyType = (EntityPropertyType)type;
                        }
                        att = node.Attributes["ParamName"];
                        if (att != null)
                        {
                            filed.ParamName = att.InnerText;
                        }
                        
                    }
                }
            }
        }
        /// <summary>
        /// ���ӳ����Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        private static void FillRelationInfo(XmlDocument doc, EntityConfig entity)
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
                foreach (EntityRelationItem filed in entity.ERelation)
                {
                    if (filed.FieldName == fName)
                    {
                        filed.IsGenerate = true;
                        att = node.Attributes["PropertyName"];
                        if (att != null)
                        {
                            filed.PropertyName = att.InnerText;
                        }
                        att = node.Attributes["SourceProperty"];
                        if (att != null)
                        {
                            filed.SourceProperty = att.InnerText;
                        }

                        att = node.Attributes["TargetProperty"];
                        if (att != null)
                        {

                            filed.TargetProperty = att.InnerText;
                        }
                        att = node.Attributes["IsToDB"];
                        if (att != null)
                        {
                            filed.IsToDB = att.InnerText=="1";
                        }
                        //att = node.Attributes["IsParent"];
                        //if (att != null)
                        //{
                        //    filed.IsParent = att.InnerText == "1";
                        //}
                        
                    }
                }
            }
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        public static void SaveXML(DBEntityInfo entity)
        {

            //string fileName = entity.FileName.Replace(entity.ClassName + ".cs", entity.ClassName + ".be.xml");
            FileInfo classFile = new FileInfo(entity.FileName);
            string dicName = classFile.DirectoryName + "\\BEM\\";
            if (!Directory.Exists(dicName))
            {
                Directory.CreateDirectory(dicName);
            }
            string fileName = dicName + entity.ClassName + ".BEM.xml";
            XmlDocument doc = ToXML(entity);
            SaveXML(fileName, doc);
            EnvDTE.ProjectItem newit = entity.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 3;
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        public static void SaveXML(EntityConfig entity) 
        {
            
            //string fileName = entity.FileName.Replace(entity.ClassName + ".cs", entity.ClassName + ".be.xml");
            FileInfo classFile = new FileInfo(entity.FileName);
            string dicName = classFile.DirectoryName + "\\BEM\\";
            if (!Directory.Exists(dicName)) 
            {
                Directory.CreateDirectory(dicName);
            }
            string fileName = dicName + entity.ClassName + ".BEM.xml";
            XmlDocument doc = ToXML(entity);
            SaveXML(fileName, doc);
            EnvDTE.ProjectItem newit = entity.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 3;
        }

        /// <summary>
        /// ����XML
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="doc">XML�ĵ�</param>
        public static void SaveXML(string path, XmlDocument doc) 
        {
            
            doc.Save(path);
        }

        /// <summary>
        /// �½�һ��XML�ĵ�
        /// </summary>
        /// <returns></returns>
        internal static XmlDocument NewXmlDocument() 
        {
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "no"));
            return doc;
        }

        /// <summary>
        /// ����XML
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XmlDocument LoadXML(string path) 
        {
            XmlDocument doc = NewXmlDocument();
            doc.Load(path);
            return doc;
        }


        /// <summary>
        /// ʵ������XML����
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static XmlDocument ToXML(DBEntityInfo entity)
        {
            XmlDocument doc = NewXmlDocument();

            XmlNode classNode = doc.CreateElement("class");
            doc.AppendChild(classNode);

            XmlAttribute att = doc.CreateAttribute("TableName");
            att.InnerText = entity.BelongTable.Name;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("ClassName");
            string className = entity.ClassName;
            att.InnerText = entity.EntityNamespace + "." + className;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("IsTable");
            att.InnerText = entity.BelongTable.IsView?"0":"1";
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("BelongDB");
            att.InnerText = entity.CurrentDBConfigInfo.DbName;
            classNode.Attributes.Append(att);

            AppendPropertyInfo(entity, classNode);
            AppendRelationInfo(entity, classNode);
            return doc;
        }

        /// <summary>
        /// ���ӳ����Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendRelationInfo(DBEntityInfo entity, XmlNode classNode)
        {
            XmlDocument doc = classNode.OwnerDocument;
            if (entity.BelongTable.RelationItems == null) 
            {
                return;
            }
            foreach (TableRelationAttribute field in entity.BelongTable.RelationItems)
            {
                
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("relation");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//�ֶ���
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//��Ӧ����������
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("SourceProperty");//���ݿ�����
                att.InnerText = EntityFieldBase.ToPascalName( field.SourceName);
                node.Attributes.Append(att);

                att = doc.CreateAttribute("TargetProperty");//���ݿ����ͳ���
                att.InnerText = EntityFieldBase.ToPascalName(field.TargetName);
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsToDB");//���ݿ����ͳ���
                att.InnerText = field.IsToDB ? "1" : "0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsParent");//���ݿ����ͳ���
                att.InnerText = field.IsParent ? "1" : "0";
                node.Attributes.Append(att);
            }
        }

        /// <summary>
        /// ������Ե�XML��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendPropertyInfo(DBEntityInfo entity, XmlNode classNode)
        {
            XmlDocument doc = classNode.OwnerDocument;
            if (entity.BelongTable.Params == null)
            {
                return;
            }
            foreach (EntityParam field in entity.BelongTable.Params)
            {
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("property");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//�ֶ���
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//��Ӧ����������
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("DbType");//���ݿ�����
                att.InnerText = field.SqlType.ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Length");//���ݿ����ͳ���
                att.InnerText = field.Length.ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("EntityPropertyType");//����
                att.InnerText = ((int)field.PropertyType).ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("ParamName");//�ֶ���
                att.InnerText = field.ParamName;
                node.Attributes.Append(att);
            }
        }

        /// <summary>
        /// ʵ������XML����
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static XmlDocument ToXML(EntityConfig entity) 
        {
            XmlDocument doc = NewXmlDocument();
            
            XmlNode classNode = doc.CreateElement("class");
            doc.AppendChild(classNode);

            XmlAttribute att = doc.CreateAttribute("TableName");
            att.InnerText = entity.TableName;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("ClassName");
            string className=entity.ClassName;
            if (entity.ClassType.Generic) 
            {
                className += "`" + entity.ClassType.TypeParameterCount;
            }
            att.InnerText = entity.Namespace+"."+className;
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("IsTable");
            att.InnerText = "1";
            classNode.Attributes.Append(att);

            att = doc.CreateAttribute("BelongDB");
            att.InnerText = entity.CurrentDBConfigInfo.DbName;
            classNode.Attributes.Append(att);

            AppendPropertyInfo(entity, classNode);
            AppendRelationInfo(entity, classNode);
            return doc;
        }

        /// <summary>
        /// ���ӳ����Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendRelationInfo(EntityConfig entity, XmlNode classNode) 
        {
            XmlDocument doc = classNode.OwnerDocument;
            foreach (EntityRelationItem field in entity.ERelation)
            {
                if (!field.IsGenerate)
                {
                    continue;
                }
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("relation");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//�ֶ���
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//��Ӧ����������
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("SourceProperty");//���ݿ�����
                att.InnerText = field.SourceProperty;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("TargetProperty");//���ݿ����ͳ���
                att.InnerText = field.TargetProperty;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsToDB");//���ݿ����ͳ���
                att.InnerText = field.IsToDB?"1":"0";
                node.Attributes.Append(att);

                att = doc.CreateAttribute("IsParent");//���ݿ����ͳ���
                att.InnerText = field.IsParent ? "1" : "0";
                node.Attributes.Append(att);
            }
        }

        /// <summary>
        /// ������Ե�XML��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="classNode"></param>
        private static void AppendPropertyInfo(EntityConfig entity, XmlNode classNode) 
        {
            XmlDocument doc=classNode.OwnerDocument;
            foreach (EntityParamField field in entity.EParamFields) 
            {
                if (!field.IsGenerate) 
                {
                    continue;
                }
                //EntityParamField field = kp.Value;
                XmlNode node = doc.CreateElement("property");
                classNode.AppendChild(node);

                XmlAttribute att = doc.CreateAttribute("FieldName");//�ֶ���
                att.InnerText = field.FieldName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("PropertyName");//��Ӧ����������
                att.InnerText = field.PropertyName;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("DbType");//���ݿ�����
                att.InnerText = field.DbType;
                node.Attributes.Append(att);

                att = doc.CreateAttribute("Length");//���ݿ����ͳ���
                att.InnerText = field.Length.ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("EntityPropertyType");//����
                att.InnerText = ((int)field.EntityPropertyType).ToString();
                node.Attributes.Append(att);

                att = doc.CreateAttribute("ParamName");//�ֶ���
                att.InnerText = field.ParamName;
                node.Attributes.Append(att);
            }
        }
    }
}
