using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel;
namespace Buffalo.Kernel.FastReflection
{
    public class FastValueGetSet
    {
        private static Dictionary<string, FastInvokeHandler> dicMethod = new Dictionary<string, FastInvokeHandler>();
        private static Dictionary<string, PropertyInfoHandle> dicProperty = new Dictionary<string, PropertyInfoHandle>();//���Ի���
        private static IDictionary<string, CreateInstanceHandler> _invokerInstance = new Dictionary<string, CreateInstanceHandler>();
        public const BindingFlags AllBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.SuppressChangeType | BindingFlags.Instance;
        /// <summary>
        /// ��ȡ���Ե���Ϣ
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandle(string proName, Type type)
        {
            string fullName = type.FullName + "." + proName;
            PropertyInfoHandle propertyHandle = null;
            
                if (!dicProperty.TryGetValue(fullName, out propertyHandle))
                {
                    using (Lock objLock = new Lock(dicProperty))
                    {
                        propertyHandle = GetPropertyInfoHandleWithOutCache(proName, type);
                        dicProperty[fullName] = propertyHandle;
                    }
                }
            
            return propertyHandle;
        }


        

        /// <summary>
        /// ��ȡ���Ե���Ϣ(��ʹ�û���)
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandleWithOutCache(string proName, Type type)
        {
            FastInvokeHandler getHandle = GetGetMethodInfo(proName, type);
            FastInvokeHandler setHandle = GetSetMethodInfo(proName, type);
            PropertyInfo pinf = type.GetProperty(proName, AllBindingFlags);//��ȡ��Ԫ�ؼ��ϵ�����
            Type proType = null;
            if (pinf != null)
            {
                proType = pinf.PropertyType;
            }
            PropertyInfoHandle propertyHandle = new PropertyInfoHandle(type, getHandle, setHandle, proType, proName);
            return propertyHandle;
        }

        /// <summary>
        /// ��ȡ��ȡֵ�ķ����ӿ�
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static FastInvokeHandler GetGetMethodInfo(string proName,Type type)
        {
            MethodInfo methodInfo = type.GetMethod("get_" + proName,AllBindingFlags);
            if (methodInfo == null)
            {
                return null;
            }else if(methodInfo.GetParameters().Length>0)
            {
                return null;
            }
            FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
            return fastInvoker;
        }


        /// <summary>
        /// ��ȡ����ֵ�ķ����ӿ�
        /// </summary>
        /// <param name="proName">������</param>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static FastInvokeHandler GetSetMethodInfo(string proName, Type type)
        {
            MethodInfo methodInfo = type.GetMethod("set_" + proName, AllBindingFlags);
            if (methodInfo == null) 
            {
                return null;
            }
            else if (methodInfo.GetParameters().Length != 1)
            {
                return null;
            }
            FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);

            return fastInvoker;
        }
        /// <summary>
        /// ��ȡ�����͵�ָ��������ί��
        /// </summary>
        /// <param name="objectType">�����������</param>
        /// <param name="methodName">������</param>
        /// <param name="parametersType">�����б�</param>
        /// <returns></returns>
        public static FastInvokeHandler GetCustomerMethodInfo(Type objectType,string methodName,Type[] parametersType)
        {
            MethodInfo methodInfo = objectType.GetMethod(methodName, AllBindingFlags, null, parametersType, null);
            if (methodInfo != null)
            {
                return GetCustomerMethodInfo(methodInfo);
            }
            return null;
        }
        /// <summary>
        /// ��ȡ�����͵�ָ��������ί��
        /// </summary>
        /// <param name="methodInfo">������Ϣ</param>
        /// <param name="parametersType">�����б�</param>
        /// <returns></returns>
        public static FastInvokeHandler GetCustomerMethodInfo(MethodInfo methodInfo)
        {
            string ptypes = "";
            ParameterInfo[] info = methodInfo.GetParameters();
            foreach (ParameterInfo ptype in info)
            {
                if (ptype.IsOut) 
                {
                    ptypes += "out ";
                }
                else if (ptype.IsRetval)
                {
                    ptypes += "Retva ";
                }
                else if (ptype.IsLcid)
                {
                    ptypes += "Lcid ";
                }
                else if (ptype.IsOptional)
                {
                    ptypes += "Optional ";
                }
                ptypes += ptype.ParameterType.FullName + ",";
            }
            if (ptypes.Length > 0)
            {
                ptypes = ptypes.Substring(0, ptypes.Length - 1);
            }
            string fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name + "(" + ptypes + ")";
            FastInvokeHandler fastInvokerHandle = null;
            
                if (!dicMethod.TryGetValue(fullName, out fastInvokerHandle))
                {
                    if (methodInfo == null)
                    {
                        return null;
                    }
                    using (Lock objLock = new Lock(dicMethod))
                    {
                        FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
                        dicMethod[fullName]=fastInvoker;
                        fastInvokerHandle = fastInvoker;
                    }
                }
            
            return fastInvokerHandle;
        }

        /// <summary>
        /// �Զ���ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="propertyName">������</param>
        /// <param name="objType">��������</param>
        public static void SetValue(object args, object value, string propertyName,Type objType) 
        {
            PropertyInfoHandle fastInvoker = GetPropertyInfoHandle(propertyName, objType);
            fastInvoker.SetValue(args,value);
        }

        /// <summary>
        /// ��ȡ����ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="propertyName">������</param>
        /// <param name="objType">��������</param>
        public static object GetValue(object args, string propertyName, Type objType)
        {
            PropertyInfoHandle fastInvoker = GetPropertyInfoHandle(propertyName, objType);
            return fastInvoker.GetValue(args);
        }

        
        /// <summary>
        /// ����ʵ����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateObject(Type type)
        {
            CreateInstanceHandler create = GetCreateInstanceHandler(type);
            return create.Invoke();
        }

        /// <summary>
        /// �������Ͳ���ָ���Ĵ�������Ĵ���
        /// </summary>
        /// <param name="type">����</param> 
        /// <returns></returns> 
        public static CreateInstanceHandler GetCreateInstanceHandler(Type type) 
        {
            CreateInstanceHandler create = null;
            string key = type.FullName;
            
                if (!_invokerInstance.TryGetValue(key, out create))
                {
                    using (Lock objLock = new Lock(_invokerInstance))
                    {
                        create = GetCreateInstanceHandlerWithOutCache(type);
                        _invokerInstance.Add(key, create);
                    }
                }
            
            return create;
        }

        /// <summary>
        /// �������Ͳ���ָ���Ĵ�������Ĵ���
        /// </summary>
        /// <param name="type">����</param> 
        /// <returns></returns> 
        public static CreateInstanceHandler GetCreateInstanceHandlerWithOutCache(Type type)
        {
            CreateInstanceHandler create = null;
            create = FastInvoke.GetInstanceCreator(type);
            return create;
        }
    }
}
