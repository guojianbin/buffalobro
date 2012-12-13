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

namespace Buffalo.DBTools.UIHelper
{
    public partial class FrmUIGenerater : Form
    {
        public FrmUIGenerater()
        {
            InitializeComponent();
        }
        private Project _currentProject;
        /// <summary>
        /// ��ǰ��Ŀ
        /// </summary>
        public Project CurrentProject
        {
            get { return _currentProject; }
            set { _currentProject = value; }
        }
        ClassDesignerDocView _selectDocView;
        /// <summary>
        /// ѡ����ĵ�
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get { return _selectDocView; }
            set { _selectDocView = value; }
        }
        private ClrTypeShape _selectedClass = null;

        /// <summary>
        /// ѡ�е���
        /// </summary>
        public ClrTypeShape SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; }
        }

        /// <summary>
        /// ����ļ�
        /// </summary>
        private XmlDocument LoadConfig() 
        {
            FileInfo file = new FileInfo(CurrentProject.FileName);
            string directory = file.DirectoryName;
            directory = directory+"\\.bmodels\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string xml = "UIConfigItem.xml";
            return null;
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="curProject"></param>
        /// <param name="curDiagram"></param>
        public XmlDocument GetConfigName()
        {



            return null;

        }
    }
}