namespace Buffalo.DBTools.UIHelper
{
    partial class FrmUIGenerater
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUIGenerater));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gvProject = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRefreash = new System.Windows.Forms.DataGridViewButtonColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGen = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.labInfo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gvMember = new System.Windows.Forms.DataGridView();
            this.ColCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel6 = new System.Windows.Forms.Panel();
            this.pnlClassConfig = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvProject)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMember)).BeginInit();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvProject);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btnGen);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 467);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(697, 71);
            this.panel1.TabIndex = 0;
            // 
            // gvProject
            // 
            this.gvProject.AllowUserToAddRows = false;
            this.gvProject.AllowUserToDeleteRows = false;
            this.gvProject.AllowUserToResizeColumns = false;
            this.gvProject.AllowUserToResizeRows = false;
            this.gvProject.BackgroundColor = System.Drawing.Color.White;
            this.gvProject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvProject.ColumnHeadersVisible = false;
            this.gvProject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.colRefreash});
            this.gvProject.Dock = System.Windows.Forms.DockStyle.Left;
            this.gvProject.Location = new System.Drawing.Point(0, 0);
            this.gvProject.MultiSelect = false;
            this.gvProject.Name = "gvProject";
            this.gvProject.RowHeadersVisible = false;
            this.gvProject.RowTemplate.Height = 23;
            this.gvProject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvProject.Size = new System.Drawing.Size(184, 71);
            this.gvProject.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn3.HeaderText = "项目名";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // colRefreash
            // 
            this.colRefreash.HeaderText = "刷新";
            this.colRefreash.Name = "colRefreash";
            this.colRefreash.Text = "刷新";
            this.colRefreash.UseColumnTextForButtonValue = true;
            this.colRefreash.Width = 80;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(601, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(520, 23);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(75, 36);
            this.btnGen.TabIndex = 0;
            this.btnGen.Text = "生成";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(697, 408);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tabPanel);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(184, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(513, 408);
            this.panel4.TabIndex = 1;
            // 
            // tabPanel
            // 
            this.tabPanel.ColumnCount = 2;
            this.tabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPanel.Location = new System.Drawing.Point(0, 23);
            this.tabPanel.Name = "tabPanel";
            this.tabPanel.RowCount = 2;
            this.tabPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.338028F));
            this.tabPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 93.66197F));
            this.tabPanel.Size = new System.Drawing.Size(513, 385);
            this.tabPanel.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.labInfo);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(513, 23);
            this.panel5.TabIndex = 1;
            // 
            // labInfo
            // 
            this.labInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labInfo.Font = new System.Drawing.Font("宋体", 12F);
            this.labInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labInfo.Location = new System.Drawing.Point(0, 0);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(513, 23);
            this.labInfo.TabIndex = 0;
            this.labInfo.Text = "  ";
            this.labInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labInfo.Click += new System.EventHandler(this.labInfo_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gvMember);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(184, 408);
            this.panel3.TabIndex = 0;
            // 
            // gvMember
            // 
            this.gvMember.AllowUserToAddRows = false;
            this.gvMember.AllowUserToDeleteRows = false;
            this.gvMember.AllowUserToResizeColumns = false;
            this.gvMember.AllowUserToResizeRows = false;
            this.gvMember.BackgroundColor = System.Drawing.Color.White;
            this.gvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColCheck,
            this.ColName});
            this.gvMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMember.Location = new System.Drawing.Point(0, 0);
            this.gvMember.MultiSelect = false;
            this.gvMember.Name = "gvMember";
            this.gvMember.RowHeadersVisible = false;
            this.gvMember.RowTemplate.Height = 23;
            this.gvMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMember.Size = new System.Drawing.Size(184, 408);
            this.gvMember.TabIndex = 0;
            this.gvMember.CurrentCellChanged += new System.EventHandler(this.gvMember_CurrentCellChanged);
            // 
            // ColCheck
            // 
            this.ColCheck.DataPropertyName = "IsGenerate";
            this.ColCheck.HeaderText = "";
            this.ColCheck.Name = "ColCheck";
            this.ColCheck.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ColCheck.Width = 50;
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.DataPropertyName = "PropertyName";
            this.ColName.HeaderText = "属性";
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "属性";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.pnlClassConfig);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(697, 59);
            this.panel6.TabIndex = 2;
            // 
            // pnlClassConfig
            // 
            this.pnlClassConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClassConfig.Location = new System.Drawing.Point(0, 0);
            this.pnlClassConfig.Name = "pnlClassConfig";
            this.pnlClassConfig.Size = new System.Drawing.Size(697, 59);
            this.pnlClassConfig.TabIndex = 0;
            // 
            // FrmUIGenerater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 538);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmUIGenerater";
            this.Text = "界面生成";
            this.Load += new System.EventHandler(this.FrmUIGenerater_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvProject)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMember)).EndInit();
            this.panel6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView gvMember;
        private System.Windows.Forms.TableLayoutPanel tabPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridView gvProject;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewButtonColumn colRefreash;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labInfo;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.FlowLayoutPanel pnlClassConfig;

    }
}