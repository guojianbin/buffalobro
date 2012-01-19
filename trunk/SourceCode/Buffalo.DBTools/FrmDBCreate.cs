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
using Buffalo.DBTools.HelperKernel;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;
using Buffalo.DB.DBCheckers;

namespace Buffalo.DBTools
{
    public partial class FrmDBCreate : Form
    {
        public FrmDBCreate()
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
        private void FrmDBCreate_Load(object sender, EventArgs e)
        {
            if (SelectedClass != null) 
            {
                GetClassSQL();
            }
        }
        
        /// <summary>
        /// 获取类的创建语句
        /// </summary>
        /// <param name="type"></param>
        private void GetClassSQL() 
        {
            Stack<EntityConfig> stkConfig = new Stack<EntityConfig>();
            EntityConfig entity = null;
            DBConfigInfo dbcinfo = FrmDBSetting.GetDBConfigInfo(entity,CurrentProject, SelectedDiagram);
            DBInfo dbInfo = dbcinfo.CreateDBInfo();
            ClrType curType=SelectedClass.AssociatedType;//当前类型
            string typeName=null;
            while(curType!=null)
            {
                entity = new EntityConfig(curType, CurrentProject, SelectedDiagram);
                
                stkConfig.Push(entity);
                curType = EntityConfig.GetBaseClass(curType, out typeName);
                if (EntityConfig.IsSystemType(curType)) 
                {
                    break;
                }
            }

            
            List<KeyWordTableParamItem> lstTable = new List<KeyWordTableParamItem>();
            List<TableParamItemInfo> lstParam=new List<TableParamItemInfo>();
            string lastTableName = null;
            while (stkConfig.Count > 0) 
            {
                EntityConfig centity = stkConfig.Pop();
                FillParams(centity, lstParam);
                lastTableName = centity.TableName;
            }
            KeyWordTableParamItem table = new KeyWordTableParamItem(lstParam,lastTableName,null);
            lstTable.Add(table);
            try
            {
                string sql = TableChecker.CheckTable(dbInfo, lstTable);
                rtbContent.Text = sql;
            }
            catch (Exception ex) 
            {
                MessageBox.Show("生成语句失败:" + ex.Message);
            }
            
        }

        /// <summary>
        /// 填充字段信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="lstParam"></param>
        private void FillParams(EntityConfig entity, List<TableParamItemInfo> lstParam) 
        {
            foreach (EntityParamField param in entity.EParamFields) 
            {
                if (!param.IsGenerate) 
                {
                    continue;
                }
                DbType dbt=(DbType)EnumUnit.GetEnumInfoByName(typeof(DbType),param.DbType).Value;
                TableParamItemInfo pInfo = new TableParamItemInfo(
                    param.ParamName,dbt , true, 
                    param.EntityPropertyType, param.Length);

                lstParam.Add(pInfo);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string sql = rtbContent.Text;
            DBConfigInfo dbcinfo = FrmDBSetting.GetDBConfigInfo(null, CurrentProject, SelectedDiagram);
            DBInfo dbInfo = dbcinfo.CreateDBInfo();
            try
            {
                dbInfo.DefaultOperate.Execute(sql, new Buffalo.DB.DbCommon.ParamList());
                rtbOutput.Text = "执行完毕";
            }
            catch (Exception ex)
            {
                rtbOutput.Text = "错误：" + ex.Message;
            }

        }

    }
}