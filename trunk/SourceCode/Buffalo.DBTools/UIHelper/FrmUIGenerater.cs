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
using System.Security.AccessControl;

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

        private UIModelItem _classInfo = null;

        private UIConfigItem _config;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public UIConfigItem Config
        {
            get { return _config; }
        }

        private Dictionary<string, EditorBase> _classUIConfig = null;
        private Dictionary<string, EditorBase> _propertyUIConfig = null;
        #region ����Ϣ
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
            _classUIConfig = new Dictionary<string, EditorBase>();
            List<ConfigItem> lstItem = _config.ClassItems;
            pnlClassConfig.Controls.Clear();
            for (int i = 0; i < lstItem.Count; i++)
            {
                EditorBase editor = NewItem(lstItem[i]);

                pnlClassConfig.Controls.Add(editor);
                //tabPanel.Controls.SetChildIndex(editor, i);
                editor.OnValueChange += new ValueChangeHandle(editorClass_OnValueChange);
                _classUIConfig[editor.BindPropertyName] = editor;
            }
        }
        void editorClass_OnValueChange(object sender, object oldValue, object newValue)
        {
            EditorBase editor = sender as EditorBase;
            if (editor == null)
            {
                return;
            }
            
            _classInfo.CheckItem[editor.BindPropertyName] = editor.Value;

        }
        /// <summary>
        /// �����ؼ�
        /// </summary>
        private void CreateItems() 
        {
            _propertyUIConfig = new Dictionary<string, EditorBase>();
            List<ConfigItem> lstItem = _config.ConfigItems;
            tabPanel.ColumnCount = 2;
            tabPanel.RowCount = (int)Math.Ceiling((double)lstItem.Count / (double)2)+1;
            tabPanel.RowStyles.Clear();
            tabPanel.ColumnStyles.Clear();
            AddCellStyle();
            for (int i = 0; i < lstItem.Count; i++) 
            {
                EditorBase editor = NewItem(lstItem[i]);
                int col = i % 2;
                int row = i / 2;
                tabPanel.Controls.Add(editor, col, row);
                tabPanel.Controls.SetChildIndex(editor, i);
                editor.OnValueChange += new ValueChangeHandle(editor_OnValueChange);
                editor.Dock = DockStyle.Left;
                _propertyUIConfig[editor.BindPropertyName] = editor;
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
        private EditorBase NewItem(ConfigItem item) 
        {
            EditorBase editor = item.GetEditor();
            
            editor.BindPropertyName = item.Name;
            editor.LableText = item.Summary;
            editor.LableFont = new Font(editor.LableFont.FontFamily, 9, FontStyle.Bold);
            editor.LableWidth=80;
            editor.Width = 250;
            
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
        #endregion



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
            LoadItemCache();
            LoadInfo();
            CreateItems();
            CreateClassItem();
            
            LoadClassItemCache();
            BindUIModleInfo(_classUIConfig, _classInfo);
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
            if (_propertyUIConfig != null && _currentItem != null)
            {
                BindUIModleInfo(_propertyUIConfig, _currentItem);
            }
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            SaveItemInfos();
            SaveClassItemInfo();

            if (gvProject.CurrentRow != null)
            {
                UIProject project = gvProject.CurrentRow.DataBoundItem as Project;
                if (project != null) 
                {
                    
                }
            }
        }
        #region ����ͼ���
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
        /// ������ѡ����Ϣ
        /// </summary>
        private void SaveItemInfos() 
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("root");
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
            string directory = ModelPath + "gencache\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                File.SetAttributes(directory, FileAttributes.Hidden);
            }
            string fileName = directory + "\\" + _curEntityInfo.FullName + ".cache.xml";
            doc.Save(fileName);
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        private void SaveClassItemInfo() 
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("root");
            doc.AppendChild(root);
            Dictionary<string,object> div = _classInfo.CheckItem;
            foreach (KeyValuePair<string, object> kvp in div)
            {
                XmlNode inode = doc.CreateElement("item");
                XmlAttribute att = doc.CreateAttribute("name");
                att.InnerText = kvp.Key;
                inode.Attributes.Append(att);

                string value = null;
                if (kvp.Value is bool)
                {
                    value = (bool)kvp.Value ? "1" : "0";
                }
                else
                {
                    value = kvp.Value as string;
                }
                att = doc.CreateAttribute("value");
                att.InnerText = value;
                inode.Attributes.Append(att);
                root.AppendChild(inode);
            }

            string directory = ModelPath + "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string fileName = directory + "\\classinfo.cache.xml";
            doc.Save(fileName);
        }

        /// <summary>
        /// ������������Ϣ
        /// </summary>
        private void LoadClassItemCache() 
        {
            _classInfo = new UIModelItem();
            string fileName =ModelPath+ "\\classinfo.cache.xml";
            if (!File.Exists(fileName)) 
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch { return; }
            XmlNodeList nodes = doc.GetElementsByTagName("item");
            foreach(XmlNode node in nodes ) 
            {
                
                XmlAttribute attName = node.Attributes["name"];
                XmlAttribute attValue = node.Attributes["value"];
                if (attName != null && attValue!=null) 
                {
                    _classInfo.CheckItem[attName.InnerText] = attValue.InnerText;
                }
            }
        }
        /// <summary>
        /// ������������Ϣ
        /// </summary>
        private void LoadItemCache()
        {
            string directory = ModelPath + "gencache\\";
            string fileName = directory + _curEntityInfo.FullName + ".cache.xml";

            if (!File.Exists(fileName))
            {
                return;
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(fileName);
            }
            catch { return; }
            XmlNodeList nodes = doc.GetElementsByTagName("modelitem");

            

            foreach (XmlNode node in nodes)
            {
                XmlAttribute att=node.Attributes["name"];
                if(att==null)
                {
                    continue;
                }
                string name=att.InnerText;
                if(name==null)
                {
                    continue;
                }
                foreach (UIModelItem item in _curEntityInfo.Propertys) 
                {
                    if (item.PropertyName == name) 
                    {
                        item.ReadItem(node);
                        item.IsGenerate = true;
                    }
                }
                
            }
        }
        /// <summary>
        /// ��UI��Ϣ
        /// </summary>
        /// <param name="dicControl"></param>
        /// <param name="item"></param>
        private void BindUIModleInfo(Dictionary<string, EditorBase> dicControl, UIModelItem item) 
        {
            
            Dictionary<string, object> dic=item.CheckItem;

            List<EditorBase> lstEditor = new List<EditorBase>(dicControl.Count);
            foreach (KeyValuePair<string, EditorBase> kvp in dicControl) 
            {
                lstEditor.Add(kvp.Value);
            }
            object value=null;
            foreach (EditorBase editor in lstEditor) 
            {
                string key = editor.BindPropertyName;
                if (item.CheckItem.TryGetValue(key, out value))
                {
                    editor.Value = value;
                }
                else 
                {
                    editor.Reset();
                }
            }

        }

        

        #endregion
        private void labInfo_Click(object sender, EventArgs e)
        {

        }

        

    }
}