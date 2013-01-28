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
        private EntityInfo _curEntityInfo;
        /// <summary>
        /// ��ǰʵ�����Ϣ
        /// </summary>
        public EntityInfo CurEntityInfo
        {
            get { return _curEntityInfo; }
            set { _curEntityInfo = value; }
        }

        //private object _currentEntity;

       

        private UIConfigItem _config;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public UIConfigItem Config
        {
            get { return _config; }
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        private void BindItems() 
        {
            if (_curEntityInfo==null)
            {
                return;
            }
            XmlDocument doc = LoadConfig();
            _config = new UIConfigItem(doc, _curEntityInfo.DesignerInfo);



            List<UIModelItem> lstItems = _curEntityInfo.Propertys;
            gvMember.DataSource = lstItems;
            BindProjects();
        }
        /// <summary>
        /// ����Ŀ��Ϣ
        /// </summary>
        private void BindProjects() 
        {
            List<UIProject> lstPorject = _config.Projects;
            gvProject.DataSource = lstPorject;
        }

        /// <summary>
        /// �����༶����Ϣ
        /// </summary>
        private void CreateClassItem() 
        {
            List<ConfigItem> lstItem = _config.ClassItems;
            pnlClassConfig.Controls.Clear();
            for (int i = 0; i < lstItem.Count; i++)
            {
                Control ctr = NewItem(lstItem[i]);

                pnlClassConfig.Controls.Add(ctr);
                tabPanel.Controls.SetChildIndex(ctr, i);

            }
        }

        /// <summary>
        /// �����ؼ�
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
        /// ��ӵ�Ԫ����ʽ
        /// </summary>
        private void AddCellStyle()
        {
            tabPanel.RowStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tabPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        }

        /// <summary>
        /// �����µ�������
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private Control NewItem(ConfigItem item) 
        {
            EditorBase editor = item.GetEditor();
            
            editor.BindPropertyName = item.Name;
            editor.LableText = item.Summary;
            editor.LableFont = new Font(editor.LableFont.FontFamily, 9, FontStyle.Bold);
            editor.LableWidth=80;
            editor.Width = 230;
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
            if(_currentItem==null)
            {
                return;
            }
            _currentItem.CheckItem[editor.BindPropertyName] = editor.Value;
            
        }

        private string _modelPath;
        /// <summary>
        /// ģ���Ŀ¼
        /// </summary>
        private string ModelPath 
        {
            get 
            {
                FileInfo file = new FileInfo(_curEntityInfo.DesignerInfo.CurrentProject.FileName);
                string directory = file.DirectoryName;
                directory = directory + "\\.bmodels\\";
                return directory;
            }
        }

        /// <summary>
        /// ����ļ�
        /// </summary>
        private XmlDocument LoadConfig() 
        {
            string directory = ModelPath;
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
        /// ������Ϣ
        /// </summary>
        private void LoadInfo() 
        {
            BindItems();
        }

        private void FrmUIGenerater_Load(object sender, EventArgs e)
        {
            gvMember.AutoGenerateColumns = false;
            gvProject.AutoGenerateColumns = false;
            LoadInfo();
            CreateItems();
            CreateClassItem();
            this.Text = "UI��������-" + _curEntityInfo.ClassName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private UIModelItem _currentItem = null;

        private void gvMember_CurrentCellChanged(object sender, EventArgs e)
        {
            if(gvMember.CurrentCell==null)
            {

                return;
            }
            _currentItem = gvMember.Rows[gvMember.CurrentCell.RowIndex].DataBoundItem as UIModelItem;
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root=doc.CreateElement("root");
            doc.AppendChild(root);
            List<UIModelItem> lst = gvMember.DataSource as List<UIModelItem>;
            foreach (UIModelItem item in lst) 
            {
                if (!item.IsGenerate) 
                {
                    continue;
                }
                XmlNode inode = doc.CreateElement("modelitem");
                root.AppendChild(inode);
                item.WriteNode(inode);
            }
            string directory=ModelPath+"gencache\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string fileName=directory+"\\"+_curEntityInfo.FullName+".cache.xml";
            doc.Save(fileName);
        }

        private void labInfo_Click(object sender, EventArgs e)
        {

        }

        

    }
}