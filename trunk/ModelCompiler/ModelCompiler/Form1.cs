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
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>[^<]+)</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(content);

            foreach (Match ma in matches) 
            {
                MessageBox.Show(ma.Groups["type"].Value);
            }
        }
    }
}