using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Buffalo.WinFormsControl.Editors
{
    public partial class OnOffCheckBox : UserControl
    {
        /// <summary>
        /// 当 System.Windows.Forms.CheckBox.Checked 属性的值更改时发生。
        /// </summary>
        public event EventHandler CheckedChanged;
        /// <summary>
        /// 当 System.Windows.Forms.CheckBox.CheckState 属性的值更改时发生。
        /// </summary>
        public event EventHandler CheckStateChanged;
        public OnOffCheckBox()
        {
            InitializeComponent();
        }


        private CheckState _checkState;

        /// <summary>
        /// 获取或设置 System.Windows.Forms.CheckBox 的状态。
        /// </summary>
        [Bindable(true)]
        public CheckState CheckState
        {
            get
            {
                return _checkState;
            }
            set
            {
                _checkState = value;
                if (CheckStateChanged != null)
                {
                    CheckStateChanged(this, new EventArgs());
                }
                RefreashDisplay();
            }
        }




        /// <summary>
        ///  获取或设置一个值，该值指示 System.Windows.Forms.CheckBox 是否处于选中状态。
        /// </summary>
        [Bindable(true)]
        [DefaultValue(false)]
        [SettingsBindable(true)]
        public bool Checked
        {
            get
            {
                return _checkState == CheckState.Checked;
            }
            set
            {
                if (value)
                {
                    _checkState = CheckState.Checked;
                }
                else
                {
                    _checkState = CheckState.Unchecked;
                }
                if (CheckedChanged != null)
                {
                    CheckedChanged(this, new EventArgs());
                }
                RefreashDisplay();
            }
        }

        private OnOffButtonType _onOffType=OnOffButtonType.Quadrate;
        /// <summary>
        /// 按钮样式类型
        /// </summary>
        public OnOffButtonType OnOffType
        {
            get { return _onOffType; }
            set 
            { 
                _onOffType = value;
                RefreashDisplay();
            }
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void RefreashDisplay()
        {
            if (pbDisplay == null)
            {
                return;
            }
            pbDisplay.Image = GetImage();

        }

        private Image GetImage()
        {
            if (_checkState == CheckState.Checked)
            {
                switch (_onOffType)
                {
                    case OnOffButtonType.Oblongrectangle:
                        return Buffalo.WinFormsControl.Properties.Resources.on1;
                    default:
                        return Buffalo.WinFormsControl.Properties.Resources.on;
                }
            }
            else
            {
                switch (_onOffType)
                {
                    case OnOffButtonType.Oblongrectangle:
                        return Buffalo.WinFormsControl.Properties.Resources.off1;
                    default:
                        return Buffalo.WinFormsControl.Properties.Resources.off;
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreashDisplay();
        }

        private void pbDisplay_Click(object sender, EventArgs e)
        {
            Checked = !Checked;
        }
    }

    /// <summary>
    /// 按钮样式
    /// </summary>
    public enum OnOffButtonType 
    {
        /// <summary>
        /// 方形
        /// </summary>
        Quadrate,
        /// <summary>
        /// 椭圆形
        /// </summary>
        Oblongrectangle
    }
}
