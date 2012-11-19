using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace ModelCompiler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text="";
            string modelName=@"E:\CShareTest\Buffalobro\ModelCompiler\model.txt";
            string content = File.ReadAllText(modelName,System.Text.Encoding.Default);
            ModelCompiler com = new ModelCompiler(content);
            StringBuilder err = new StringBuilder(2000);
            string code=com.GetContent(err);
            if (string.IsNullOrEmpty(code))
            {
                richTextBox1.AppendText("´íÎó:\n");
                richTextBox1.AppendText(err.ToString());
            }
            else 
            {
                richTextBox1.Text = code;
            }
        }
    }
}