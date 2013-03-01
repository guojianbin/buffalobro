using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.UIHelper.ModelLoader;
using EnvDTE;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 项目项
    /// </summary>
    public class UIProject
    {
        private string _name;
        /// <summary>
        /// 项目名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<UIProjectItem> _lstItems=new List<UIProjectItem>();

        /// <summary>
        /// 项目项
        /// </summary>
        public List<UIProjectItem> LstItems
        {
            get { return _lstItems; }
        }

         /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="classConfig">UI配置信息</param>
        /// <param name="selectPropertys">选中需要生成的属性信息</param>
        /// <returns></returns>
        public void GenerateCode(EntityInfo entityInfo, UIConfigItem classConfig,Project selectedProject, List<UIModelItem> selectPropertys, UIModelItem classInfo)
        {
            GenerateCode(entityInfo, classConfig,selectedProject, selectPropertys,classInfo, _lstItems, null);
        }

        /// <summary>
        /// 清空项目的编译缓存
        /// </summary>
        public void ClearCache(EntityInfo entityInfo)
        {
            CodeGenCache.Clear();
        }
        ///// <summary>
        ///// 清空项目的编译缓存
        ///// </summary>
        //public void ClearCache(EntityInfo entityInfo, List<UIProjectItem> lstItem) 
        //{
        //    foreach (UIProjectItem pitem in lstItem)
        //    {
        //        string mPath = UIConfigItem.FormatParameter(pitem.ModelPath, entityInfo);
        //        CodeGenCache.DeleteGenerationer(mPath);
        //        if (pitem.ChildItems != null && pitem.ChildItems.Count > 0)
        //        {
        //            ClearCache(entityInfo, pitem.ChildItems);
        //        }
        //    }
        //}

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="classConfig">UI配置信息</param>
        /// <param name="selectPropertys">选中需要生成的属性信息</param>
        /// <param name="lstItem">UI项目项</param>
        /// <param name="parentItem">父项</param>
        /// <returns></returns>
        private void GenerateCode(EntityInfo entityInfo, UIConfigItem classConfig, Project selectedProject,
            List<UIModelItem> selectPropertys,UIModelItem classInfo,List<UIProjectItem> lstItem,ProjectItem parentItem) 
        {
            Encoding fileEncoding = FileEncodingInfo.GetEncodingType(entityInfo.FileName, true);

            foreach (UIProjectItem pitem in lstItem) 
            {
                string mPath = UIConfigItem.FormatParameter(pitem.ModelPath, entityInfo,selectedProject);
                string tPath = UIConfigItem.FormatParameter(pitem.TargetPath, entityInfo, selectedProject);
                CodeGenInfo info=CodeGenCache.GetGenerationer(mPath,entityInfo);
                string content=info.Invoke(entityInfo, classConfig, selectPropertys,classInfo);

                ProjectItem item = SaveItem(tPath, selectedProject, content, pitem.GenType, parentItem, fileEncoding);
                if (pitem.ChildItems != null && pitem.ChildItems.Count > 0) 
                {
                    GenerateCode(entityInfo, classConfig, selectedProject, selectPropertys, classInfo, pitem.ChildItems, item);
                }
            }

            
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="entityInfo"></param>
        /// <param name="content"></param>
        /// <param name="baction"></param>
        /// <returns></returns>
        private ProjectItem SaveItem(string fileName, Project selectedProject,
            string content, BuildAction baction, ProjectItem parentItem,Encoding encoding) 
        {
            CodeFileHelper.SaveFile(fileName, content, encoding);
            EnvDTE.ProjectItem newit = null;
            if (parentItem != null)
            {
                newit = parentItem.ProjectItems.AddFromFile(fileName);
                newit.Properties.Item("BuildAction").Value = (int)baction;
            }
            else 
            {
                newit = selectedProject.ProjectItems.AddFromFile(fileName);
                newit.Properties.Item("BuildAction").Value = (int)baction;
            }
            return newit;
        }

    }
}
