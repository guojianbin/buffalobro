using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.Editors
{
    /// <summary>
    /// 文本框的编辑器
    /// </summary>
    [ToolboxItem(true)]
    public partial class TextBoxEditor : EditorBase
    {
        public TextBoxEditor()
        {
            InitializeComponent();
        }

        private void TextBoxEditor_Load(object sender, EventArgs e)
        {
            
        }

        private void pnlValue_Resize(object sender, EventArgs e)
        {
            int width = pnlValue.Width - 4;
            if (width > 0) 
            {
                txtValue.Width = width;
            }
        }
        public override string  Text
        {
	        get 
	        { 
		         return txtValue.Text;
	        }
	          set 
	        { 
		        txtValue.Text = value;
	        }
        }

        public override int LableWidth
        {
            get
            {
                return pnlLable.Width;
            }
            set
            {
                pnlLable.Width = value;
            }
        }
        public override Label Lable
        {
            get
            {
                return labSummary;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (txtValue.Multiline) 
            {
                txtValue.Height = this.Height - 4;
            }
        }

        /// <summary>
        /// 是否支持多行
        /// </summary>
        public bool Multiline 
        {
            get 
            {
                return txtValue.Multiline;
            }
            set 
            {
                txtValue.Multiline = value;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            DoValueChange( txtValue.Text);
        }



    }
}
