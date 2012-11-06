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
            string content = File.ReadAllText(@"E:\CShareTest\Buffalobro\ModelCompiler\model.txt");
            Compiler com = new Compiler(content);
            Queue<ExpressionItem> item = com.ExpressionItems;
        }
    }
}