using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using EnvDTE;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.IO;
using Buffalo.DBTools.HelperKernel;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;

namespace Buffalo.DBTools
{
    public partial class FrmClassDesigner : Form
    {
        public FrmClassDesigner()
        {
            InitializeComponent();
        }
        private ClrTypeShape _selectedClass = null;

        /// <summary>
        /// 选中的类
        /// </summary>
        public ClrTypeShape SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; }
        }
        private Diagram _selectedDiagram = null;

        /// <summary>
        /// 选中的关系图
        /// </summary>
        public Diagram SelectedDiagram
        {
            get { return _selectedDiagram; }
            set { _selectedDiagram = value; }
        }
        private Project _currentProject;
        /// <summary>
        /// 当前项目
        /// </summary>
        public Project CurrentProject
        {
            get { return _currentProject; }
            set { _currentProject = value; }
        }
        ClassDesignerDocView _selectDocView;
        /// <summary>
        /// 选择的文档
        /// </summary>
        public ClassDesignerDocView SelectDocView
        {
            get { return _selectDocView; }
            set { _selectDocView = value; }
        }
        GridViewComboBoxCell _cmbCell = null;
        GridViewComboBoxCell _relationCell = null;
        EntityConfig _config = null;
        private void FrmClassDesigner_Load(object sender, EventArgs e)
        {
            _cmbCell = new GridViewComboBoxCell(gvField);
            _relationCell = new GridViewComboBoxCell(gvMapping);
            gvField.AutoGenerateColumns = false;
            gvMapping.AutoGenerateColumns = false;
            _config = new EntityConfig(SelectedClass.AssociatedType, CurrentProject, SelectedDiagram);
            _config.SelectDocView = SelectDocView;
            BindFieldInfos();
            //_cmbCell.SetDataSource(EntityFieldBase.GetAllSupportTypes());
            gvMapping.CurrentCellChanged += new EventHandler(gvMapping_CurrentCellChanged);
            gvField.CurrentCellChanged+=new EventHandler(gvField_CurrentCellChanged);
        }

        void gvMapping_CurrentCellChanged(object sender, EventArgs e)
        {
            if (gvMapping.CurrentCell == null) 
            {
                return;
            }
            string colName = gvMapping.Columns[gvMapping.CurrentCell.ColumnIndex].Name;
            if (colName == "ColSource") 
            {
                _relationCell.SetDataSource(_config.AllPropertyNames);
                _relationCell.ShowComboBox(gvMapping.CurrentCell);
            }
            else if (colName == "ColTarget")
            {
                EntityRelationItem erf = gvMapping.Rows[gvMapping.CurrentCell.RowIndex].DataBoundItem as EntityRelationItem;
                if (erf != null)
                {
                    _relationCell.SetDataSource(erf.TargetPropertyList);
                    _relationCell.ShowComboBox(gvMapping.CurrentCell);
                }
            }
        }

        private void BindFieldInfos() 
        {
            txtClassName.Text = _config.ClassName;
            txtTableName.Text = _config.TableName;
            
                txtBaseClass.Text = _config.BaseTypeName;
            
            gvField.DataSource = _config.EParamFields;
            gvMapping.DataSource = _config.ERelation;
        }

        private void gvField_CurrentCellChanged(object sender, EventArgs e)
        {
            if (gvField.CurrentCell == null)
            {
                return;
            }
            string colName=gvField.Columns[gvField.CurrentCell.ColumnIndex].Name;
            if (colName == "ColParamType") 
            {
                EntityParamField epf=gvField.Rows[gvField.CurrentCell.RowIndex].DataBoundItem as EntityParamField;
                if (epf != null)
                {
                    DataTypeInfos info = EntityFieldBase.GetTypeInfo(epf.FInfo);
                    if (info != null)
                    {
                        _cmbCell.SetDataSource(info.DbTypes);
                        _cmbCell.ShowComboBox(gvField.CurrentCell);

                    }
                    
                }
            }

            if (colName == "ColPropertyType")
            {
                EntityParamField epf = gvField.Rows[gvField.CurrentCell.RowIndex].DataBoundItem as EntityParamField;
                if (epf != null)
                {
                    DataTypeInfos info = EntityFieldBase.GetTypeInfo(epf.FInfo);
                    if (info != null)
                    {
                        _cmbCell.SetDataSource(EntityFieldBase.PropertyTypeItems);
                        _cmbCell.ShowComboBox(gvField.CurrentCell);
                    }

                }
            }
        }

        private void btnGenCode_Click(object sender, EventArgs e)
        {
            //_config.BaseType = txtBaseClass.Text;
            _config.TableName = txtTableName.Text;
            _config.GenerateCode();
            this.DialogResult = DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        

    }
}