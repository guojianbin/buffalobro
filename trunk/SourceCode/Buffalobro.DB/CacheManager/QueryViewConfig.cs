using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;
using Buffalobro.DB.PropertyAttributes;
using Buffalo.Kernel;
using Buffalobro.Kernel.FastReflection;

namespace Buffalobro.DB.CacheManager
{
    class QueryViewConfig
    {
        private static Dictionary<string, List<string>> dicConfig = new Dictionary<string,List<string>>();//��¼�ж��ٸ���ͼ���Ըñ�
        private static Dictionary<string, bool> dicRegisted = new Dictionary<string,bool>();//��¼�Ѿ�ע�����������ͼ

        //#region ��ʼ������
        ///// <summary>
        ///// ��ʼ������
        ///// </summary>
        //private static void InitConfig()
        //{
        //    dicConfig = new Dictionary<string, List<string>>();
        //    XmlDocument doc = ConfigXmlLoader.LoadXml("ViewConfig");
        //    if (doc != null)
        //    {
        //        LoadConfig(doc);
        //    }
        //}
        

        ///// <summary>
        ///// ������Ϣ
        ///// </summary>
        ///// <param name="doc">XML</param>
        //private static void LoadConfig(XmlDocument doc)
        //{
        //    XmlNodeList lstEntity = doc.GetElementsByTagName("view");
        //    foreach (XmlNode node in lstEntity)
        //    {
        //        XmlAttribute attName = node.Attributes["name"];
        //        if (attName != null)
        //        {
        //            string viewName = attName.Value;
        //            XmlNodeList lstTable = node.ChildNodes;
        //            foreach (XmlNode nodeTable in lstTable)
        //            {

        //                if (nodeTable.Name == "table")
        //                {
        //                    string tableName = nodeTable.InnerText;//��ȡ��Ӧ��Ķ�����

        //                    List<string> lstTableNames = null;
        //                    if (!dicConfig.TryGetValue(tableName, out lstTableNames)) //���ݱ�����ȡ�ñ��Ӧ����ͼ����
        //                    {
        //                        lstTableNames = new List<string>();
        //                        dicConfig.Add(tableName, lstTableNames);
        //                    }
        //                    lstTableNames.Add(viewName);
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion

        /// <summary>
        /// �Ǽ���ͼ
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public static bool RegisterView(Type viewType)
        {
            string vName=viewType.FullName;
            bool ret = false;
            if (dicRegisted.TryGetValue(vName, out ret)) 
            {
                return ret;
            }
            ViewRelationTables tInfo = FastInvoke.GetClassAttribute<ViewRelationTables>(viewType);
            if (tInfo != null) 
            {
                string[] names = tInfo.EntityNames;
                if (names != null) 
                {
                    foreach (string name in names) 
                    {
                        List<string> lstViewNames = null;
                        if (!dicConfig.TryGetValue(name, out lstViewNames)) //���ݱ�����ȡ�ñ��Ӧ����ͼ����
                        {
                            lstViewNames = new List<string>();
                            using (Lock objLock = new Lock(dicConfig))
                            {
                                dicConfig[name] = lstViewNames;
                            }
                        }
                        lstViewNames.Add(vName);
                    }
                    ret = true;
                }
            }
            dicRegisted[vName] = ret;
            return ret;
        }

        /// <summary>
        /// ����ʵ������ȡ��Ӧ����ͼ����
        /// </summary>
        /// <param name="entityName">ʵ����</param>
        /// <returns></returns>
        public static List<string> GetViewList(string entityName) 
        {
            //if (dicConfig == null) 
            //{
            //    InitConfig();
            //}
            List<string> retList = null;
            dicConfig.TryGetValue(entityName,out retList);
            return retList;
        }
    }
}
