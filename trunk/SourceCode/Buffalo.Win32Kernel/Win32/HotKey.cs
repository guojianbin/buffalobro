using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.Win32;

namespace Buffalo.Win32Kernel.Win32
{
    public class HotKey:IDisposable
    {
        int id = -1;

        /// <summary>
        /// �ȼ��ı�ʶ
        /// </summary>
        public int Id
        {
            get { return id; }
        }
        KeyModifiers modifierKey = KeyModifiers.None;

        /// <summary>
        /// ���μ�
        /// </summary>
        public KeyModifiers ModifierKey
        {
            get { return modifierKey; }
        }
        Keys key = Keys.None;

        /// <summary>
        /// ����
        /// </summary>
        public Keys Key
        {
            get { return key; }
        }

        Control bindControl;

        /// <summary>
        /// �󶨵Ŀؼ�s
        /// </summary>
        public Control BindControl
        {
          get { return bindControl; }
        }

        public HotKey(int id, KeyModifiers modifierKey, Keys key,Control bindControl) 
        {
            this.id = id;
            this.modifierKey = modifierKey;
            this.key = key;
            this.bindControl=bindControl;
        }
        bool isRegistered = false;//�Ƿ��Ѿ�ע��

        /// <summary>
        /// �Ƿ��Ѿ�ע�����ȼ�
        /// </summary>
        public bool IsRegistered
        {
            get { return isRegistered; }
        }

        /// <summary>
        /// ע����ȼ�
        /// </summary>
        public void Register()
        {
            if (!isRegistered)
            {
                WindowsAPI.RegisterHotKey(bindControl.Handle, id, (uint)modifierKey, (uint)key);
                isRegistered = true;
            }
        }

        /// <summary>
        /// ж�ش��ȼ��ȼ�
        /// </summary>
        public void UnRegister()
        {
            if (isRegistered)
            {
                WindowsAPI.UnregisterHotKey(bindControl.Handle, id);
                isRegistered = false;
            }
        }




        #region IDisposable ��Ա

        public void Dispose()
        {
            UnRegister();
            GC.SuppressFinalize(this);
        }

        #endregion

        ~HotKey() 
        {
            UnRegister();
        }
    }
}


/*=============����================
public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HotKey objHotKey;
        private void Form1_Load(object sender, EventArgs e)
        {
            objHotKey = new HotKey(123, KeyModifiers.Control | KeyModifiers.Alt, Keys.W, this);
            objHotKey.Register();

            
        }
        
        protected override void DefWndProc(ref Message m)
        {
            
            base.DefWndProc(ref m);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)Msg.WM_HOTKEY)
            {
                int msgId = m.WParam.ToInt32();
                if (msgId == 123) 
                {
                    MessageBox.Show("fuck");
                    this.Close();
                }

            }
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            objHotKey.UnRegister();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }
*/