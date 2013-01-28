using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;
using System.ComponentModel;
using Buffalo.WinFormsControl.Editors;
using Buffalo.Win32Kernel;



namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ��ѡ��
    /// </summary>
    public class ConfigItem
    {
        private string _name;
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get { return _name;}
            set { _name = value; }
        }

        private string _summary;
        /// <summary>
        /// ˵��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private List<ComboBoxItem> _lstItems = null;

        public List<ComboBoxItem> Items
        {
            get { return _lstItems; }
            set { _lstItems = value; }
        }
        private ConfigItemType _type;
        /// <summary>
        /// ������
        /// </summary>
        public ConfigItemType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public EditorBase GetEditor() 
        {
            switch (Type)
            {
                case ConfigItemType.Check:
                    CheckBoxEditor cbe = new CheckBoxEditor();
                    
                    cbe.OnOffType = OnOffButtonType.Oblongrectangle;
                    return cbe;
                case ConfigItemType.Combo:
                    
                    ComboBoxEditor cmb = new ComboBoxEditor();
                    cmb.BindValue(Items);
                    return cmb;
                case ConfigItemType.MText:
                    TextBoxEditor mtxt = new TextBoxEditor();
                    mtxt.Multiline = true;
                    mtxt.Height = 50;
                    return mtxt;
                default:
                    TextBoxEditor txt = new TextBoxEditor();

                    return txt;
            }
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public enum ConfigItemType 
    {
        /// <summary>
        /// ѡ���
        /// </summary>
        [Description("ѡ���")]
        Check,
        /// <summary>
        /// ������
        /// </summary>
        [Description("������")]
        Combo,
        /// <summary>
        /// �ı���
        /// </summary>
        [Description("�ı���")]
        Text,
        /// <summary>
        /// �����ı���
        /// </summary>
        [Description("�����ı���")]
        MText
    }
}
