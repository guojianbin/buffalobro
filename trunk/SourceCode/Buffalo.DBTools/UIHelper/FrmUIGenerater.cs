using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Xml;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;

namespace Buffalo.DBTools.UIHelper
{
    public partial class FrmUIGenerater : Form
    {
        public FrmUIGenerater()
        {
            InitializeComponent();
        }
        private ClassDesignerInfo _designerInfo;

        /// <summary>
        /// 类设计图信息
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set 
            { 
                _designerInfo = value;
                
            }
        }
        private ClrTypeShape _selectedClass = null;

        /// <summary>
        /// 选中的类
        /// </summary>
        public ClrTypeShape SelectedClass
        {
            get { return _selectedClass; }
            set
            { 
                _selectedClass = value;
               
            }
        }

        

       

        private UIConfigItem _config;
        /// <summary>
        /// 配置信息
        /// </summary>
        public UIConfigItem Config
        {
            get { return _config; }
        }


        private void BindItems() 
        {
            if (_selectedClass == null || _designerInfo == null)
            {
                return;
            }
            XmlDocument doc = LoadConfig();
            _config = new UIConfigItem(doc, DesignerInfo);
            
            List<UIModelItem> lstItems = new List<UIModelItem>();
            List<ClrProperty> lstProperty = EntityConfig.GetAllMember<ClrProperty>(_selectedClass.AssociatedType, true);
            foreach (ClrProperty property in lstProperty) 
            {
                UIModelItem item = new UIModelItem(property);
                lstItems.Add(item);
            }
            gvMember.DataSource = lstItems;
        }


        private void CreateItems() 
        {
            List<ConfigItem> lstItem = _config.ConfigItems;

        }

        /// <summary>
        /// 检测文件
        /// </summary>
        private XmlDocument LoadConfig() 
        {
            FileInfo file = new FileInfo(DesignerInfo.CurrentProject.FileName);
            string directory = file.DirectoryName;
            directory = directory+"\\.bmodels\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string xml = "UIConfigItem.xml";
            string fileName = directory + xml;
            if (!File.Exists(fileName)) 
            {
                File.WriteAllText(fileName, Models.UIConfigItem);
            }
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(fileName);

            return xmldoc;
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        private void LoadInfo() 
        {
            BindItems();
        }

        private void FrmUIGenerater_Load(object sender, EventArgs e)
        {
            gvMember.AutoGenerateColumns = false;
            LoadInfo();
        }
    }
}