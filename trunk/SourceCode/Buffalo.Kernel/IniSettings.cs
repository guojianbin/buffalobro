using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ini���ö�ȡ��
    /// </summary>
    public class IniSettings
    {
        private Dictionary<string, string> _dicConfig;
        /// <summary>
        /// ini���ö�ȡ��
        /// </summary>
        /// <param name="filePath">ini�ļ���·��</param>
        public IniSettings(string filePath) 
        {
            using (FileStream stm = new FileStream(filePath, FileMode.Open, FileAccess.Read)) 
            {
                LoadIniConfig(stm);
            }
        }
        /// <summary>
        /// ini���ö�ȡ��
        /// </summary>
        /// <param name="stm">ini�ļ�����</param>
        public IniSettings(Stream stm)
        {
            LoadIniConfig(stm);
        }

        /// <summary>
        /// ���ر������µ�ini�ļ�
        /// </summary>
        /// <param name="fileName">ini����</param>
        /// <returns></returns>
        public static IniSettings Load(string fileName) 
        {
            string filePath=AppDomain.CurrentDomain.BaseDirectory + fileName;
            return new IniSettings(filePath);
        }

        /// <summary>
        /// ����Ini����
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        private void LoadIniConfig(Stream stm) 
        {
            _dicConfig = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader(stm)) 
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split('=');
                    if (items.Length != 2)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(items[0]))
                    {
                        continue;
                    }
                    string key = items[0].Trim();
                    _dicConfig[key] = items[1].Replace("&DY", "=").Replace("&BN", "\n").Replace("&BR", "\r");
                }
            }
        }
    }
}
