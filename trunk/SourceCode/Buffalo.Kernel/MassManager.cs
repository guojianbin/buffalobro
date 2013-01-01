using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 常量管理器
    /// </summary>
    public class MassManager
    {
        private static Dictionary<string, List<EnumInfo>> dicMass = new Dictionary<string, List<EnumInfo>>();

        /// <summary>
        /// 根据类型获取常量集合
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static List<EnumInfo> GetMassInfos(Type objType) 
        {
            string fullName = objType.FullName;
            List<EnumInfo> ret = null;
            if (!dicMass.TryGetValue(fullName, out ret)) 
            {
                ret= GetInfos(objType);
                dicMass[fullName] = ret;
            }
            return ret;
        }

        /// <summary>
        /// 通过字段名获取其常量信息
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static EnumInfo GetInfoByName(Type objType, string fieldName)
        {
            List<EnumInfo> dicInfos = GetMassInfos(objType);
            foreach (EnumInfo pair in dicInfos) 
            {
                if (pair.FieldName == fieldName) 
                {
                    return pair;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过值获取其常量信息
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumInfo GetInfoByValue(Type objType, object value) 
        {
            List<EnumInfo> lstInfos = GetMassInfos(objType);
            foreach (EnumInfo pair in lstInfos)
            {
                if (pair.Value.Equals(value))
                {
                    return pair;
                }
            }
            
            return null;
        }

        /// <summary>
        /// 获取此类的常量信息
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static List<EnumInfo> GetInfos(Type objType) 
        {

            List<EnumInfo> lstInfos = new List<EnumInfo>();
            FieldInfo[] fields = objType.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                EnumInfo info = new EnumInfo();
                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs != null)
                {
                    if (objs.Length > 0)
                    {
                        DescriptionAttribute da = objs[0] as DescriptionAttribute;
                        info.Description = da.Description;
                    }
                }
                objs = field.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                if (objs != null)
                {
                    if (objs.Length > 0)
                    {
                        DisplayNameAttribute da = objs[0] as DisplayNameAttribute;
                        info.DisplayName = da.DisplayName;
                    }
                }
                
                info.FieldName = field.Name;
                if (objType.IsEnum)
                {
                    info.Value = Enum.Parse(objType, field.Name);

                }
                else
                {
                    info.Value = field.GetValue(null);
                }
                lstInfos.Add(info);

            }
            return lstInfos;
        }
    }
}
