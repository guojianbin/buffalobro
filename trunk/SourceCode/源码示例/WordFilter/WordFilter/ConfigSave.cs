using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WordFilter
{
    /// <summary>
    /// 配置保存器
    /// </summary>
    public class ConfigSave
    {
        private static readonly string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + "\\config.cfg";
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="outItem">保存输出的类型</param>
        /// <returns></returns>
        public static void SaveConfig(int outItem) 
        {
            using (FileStream file = new FileStream(ConfigPath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(file))
                {
                    writer.Write(outItem);
                }
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <returns></returns>
        public static int ReadConfig()
        {
            if (!File.Exists(ConfigPath)) 
            {
                return 0;
            }
            using (FileStream file = new FileStream(ConfigPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(file))
                {
                    return reader.ReadInt32();
                }
            }
        }
    }
}
