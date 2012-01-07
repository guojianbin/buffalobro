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
        private static Dictionary<string, List<string>> dicConfig = new Dictionary<string,List<string>>();//记录有多少个视图来自该表
        private static Dictionary<string, bool> dicRegisted = new Dictionary<string,bool>();//记录已经注册过关联的视图

        //#region 初始化配置
        ///// <summary>
        ///// 初始化配置
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
        ///// 加载信息
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
        //                    string tableName = nodeTable.InnerText;//获取对应表的对象名

        //                    List<string> lstTableNames = null;
        //                    if (!dicConfig.TryGetValue(tableName, out lstTableNames)) //根据表名获取该表对应的视图集合
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
        /// 登记视图
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
                        if (!dicConfig.TryGetValue(name, out lstViewNames)) //根据表名获取该表对应的视图集合
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
        /// 根据实体名获取对应的视图集合
        /// </summary>
        /// <param name="entityName">实体名</param>
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
