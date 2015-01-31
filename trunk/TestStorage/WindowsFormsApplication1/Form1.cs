using Buffalo.Storage.LocalFileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LocalFileAdapter fa = new LocalFileAdapter(@"D:\\", "", "");
            fa.Open();
            fa.ListFiles();
            
            List<string> files = fa.ListFiles();

            fa.Close();
        }
    }
}
