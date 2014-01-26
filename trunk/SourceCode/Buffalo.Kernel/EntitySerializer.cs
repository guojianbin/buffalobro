using System;
using System.Collections.Generic;
using Buffalo.Kernel.FastReflection;
using System.Collections;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 格式化类型的方法
    /// </summary>
    /// <param name="key">Dictionary键</param>
    /// <param name="propertyName">属性名(如：User.Name)</param>
    /// <param name="entity">实体</param>
    /// <param name="value">值</param>
    /// <param name="newValue">新值</param>
    /// <returns>是否格式化成功(失败的话就不赋值)</returns>
    public delegate object DelFormatValue(string key, string propertyName, object entity, object value);

    /// <summary>
    /// 实体序列化类
    /// </summary>
    public class EntitySerializer
    {

        /// <summary>
        /// 值集合转换为字典集合(适合JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyMap">Key的名字</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ValueListToDictionary(IList lstValue,
            string propertyMap, DelFormatValue formatValue)
        {
            if (lstValue.Count == 0)
            {
                return new List<Dictionary<string, object>>();
            }
            List<Dictionary<string, object>> lstDic = new List<Dictionary<string, object>>(lstValue.Count);
            object value = null;

            foreach (object obj in lstValue)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                value =obj;
                if (formatValue != null)
                {
                    value = formatValue(propertyMap, "value", value, value);
                }
                item[propertyMap] = value;
                lstDic.Add(item);
            }
            return lstDic;
        }
         /// <summary>
        /// 值集合转换为字典集合(适合JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyMap">Key的名字</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> ValueListToDictionary(IList lstValue,
            string propertyMap)
        {
            return ValueListToDictionary(lstValue, propertyMap, DefaultFormatValue);
        }
        /// <summary>
        /// 实体转换为字典集合(适合JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyCollection">需要对应的字典键值（如：new string[]{"user=User.Name","uid=UserId"}）</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> EntityListToDictionary(IList lstEntity,
            IEnumerable<string> propertyCollection,DelFormatValue formatValue)
        {
            if (lstEntity.Count == 0)
            {
                return new List<Dictionary<string, object>>();
            }
            List<Dictionary<string, object>> lstDic = new List<Dictionary<string, object>>(lstEntity.Count);
            Type objType = lstEntity[0].GetType();
            List<EntitySerializerInfo> lstInfo = GetDicInfos(objType, propertyCollection);
            object value = null;

            foreach (object obj in lstEntity)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                foreach (EntitySerializerInfo info in lstInfo)
                {
                    value=GetValue(info, obj);
                    if(formatValue!=null)
                    {
                        value = formatValue(info.Name, info.PropertyName, obj, value);
                    }
                    item[info.Name] = value;
                }
                lstDic.Add(item);
            }
            return lstDic;

        }

        /// <summary>
        /// 实体转换为字典集合(适合JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyCollection">需要对应的字典键值（如：new string[]{"user=User.Name","uid=UserId"}）</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static Dictionary<string, object> EntityToDictionary(object entity,
            IEnumerable<string> propertyCollection)
        {
            return EntityToDictionary(entity, propertyCollection, DefaultFormatValue);
        }

        /// <summary>
        /// 实体转换为字典集合(适合JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyCollection">需要对应的字典键值（如：new string[]{"user=User.Name","uid=UserId"}）</param>
        /// <param name="formatValue">格式化值的方法</param>
        /// <returns></returns>
        public static Dictionary<string, object> EntityToDictionary(object entity,
            IEnumerable<string> propertyCollection, DelFormatValue formatValue)
        {
            if (entity == null)
            {
                return new Dictionary<string, object>();
            }

            Type objType = entity.GetType();
            List<EntitySerializerInfo> lstInfo = GetDicInfos(objType, propertyCollection);
            object value = null;


            Dictionary<string, object> item = new Dictionary<string, object>();
            foreach (EntitySerializerInfo info in lstInfo)
            {
                value = GetValue(info, entity);
                if (formatValue != null)
                {
                    value = formatValue(info.Name, info.PropertyName, entity, value);
                }
                item[info.Name] = value;
            }
            return item;
        }

        /// <summary>
        /// 实体转换为字典集合(适合JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">实体</param>
        /// <param name="propertyCollection">需要对应的字典键值（如：new string[]{"user=User.Name","uid=UserId"}）</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> EntityListToDictionary(IList lstEntity,
            IEnumerable<string> propertyCollection)
        {
            return EntityListToDictionary(lstEntity, propertyCollection, DefaultFormatValue);
        }

        /// <summary>
        /// 默认的值格式化方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DefaultFormatValue(string key, string propertyName, object entity, object value)
        {

            if (value == null)
            {

                return null;
            }
            Type objType = value.GetType();
            if (objType.IsEnum)
            {
                return (int)value;
            }
            else if (value is DateTime)
            {
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.ms");
            }

            return value;

        }

        /// <summary>
        /// 根据反射信息获取值
        /// </summary>
        /// <param name="info"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetValue(EntitySerializerInfo info, object obj)
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
        private static List<EntitySerializerInfo> GetDicInfos(Type objType, IEnumerable<string> propertyCollection)
        {
            List<EntitySerializerInfo> lstInfos = new List<EntitySerializerInfo>(12);
            foreach (string strItem in propertyCollection)
            {
                string[] itemPart = strItem.Split('=');
                if (itemPart.Length < 2)
                {
                    continue;
                }
                EntitySerializerInfo info = new EntitySerializerInfo();
                
                info.Name = itemPart[0];
                info.PropertyName = itemPart[1];
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
    /// 实体序列化信息
    /// </summary>
    public class EntitySerializerInfo
    {
        private string _name;
        /// <summary>
        /// 键名
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
        private string _propertyName;
        /// <summary>
        /// 属性名字
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
            set
            {
                _propertyName = value;
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