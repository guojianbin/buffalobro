namespace Buffalo.DBTools
{
    partial class FrmAllTables
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAllTables));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.gvTables = new System.Windows.Forms.DataGridView();
            this.ColChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColTableNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTables)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 405);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 32);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnSubmit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(353, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(214, 32);
            this.panel3.TabIndex = 2;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(127, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(46, 5);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "确定";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkAll);
            this.panel2.Controls.Add(this.gvTables);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 405);
            this.panel2.TabIndex = 1;
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.BackColor = System.Drawing.Color.Transparent;
            this.chkAll.Location = new System.Drawing.Point(3, 3);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(48, 16);
            this.chkAll.TabIndex = 1;
            this.chkAll.Text = "选择";
            this.chkAll.UseVisualStyleBackColor = false;
            this.chkAll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkAll_MouseUp);
            // 
            // gvTables
            // 
            this.gvTables.AllowUserToAddRows = false;
            this.gvTables.AllowUserToResizeColumns = false;
            this.gvTables.AllowUserToResizeRows = false;
            this.gvTables.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvTables.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColChecked,
            this.ColTableNames,
            this.ColType});
            this.gvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTables.Location = new System.Drawing.Point(0, 0);
            this.gvTables.Name = "gvTables";
            this.gvTables.RowHeadersVisible = false;
            this.gvTables.RowTemplate.Height = 23;
            this.gvTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvTables.Size = new System.Drawing.Size(567, 405);
            this.gvTables.TabIndex = 0;
            this.gvTables.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvTables_CellMouseUp);
            // 
            // ColChecked
            // 
            this.ColChecked.DataPropertyName = "IsGenerate";
            dataGridViewCellStyle2.NullValue = false;
            this.ColChecked.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColChecked.HeaderText = "选择";
            this.ColChecked.Name = "ColChecked";
            this.ColChecked.Width = 50;
            // 
            // ColTableNames
            // 
            this.ColTableNames.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColTableNames.DataPropertyName = "Name";
            this.ColTableNames.HeaderText = "对象";
            this.ColTableNames.Name = "ColTableNames";
            this.ColTableNames.ReadOnly = true;
            // 
            // ColType
            // 
            this.ColType.DataPropertyName = "ObjectTypeName";
            this.ColType.HeaderText = "类型";
            this.ColType.Name = "ColType";
            // 
            // FrmAllTables
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(567, 437);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAllTables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Buffalo助手--选择要生成的表";
            this.Load += new System.EventHandler(this.FrmAllTables_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTables)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView gvTables;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTableNames;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.CheckBox chkAll;
    }
}