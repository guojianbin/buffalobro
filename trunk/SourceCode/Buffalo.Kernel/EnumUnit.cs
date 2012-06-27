using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace Buffalo.Kernel
{
    public class EnumUnit
    {
        /// <summary>
        /// 在属性集合添加一个属性
        /// </summary>
        /// <param name="source">属性集合</param>
        /// <param name="value">要添加的属性值</param>
        /// <returns></returns>
        public static int AddValue(int source, int value)
        {
            return source | value;
        }
        /// <summary>
        /// 在属性集合删除一个属性
        /// </summary>
        /// <param name="source">属性集合</param>
        /// <param name="value">要删除的属性值</param>
        /// <returns></returns>
        public static int DeleteValue(int source, int value)
        {
            int tmp = ~value;
            return source & tmp;
        }

        /// <summary>
        /// 判断集合里边是否含有该属性
        /// </summary>
        /// <param name="source">属性集合</param>
        /// <param name="value">要判断的属性值</param>
        /// <returns></returns>
        public static bool ContainerValue(int source, int value)
        {
            int tmp = source | value;
            return source == tmp;
        }


        /// <summary>
        /// 获取本枚举里边所有属性的信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<EnumInfo> GetEnumInfos(Type enumType) 
        {
            List<EnumInfo> dicInfos = MassManager.GetMassInfos(enumType);
            return dicInfos;
        }

        /// <summary>
        /// 获取枚举值的注释
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum objEnum) 
        {
            EnumInfo info = GetEnumInfo(objEnum);
            if (info != null) 
            {
                return info.Description;
            }
            return null;
        }

        /// <summary>
        /// 获取枚举值的信息
        /// </summary>
        /// <param name="objEnum">枚举值</param>
        /// <returns></returns>
        public static EnumInfo GetEnumInfo(Enum objEnum)
        {
            return MassManager.GetInfoByValue(objEnum.GetType(),objEnum);
        }


        /// <summary>
        /// 根据枚举的属性名来获取其信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public static EnumInfo GetEnumInfoByName(Type enumType, string name)
        {

            return MassManager.GetInfoByName(enumType,name);
        }

        /// <summary>
        /// 获取枚举字符串
        /// </summary>
        /// <param name="objEnum"></param>
        /// <returns></returns>
        public static string EnumString(Enum objEnum) 
        {
            StringBuilder sbTmp = new StringBuilder();
            string typeName = objEnum.GetType().Name;
            List<EnumInfo> dic = GetEnumInfos(objEnum.GetType());
            foreach (EnumInfo kvp in dic) 
            {
                object value = kvp.Value;
                if ((Convert.ToUInt32(objEnum) & (uint)value) == (uint)value) 
                {
                    sbTmp.Append(typeName);
                    sbTmp.Append(".");
                    sbTmp.Append(kvp.FieldName);
                    sbTmp.Append("|");
                }
            }
            
            if (sbTmp.Length > 0) 
            {
                sbTmp.Remove(sbTmp.Length - 1, 1);
            }
            return sbTmp.ToString();
        }
    }
}
//******************例子*******************
            //SHFileOperationError err = SHFileOperationError.DE_MANYSRC1DEST;
            //EnumInfo info= EnumUnit.GetEnumInfo(err);