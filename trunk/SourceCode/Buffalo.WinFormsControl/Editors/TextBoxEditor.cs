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
    /// ÎÄ±¾¿òµÄ±à¼­Æ÷
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

        public override Label Lable
        {
            get
            {
                return labSummary;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            DoValueChange(sender, txtValue.Text);
        }



    }
}
