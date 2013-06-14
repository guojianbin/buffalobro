using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CommBase;
using Buffalo.DB.ProxyBuilder;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// �����ഴ����
    /// </summary>
    public class CH
    {
        /// <summary>
        /// ����������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>() where T:EntityBase
        {
            Type objType=typeof(T);
            EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(objType);
            if (handle != null) 
            {
                return handle.CreateProxyInstance() as T;
            }
            return null;
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        public static object Create(Type objType) 
        {
            EntityInfoHandle handle = EntityInfoManager.GetEntityHandle(objType);
            if (handle != null)
            {
                return handle.CreateProxyInstance();
            }
            return null;
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type GetRealType(object obj)
        {
            IEntityProxy iep = obj as IEntityProxy;
            if (iep != null) 
            {
                return iep.GetEntityType();
            }
            return obj.GetType();
        }
    }
}
