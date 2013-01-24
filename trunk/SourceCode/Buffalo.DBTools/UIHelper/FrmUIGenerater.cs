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
using Buffalo.WinFormsControl.Editors;
using Buffalo.Kernel.FastReflection;

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

        private object _currentEntity;

       

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

        /// <summary>
        /// 创建控件
        /// </summary>
        private void CreateItems() 
        {
            List<ConfigItem> lstItem = _config.ConfigItems;
            tabPanel.ColumnCount = 2;
            tabPanel.RowCount = (int)Math.Ceiling((double)lstItem.Count / (double)2);
            tabPanel.RowStyles.Clear();
            tabPanel.ColumnStyles.Clear();
            AddCellStyle();
            for (int i = 0; i < lstItem.Count; i++) 
            {
                Control ctr = NewItem(lstItem[i]);
                int col = i % 2;
                int row = i / 2;
                tabPanel.Controls.Add(ctr, col, row);
                tabPanel.Controls.SetChildIndex(ctr, i);
                
                    ctr.Dock = DockStyle.Left;
                
            }
        }
        /// <summary>
        /// 添加单元格样式
        /// </summary>
        private void AddCellStyle()
        {
            tabPanel.RowStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        }
        private Control NewItem(ConfigItem item) 
        {
            EditorBase editor = null;
            switch (item.Type) 
            {
                case ConfigItemType.Check:
                    CheckBoxEditor cbe = new CheckBoxEditor();
                    editor = cbe;
                    cbe.OnOffType = OnOffButtonType.Oblongrectangle;
                    break;
                case ConfigItemType.Combo:
                    editor = new ComboBoxEditor();
                    break;
                case ConfigItemType.Text:
                    editor = new TextBoxEditor();
                    break;
                default:
                    editor = new TextBoxEditor();
                    break;
            }
            editor.BindPropertyName = item.Name;
            editor.LableText = item.Summary;
            editor.LableFont = new Font(editor.LableFont.FontFamily, 9, FontStyle.Bold);
            
            editor.OnValueChange += new ValueChangeHandle(editor_OnValueChange);
            return editor;
        }

        void editor_OnValueChange(object sender, object oldValue, object newValue)
        {
            EditorBase editor = sender as EditorBase;
            if (editor == null) 
            {
                return;
            }
            if(_currentEntity==null)
            {
                return;
            }
            PropertyInfoHandle handle = FastValueGetSet.GetPropertyInfoHandle(editor.BindPropertyName, _currentEntity.GetType());
            if(handle==null || handle.HasSetHandle==null)
            {
                return;
            }
            handle.SetValue(_currentEntity,editor.Value);
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
            CreateItems();
        }
    }
}