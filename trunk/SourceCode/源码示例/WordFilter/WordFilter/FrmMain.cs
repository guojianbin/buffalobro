using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel.Win32;


namespace WordFilter
{
    public partial class FrmMain : Form
    {
        HotKey _hotKey;
        
        bool _visable = false;
        WordPicture _wp;
        QRCodeUnit _qrcode;

        private ToolStripMenuItem[] _toolItems;

        const int WM_DRAWCLPBOARD = 0x308;
        const int WM_CHANGCBCHAIN = 0X030D;
        const int WM_HOTKEY = 0x312;
        internal bool _isSys = false;//�Ƿ�ϵͳ����
        IntPtr _hNextClipboardViewer;//��һ�����ӵĴ���

        public FrmMain()
        {
            

            _hotKey = new HotKey(1, KeyModifiers.None, Keys.F6, this);
            _hotKey.Register();
            _wp = new WordPicture();
            _wp.Fcolor = Color.Black;
            _wp.LineAlpha = 200;
            _wp.Font = new Font("����", 12, FontStyle.Bold);
            _qrcode = new QRCodeUnit();
            
            InitializeComponent();
            _toolItems = new ToolStripMenuItem[] { itemFont, itemQRCode, itemQRCodeEncry };
            _hNextClipboardViewer = WindowsAPI.SetClipboardViewer(this.Handle);
            InitSelectItem();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override void WndProc(ref Message m)
        {
           
            if (m.Msg == WM_HOTKEY)
            {
                try
                {
                    _isSys = true;
                    Clipboard.Clear();
                    
                    SendKeys.SendWait("^A");
                    SendKeys.SendWait("^C");


                    if (Clipboard.ContainsText())
                    {
                        string str = (String)Clipboard.GetData(DataFormats.Text);
                        if (str != null)
                        {
                            try
                            {

                                Image img = GetPicture(str);
                                if (img != null)
                                {
                                    Clipboard.SetImage(img);
                                    SendKeys.SendWait("^V");
                                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                                    notifyIcon1.BalloonTipText = str;
                                    notifyIcon1.BalloonTipTitle = "�Ѿ�ת������";
                                    notifyIcon1.ShowBalloonTip(1000);
                                    
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    
                }
                finally 
                {
                    _isSys = false;
                }
            }
            else if (m.Msg == WM_DRAWCLPBOARD)
            {
                if (!_isSys)
                {
                    string str = _qrcode.GetQRCodeString();
                    if (!string.IsNullOrEmpty(str))
                    {
                        FrmQRResault.ShowBox(str);
                    }
                    WindowsAPI.SendMessage(_hNextClipboardViewer, (Msg)m.Msg, m.WParam, m.LParam);
                }
            }
            else if (m.Msg == WM_CHANGCBCHAIN)
            {
                if (_hNextClipboardViewer == m.WParam)
                {//����Ҫ������Ϣ����һ�����ڵľ��
                    _hNextClipboardViewer = m.LParam;
                }
                else
                {
                    WindowsAPI.SendMessage(_hNextClipboardViewer, (Msg)m.Msg, m.WParam, m.LParam);
                }
            }
            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_visable);
        }

        /// <summary>
        /// ��ȡͼƬ
        /// </summary>
        /// <returns></returns>
        private Bitmap GetPicture(string str) 
        {
            if (itemFont.Checked) 
            {
                return  _wp.DrawWordPicture(str);
            }
            else if (itemQRCode.Checked) 
            {
                return _qrcode.GetQRCode(str);
            }
            else if (itemQRCodeEncry.Checked) 
            {
                return _qrcode.GetEncryQRCode(str);
            }
            return _wp.DrawWordPicture(str);
        }
        

        private void �˳�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //kbListener.StopListener();
            SaveSelectItem();
            WindowsAPI.ChangeClipboardChain(this.Handle, _hNextClipboardViewer);
            _hotKey.UnRegister();
            Application.Exit();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ֹͣ��ؼ���ToolStripMenuItem.Checked = _hotKey.IsRegistered;
        }

        private void ֹͣ��ؼ���ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (kbListener.IsListening)
            //{
            //    kbListener.StopListener();
            //}
            //else 
            //{
            //    kbListener.StartListener();
            //}

            if (_hotKey.IsRegistered)
            {
                _hotKey.UnRegister();
            }
            else 
            {
                _hotKey.Register();
            }
        }

        /// <summary>
        /// ���ѡ�е���
        /// </summary>
        private void ClearSelectItem() 
        {
            foreach (ToolStripMenuItem item in _toolItems)
            {
                item.Checked = false;
            }
        }

        private void itemType_Click(object sender, EventArgs e)
        {
            ClearSelectItem();
            ToolStripMenuItem cur = sender as ToolStripMenuItem;
            if (cur != null) 
            {
                cur.Checked = true;
                
            }
        }

        /// <summary>
        /// ����ѡ�е���
        /// </summary>
        private void SaveSelectItem() 
        {
            for (int i = 0; i < _toolItems.Length; i++)
            {
                if (_toolItems[i].Checked)
                {
                    try
                    {
                        ConfigSave.SaveConfig(i);
                    }
                    catch { }
                    break;
                }
            }
        }

        /// <summary>
        /// ��ʼ��ѡ�е��������
        /// </summary>
        private void InitSelectItem() 
        {
            
            try
            {
                int index = ConfigSave.ReadConfig();
                ClearSelectItem();
                _toolItems[index].Checked = true; ;
            }
            catch { }
        }
    }
}