using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Buffalo.DBTools
{
    public partial class FrmProcess : Form
    {
        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <returns></returns>
        public static FrmProcess ShowProcess() 
        {
            FrmProcess frm = new FrmProcess();
            frm.Show();
            return frm;
        }
    


        public bool IsSysExit;
        public delegate void CloseHandle();
        public event CloseHandle OnClose;
        public bool CanClose
        {
            get { return button1.Visible; }
            set { button1.Visible = value; }
        }
        public FrmProcess()
        {
            InitializeComponent();
        }

        public void UpdateProgress(int value,string msg)
        {
            UpdateProgress(value, 100, msg);
        }

        public void UpdateProgress(int value, int total,string msg)
        {
            if (value > total) value = total;
            progressBar1.Maximum = total;
            progressBar1.Value = value;
            label1.Text = msg;
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            if (OnClose != null)
            {
                OnClose();
            }
        }
        
        private void SalesSumProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsSysExit)
            {
                if (MessageBox.Show("操作正在进行,确定退出吗?", "游卡通", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (Brush backbrush = new LinearGradientBrush(new Point(0, 0), new Point(0, panel1.Height), Color.White, SystemColors.Control))
            {
                e.Graphics.FillRectangle(backbrush, 0, 0, panel1.Width, panel1.Height);
            }
        }
    }
}
