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
        /// 热键的标识
        /// </summary>
        public int Id
        {
            get { return id; }
        }
        KeyModifiers modifierKey = KeyModifiers.None;

        /// <summary>
        /// 屏蔽键
        /// </summary>
        public KeyModifiers ModifierKey
        {
            get { return modifierKey; }
        }
        Keys key = Keys.None;

        /// <summary>
        /// 按键
        /// </summary>
        public Keys Key
        {
            get { return key; }
        }

        Control bindControl;

        /// <summary>
        /// 绑定的控件s
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
        bool isRegistered = false;//是否已经注册

        /// <summary>
        /// 是否已经注册了热键
        /// </summary>
        public bool IsRegistered
        {
            get { return isRegistered; }
        }

        /// <summary>
        /// 注册此热键
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
        /// 卸载此热键热键
        /// </summary>
        public void UnRegister()
        {
            if (isRegistered)
            {
                WindowsAPI.UnregisterHotKey(bindControl.Handle, id);
                isRegistered = false;
            }
        }




        #region IDisposable 成员

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


/*=============例子================
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