using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��Ŀ��
    /// </summary>
    public class UIProject
    {
        private string _name;
        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<UIProjectItem> _lstItems=new List<UIProjectItem>();

        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public List<UIProjectItem> LstItems
        {
            get { return _lstItems; }
        }

        /// <summary>
        /// ���ɴ���
        /// </summary>
        /// <returns></returns>
        public string GenerateCode(EntityInfo entityInfo, UIConfigItem classConfig,
            List<UIModelItem> selectPropertys) 
        {
            foreach (UIProjectItem pitem in _lstItems) 
            {
                string mPath = UIConfigItem.FormatParameter(pitem.ModelPath, entityInfo);
                string tPath = UIConfigItem.FormatParameter(pitem.TargetPath, entityInfo);

            }
        }

    }
}
