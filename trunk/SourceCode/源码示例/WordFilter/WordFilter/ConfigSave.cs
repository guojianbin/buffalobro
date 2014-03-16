using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Buffalo.Kernel.Win32;

namespace WordFilter
{
    /// <summary>
    /// ����
    /// </summary>
    public class ConfigSave
    {
        /// <summary>
        /// ����
        /// </summary>
        public ConfigSave() 
        {
            _outItem = 0;
            _hotKey = Keys.F6;
            _modifiers = KeyModifiers.None;
            _showTime = 500;
            _side = 300;
        }

        private int _outItem;
        /// <summary>
        /// �������
        /// </summary>
        public int OutItem
        {
            get { return _outItem; }
            set { _outItem = value; }
        }
        private Keys _hotKey;
        /// <summary>
        /// �ȼ�
        /// </summary>
        public Keys HotKey
        {
            get { return _hotKey; }
            set { _hotKey = value; }
        }
        private KeyModifiers _modifiers;
        /// <summary>
        /// �ȼ���ϼ�
        /// </summary>
        public KeyModifiers Modifiers
        {
            get { return _modifiers; }
            set { _modifiers = value; }
        }
        private int _side;
        /// <summary>
        /// �ߴ�
        /// </summary>
        public int Side
        {
            get { return _side; }
            set 
            {
                if (value > 50)
                {
                    _side = value;
                }
            }
        }
        private int _showTime;
        /// <summary>
        /// ��ʾ���ֵı���
        /// </summary>
        public int ShowTime
        {
            get { return _showTime; }
            set { _showTime = value; }
        }

        private bool _listenClipboard=true;
        /// <summary>
        /// ����������
        /// </summary>
        public bool ListenClipboard
        {
            get { return _listenClipboard; }
            set { _listenClipboard = value; }
        }
        private static readonly string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + "\\config.cfg";
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="outItem">�������������</param>
        /// <returns></returns>
        public void SaveConfig() 
        {
            using (FileStream file = new FileStream(ConfigPath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(file))
                {
                    try
                    {
                        writer.Write(_outItem);
                        writer.Write((int)_hotKey);
                        writer.Write((int)_modifiers);
                        writer.Write(_side);
                        writer.Write(_showTime);
                        writer.Write(_listenClipboard);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public static ConfigSave ReadConfig()
        {
            ConfigSave config = new ConfigSave();

            if (!File.Exists(ConfigPath)) 
            {
                return config;
            }
            using (FileStream file = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(file))
                {
                    try
                    {
                        config._outItem = reader.ReadInt32();
                        config._hotKey = (Keys)reader.ReadInt32();
                        config._modifiers = (KeyModifiers)reader.ReadInt32();
                        config.Side = reader.ReadInt32(); 
                        config._showTime = reader.ReadInt32();
                        config._listenClipboard = reader.ReadBoolean(); 
                    }
                    catch { }
                }
            }
            return config;
        }
    }
}
