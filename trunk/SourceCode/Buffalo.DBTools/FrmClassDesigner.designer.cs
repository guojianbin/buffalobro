namespace Buffalo.DBTools
{
    partial class FrmClassDesigner
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBaseClass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbEntity = new System.Windows.Forms.TabControl();
            this.tpEntity = new System.Windows.Forms.TabPage();
            this.gvField = new System.Windows.Forms.DataGridView();
            this.ColChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColFName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProperty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColParam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColParamType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPropertyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpMapping = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gvMapping = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.btnGenCode = new System.Windows.Forms.Button();
            this.ColSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropertyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIsToDB = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tbEntity.SuspendLayout();
            this.tpEntity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvField)).BeginInit();
            this.tpMapping.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMapping)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtBaseClass);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtTableName);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtClassName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(797, 44);
            this.panel1.TabIndex = 0;
            // 
            // txtBaseClass
            // 
            this.txtBaseClass.Location = new System.Drawing.Point(467, 12);
            this.txtBaseClass.Name = "txtBaseClass";
            this.txtBaseClass.ReadOnly = true;
            this.txtBaseClass.Size = new System.Drawing.Size(160, 21);
            this.txtBaseClass.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(430, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "父类:";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(243, 12);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(160, 21);
            this.txtTableName.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(193, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "所属表:";
            // 
            // txtClassName
            // 
            this.txtClassName.BackColor = System.Drawing.SystemColors.Control;
            this.txtClassName.Location = new System.Drawing.Point(50, 12);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.ReadOnly = true;
            this.txtClassName.Size = new System.Drawing.Size(126, 21);
            this.txtClassName.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "类名:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(797, 495);
            this.panel2.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbEntity);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(797, 495);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "配置实体";
            // 
            // tbEntity
            // 
            this.tbEntity.Controls.Add(this.tpEntity);
            this.tbEntity.Controls.Add(this.tpMapping);
            this.tbEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEntity.Location = new System.Drawing.Point(3, 17);
            this.tbEntity.Name = "tbEntity";
            this.tbEntity.SelectedIndex = 0;
            this.tbEntity.Size = new System.Drawing.Size(791, 439);
            this.tbEntity.TabIndex = 26;
            // 
            // tpEntity
            // 
            this.tpEntity.Controls.Add(this.gvField);
            this.tpEntity.Location = new System.Drawing.Point(4, 22);
            this.tpEntity.Name = "tpEntity";
            this.tpEntity.Padding = new System.Windows.Forms.Padding(3);
            this.tpEntity.Size = new System.Drawing.Size(783, 413);
            this.tpEntity.TabIndex = 0;
            this.tpEntity.Text = "实体字段";
            this.tpEntity.UseVisualStyleBackColor = true;
            // 
            // gvField
            // 
            this.gvField.AllowUserToAddRows = false;
            this.gvField.AllowUserToDeleteRows = false;
            this.gvField.AllowUserToResizeColumns = false;
            this.gvField.AllowUserToResizeRows = false;
            this.gvField.BackgroundColor = System.Drawing.Color.White;
            this.gvField.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvField.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColChecked,
            this.ColFName,
            this.ColType,
            this.ColProperty,
            this.ColParam,
            this.ColParamType,
            this.ColLength,
            this.ColPropertyType});
            this.gvField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvField.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvField.Location = new System.Drawing.Point(3, 3);
            this.gvField.Name = "gvField";
            this.gvField.RowHeadersVisible = false;
            this.gvField.RowTemplate.Height = 23;
            this.gvField.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvField.Size = new System.Drawing.Size(777, 407);
            this.gvField.TabIndex = 27;
            // 
            // ColChecked
            // 
            this.ColChecked.DataPropertyName = "IsGenerate";
            this.ColChecked.HeaderText = "选中";
            this.ColChecked.Name = "ColChecked";
            this.ColChecked.Width = 50;
            // 
            // ColFName
            // 
            this.ColFName.DataPropertyName = "FieldName";
            this.ColFName.HeaderText = "变量名";
            this.ColFName.Name = "ColFName";
            this.ColFName.Width = 113;
            // 
            // ColType
            // 
            this.ColType.DataPropertyName = "FieldType";
            this.ColType.HeaderText = "变量类型";
            this.ColType.Name = "ColType";
            this.ColType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColType.Width = 112;
            // 
            // ColProperty
            // 
            this.ColProperty.DataPropertyName = "PropertyName";
            this.ColProperty.HeaderText = "属性名";
            this.ColProperty.Name = "ColProperty";
            // 
            // ColParam
            // 
            this.ColParam.DataPropertyName = "ParamName";
            this.ColParam.HeaderText = "字段名";
            this.ColParam.Name = "ColParam";
            // 
            // ColParamType
            // 
            this.ColParamType.DataPropertyName = "DbType";
            this.ColParamType.HeaderText = "字段类型";
            this.ColParamType.Name = "ColParamType";
            this.ColParamType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ColLength
            // 
            this.ColLength.DataPropertyName = "Length";
            this.ColLength.HeaderText = "长度";
            this.ColLength.Name = "ColLength";
            // 
            // ColPropertyType
            // 
            this.ColPropertyType.DataPropertyName = "PropertyType";
            this.ColPropertyType.HeaderText = "类型";
            this.ColPropertyType.Name = "ColPropertyType";
            // 
            // tpMapping
            // 
            this.tpMapping.Controls.Add(this.panel4);
            this.tpMapping.Location = new System.Drawing.Point(4, 22);
            this.tpMapping.Name = "tpMapping";
            this.tpMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tpMapping.Size = new System.Drawing.Size(783, 413);
            this.tpMapping.TabIndex = 1;
            this.tpMapping.Text = "实体映射";
            this.tpMapping.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gvMapping);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(777, 407);
            this.panel4.TabIndex = 1;
            // 
            // gvMapping
            // 
            this.gvMapping.AllowUserToAddRows = false;
            this.gvMapping.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvMapping.BackgroundColor = System.Drawing.Color.White;
            this.gvMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMapping.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSelect,
            this.ColField,
            this.colPropertyName,
            this.colPType,
            this.ColSource,
            this.ColTarget,
            this.ColIsToDB});
            this.gvMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMapping.Location = new System.Drawing.Point(0, 0);
            this.gvMapping.MultiSelect = false;
            this.gvMapping.Name = "gvMapping";
            this.gvMapping.RowHeadersVisible = false;
            this.gvMapping.RowTemplate.Height = 23;
            this.gvMapping.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMapping.Size = new System.Drawing.Size(777, 407);
            this.gvMapping.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.btnGenCode);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 456);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(791, 36);
            this.panel3.TabIndex = 25;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(707, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 24;
            this.button3.Text = "关闭";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnGenCode
            // 
            this.btnGenCode.Location = new System.Drawing.Point(626, 6);
            this.btnGenCode.Name = "btnGenCode";
            this.btnGenCode.Size = new System.Drawing.Size(75, 23);
            this.btnGenCode.TabIndex = 22;
            this.btnGenCode.Text = "生成类";
            this.btnGenCode.UseVisualStyleBackColor = true;
            this.btnGenCode.Click += new System.EventHandler(this.btnGenCode_Click);
            // 
            // ColSelect
            // 
            this.ColSelect.DataPropertyName = "IsGenerate";
            this.ColSelect.HeaderText = "选中";
            this.ColSelect.Name = "ColSelect";
            this.ColSelect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColSelect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColField
            // 
            this.ColField.DataPropertyName = "FieldName";
            this.ColField.HeaderText = "变量名";
            this.ColField.Name = "ColField";
            // 
            // colPropertyName
            // 
            this.colPropertyName.DataPropertyName = "PropertyName";
            this.colPropertyName.HeaderText = "属性名";
            this.colPropertyName.Name = "colPropertyName";
            // 
            // colPType
            // 
            this.colPType.DataPropertyName = "FieldType";
            this.colPType.HeaderText = "变量类型";
            this.colPType.Name = "colPType";
            // 
            // ColSource
            // 
            this.ColSource.DataPropertyName = "SourceProperty";
            this.ColSource.HeaderText = "源实体属性";
            this.ColSource.Name = "ColSource";
            // 
            // ColTarget
            // 
            this.ColTarget.DataPropertyName = "TargetProperty";
            this.ColTarget.HeaderText = "目标实体属性";
            this.ColTarget.Name = "ColTarget";
            // 
            // ColIsToDB
            // 
            this.ColIsToDB.DataPropertyName = "IsToDB";
            this.ColIsToDB.HeaderText = "生成到数据库";
            this.ColIsToDB.Name = "ColIsToDB";
            // 
            // FrmClassDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 539);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmClassDesigner";
            this.Text = "Buffalo助手--类生成";
            this.Load += new System.EventHandler(this.FrmClassDesigner_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tbEntity.ResumeLayout(false);
            this.tpEntity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvField)).EndInit();
            this.tpMapping.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvMapping)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtBaseClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tbEntity;
        private System.Windows.Forms.TabPage tpEntity;
        private System.Windows.Forms.DataGridView gvField;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColParam;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColParamType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPropertyType;
        private System.Windows.Forms.TabPage tpMapping;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnGenCode;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView gvMapping;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColField;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropertyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTarget;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColIsToDB;


    }
}