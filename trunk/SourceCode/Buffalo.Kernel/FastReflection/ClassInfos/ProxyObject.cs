using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    public class ProxyObject : MarshalByRefObject,IDisposable
    {
        ClassInfoHandle _classHandle = null;
        object instance;//ʵ��
        AppDomain app;
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="filePath">�ļ���</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(string filePath,string TypeName,params object[] args)
        {
            Assembly assembly = Assembly.LoadFrom(filePath);
            
            Type tp = assembly.GetType(TypeName);
            Init(tp,args);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dllContent">�ļ�����</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(byte[] dllContent, string TypeName,string domainName, params object[] args)
        {
            app=AppDomain.CreateDomain(domainName);
            Assembly assembly = app.Load(dllContent);
            Type tp = assembly.GetType(TypeName);
            Init(tp, args);
            
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="dllContent">�ļ�����</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(byte[] dllContent, string TypeName, params object[] args)
        {
            app = AppDomain.CreateDomain("domain"+TypeName);
            Assembly assembly = app.Load(dllContent);
            Type tp = assembly.GetType(TypeName);
            Init(tp, args);

        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="assembly">����</param>
        /// <param name="TypeName">����</param>
        public ProxyObject(Assembly assembly, string TypeName, params object[] args)
        {
            Type tp = assembly.GetType(TypeName);
            Init(tp, args);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="tp">����</param>
        public ProxyObject(Type tp, params object[] args)
        {
            //Type tp = assembly.GetType(TypeName);
            Init(tp, args);
        }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="assembly">����</param>
        /// <param name="TypeName">������</param>
        private void Init(Type tp, params object[] args) 
        {
            
            _classHandle = ClassInfoManager.GetClassHandle(tp);
            instance = Activator.CreateInstance(tp,args);
        }

        /// <summary>
        /// ���иú���
        /// </summary>
        /// <param name="methodName">������</param>
        /// <param name="args">����</param>
        /// <returns></returns>
        public object Invoke(string methodName, params object[] args)
        {
            FastInvokeHandler fhandle = FastValueGetSet.GetCustomerMethodInfo(_classHandle.ClassType, methodName, GetParamTypes(args));
            return fhandle.Invoke(instance, args);
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="args">��������</param>
        /// <returns></returns>
        private static Type[] GetParamTypes(object[] args) 
        {
            List<Type> lstType = new List<Type>(10);
            if (args != null) 
            {
                foreach (object obj in args) 
                {
                    lstType.Add(obj.GetType());
                }
            }
            return lstType.ToArray();
        }

        /// <summary>
        /// ���û��ȡ����
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public object this[string propertyName] 
        {
            get 
            {
                return _classHandle.PropertyInfo[propertyName].GetValue(instance);
            }
            set 
            {
                 _classHandle.PropertyInfo[propertyName].SetValue(value,instance);
            }
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public ClassInfoHandle ClassHandle
        {
            get { return _classHandle; }
            set { _classHandle = value; }
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            if (app != null) 
            {
                AppDomain.Unload(app);
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        ~ProxyObject() 
        {
            Dispose();
        }
    }   
}
