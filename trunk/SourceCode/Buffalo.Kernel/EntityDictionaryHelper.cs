using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buffalo.Kernel.FastReflection;
using System.Collections;

namespace BleServer.App_Code
{
    public class EntityDictionaryHelper
    {

        /// <summary>
        /// 实体转换为字典集合
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyCollection">需要对应的字典键值（如：new string[]{"user=User.Name","uid=UserId"}）</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ValueListToDictionary(IList lstValue,
            string propertyMap)
        {

        }
        /// <summary>
        /// 实体转换为字典集合
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyCollection">需要对应的字典键值（如：new string[]{"user=User.Name","uid=UserId"}）</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> EntityListToDictionary(IList lstEntity,
            IEnumerable<string> propertyCollection)
        {
            if (lstEntity.Count == 0)
            {
                return new List<Dictionary<string, object>>();
            }
            List<Dictionary<string, object>> lstDic = new List<Dictionary<string, object>>(lstEntity.Count);
            Type objType = lstEntity[0].GetType();
            List<EntityToDictionaryInfo> lstInfo = GetDicInfos(objType, propertyCollection);
            foreach (object obj in lstEntity)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                foreach (EntityToDictionaryInfo info in lstInfo)
                {
                    item[info.Name] = GetValue(info, obj);
                }
                lstDic.Add(item);
            }
            return lstDic;

        }

        /// <summary>
        /// 根据反射信息获取值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetValue(EntityToDictionaryInfo info, object obj)
        {
            object value = obj;
            foreach (PropertyInfoHandle pHandle in info.PropertyInfos)
            {
                value = pHandle.GetValue(value);
            }
            return value;
        }
        /// <summary>
        /// 根据需要的数据获取反射信息
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="propertyCollection"></param>
        /// <returns></returns>
        private static List<EntityToDictionaryInfo> GetDicInfos(Type objType, IEnumerable<string> propertyCollection)
        {
            List<EntityToDictionaryInfo> lstInfos = new List<EntityToDictionaryInfo>(12);
            foreach (string strItem in propertyCollection)
            {
                string[] itemPart = strItem.Split('=');
                if (itemPart.Length < 2)
                {
                    continue;
                }
                EntityToDictionaryInfo info = new EntityToDictionaryInfo();
                info.Name = itemPart[0];
                string[] propertyItems = itemPart[1].Split('.');
                Type curType = objType;
                foreach (string proName in propertyItems)
                {
                    PropertyInfoHandle pinfo = FastValueGetSet.GetPropertyInfoHandle(proName, curType);
                    info.PropertyInfos.Add(pinfo);
                    curType = pinfo.PropertyType;
                }
                lstInfos.Add(info);
            }
            return lstInfos;
        }
    }



    /// <summary>
    /// 实体转换到字典的信息
    /// </summary>
    public class EntityToDictionaryInfo
    {
        private string _name;
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private List<PropertyInfoHandle> _propertyInfos = new List<PropertyInfoHandle>(4);
        /// <summary>
        /// 所属的属性反射
        /// </summary>
        public List<PropertyInfoHandle> PropertyInfos
        {
            get
            {
                return _propertyInfos;
            }

        }
    }
}