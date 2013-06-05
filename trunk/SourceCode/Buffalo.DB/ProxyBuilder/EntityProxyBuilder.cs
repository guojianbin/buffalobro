using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Reflection;
using System.Reflection.Emit;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Defaults;

/** 
 * @ԭ����:benben
 * @����ʱ��:2012-2-19 09:02
 * @����:http://www.189works.com/article-43203-1.html
 * @˵��:.NET IL��̬���������޸İ棩
*/

namespace Buffalo.DB.ProxyBuilder
{
    public class EntityProxyBuilder
    {
       private static readonly Type VoidType = Type.GetType("System.Void");
        AssemblyName _assemblyName ;
        AssemblyBuilder _assemblyBuilder;
        ModuleBuilder _moduleBuilder;
        /// <summary>
        /// ��������
        /// </summary>
        public EntityProxyBuilder(string classNamespace) 
        {
            _assemblyName = new AssemblyName(classNamespace);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName,
                                                                            AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(classNamespace);
        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type CreateProxyType(Type classType)
        {

            //string name = classType.Namespace + ".ProxyClass";

            string className = classType.FullName;

            Type aopType = BulidType(classType, _moduleBuilder, className);
            
            //_assemblyBuilder.Save("bac.dll");
            return aopType;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="moduleBuilder"></param>
        /// <returns></returns>
        private Type BulidType(Type classType, ModuleBuilder moduleBuilder,string className)
        {
            //string className = classType.Name + "_Proxy";
            if (string.IsNullOrEmpty(className)) 
            {
                className = classType.Name;
            }
            //��������
            TypeBuilder typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType);
            ////�����ֶ� _inspector
            //FieldBuilder inspectorFieldBuilder = typeBuilder.DefineField("_inspector", typeof(IInterceptor),
            //                                                    FieldAttributes.Public | FieldAttributes.InitOnly);
            ////���캯��
            //BuildCtor(classType, typeBuilder);

            //���췽��
            BuildMethod(classType, typeBuilder);
            Type aopType = typeBuilder.CreateType();
            return aopType;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="inspectorFieldBuilder"></param>
        /// <param name="typeBuilder"></param>
        private void BuildMethod(Type classType,  TypeBuilder typeBuilder)
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(classType);
            MethodInfo updateMethod = classType.GetMethod(
                "OnPropertyUpdated", FastValueGetSet.AllBindingFlags);
            MethodInfo mapupdateMethod = classType.GetMethod(
                "OnMapPropertyUpdated", FastValueGetSet.AllBindingFlags);
            foreach (EntityPropertyInfo pInfo in entityInfo.PropertyInfo) 
            {
                BuildEmit(classType, pInfo.PropertyName, typeBuilder, updateMethod, "set_" + pInfo.PropertyName);
            }
            MethodInfo fillChildMethod = classType.GetMethod(
                "FillChild", FastValueGetSet.AllBindingFlags);
            MethodInfo fillParent = classType.GetMethod(
                "FillParent", FastValueGetSet.AllBindingFlags);

            foreach (EntityMappingInfo mInfo in entityInfo.MappingInfo)
            {
                FieldInfo finfo = GetFieldInfo(classType, mInfo.FieldName);
                if (mInfo.IsParent)
                {

                    BuildEmit(classType, mInfo.PropertyName, typeBuilder, mapupdateMethod, "set_" + mInfo.PropertyName);
                    BuildMapEmit(classType, mInfo.PropertyName, finfo, typeBuilder, fillParent, "get_" + mInfo.PropertyName);
                }
                else 
                {
                    BuildMapEmit(classType, mInfo.PropertyName, finfo, typeBuilder, fillChildMethod, "get_" + mInfo.PropertyName);
                }

            }

           
        }

        private FieldInfo GetFieldInfo(Type classType, string fieldName) 
        {
            FieldInfo finfo = null;
            Type curType = classType;
            while (curType != null) 
            {
                finfo = curType.GetField(fieldName);
                if (finfo != null) 
                {
                    return finfo;
                }
            }
            return null;
        }

        /// <summary>
        /// ����IL
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="pInfo"></param>
        /// <param name="typeBuilder"></param>
        /// <param name="updateMethod"></param>
        /// <param name="methodName"></param>
        private void BuildEmit(Type classType,string propertyName,
            TypeBuilder typeBuilder, MethodInfo updateMethod,string methodName)
        {
            MethodInfo methodInfo = classType.GetMethod(methodName);
            
            if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
            {
                throw new Exception("�������:" + propertyName + "����Ϊvirtual");
                return;
            }

           
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            List<Type> lstType = new List<Type>(parameterInfos.Length);
            foreach (ParameterInfo info in parameterInfos)
            {
                lstType.Add(info.ParameterType);
            }

            Type[] parameterTypes = lstType.ToArray();
            int parameterLength = parameterTypes.Length;
            bool hasResult = methodInfo.ReturnType != VoidType;

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                                                         MethodAttributes.Public |
                                                         MethodAttributes.Virtual
                                                         , methodInfo.ReturnType
                                                         , parameterTypes);
            
            ILGenerator il = methodBuilder.GetILGenerator();

            il.DeclareLocal(typeof(object)); //result ����Ϊ0
            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 1; i <= parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, i);//���صڼ�������
            }
            il.Emit(OpCodes.Call, methodInfo);
            //������ֵѹ�� �ֲ�����1result void��ѹ��null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//��ֵ����װ��
            }

            il.Emit(OpCodes.Stloc_0);

            //callupdateMethod
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyName);//����propertyName

            il.Emit(OpCodes.Call, updateMethod);//����updateMethod

            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc_0);//��voidȡ���ֲ�����1 result
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//��ֵ���Ͳ���
                }
            }
            il.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// ����IL
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="pInfo"></param>
        /// <param name="typeBuilder"></param>
        /// <param name="updateMethod"></param>
        /// <param name="methodName"></param>
        private void BuildMapEmit(Type classType, string propertyName,FieldInfo finfo,
            TypeBuilder typeBuilder, MethodInfo updateMethod, string methodName)
        {
            MethodInfo methodInfo = classType.GetMethod(methodName);
            if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
            {
                throw new Exception("�������:" + propertyName + "����Ϊvirtual");
                return;
            }


            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            List<Type> lstType = new List<Type>(parameterInfos.Length);
            foreach (ParameterInfo info in parameterInfos)
            {
                lstType.Add(info.ParameterType);
            }

            Type[] parameterTypes = lstType.ToArray();
            int parameterLength = parameterTypes.Length;
            bool hasResult = methodInfo.ReturnType != VoidType;

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                                                         MethodAttributes.Public |
                                                         MethodAttributes.Virtual
                                                         , methodInfo.ReturnType
                                                         , parameterTypes);

            ILGenerator il = methodBuilder.GetILGenerator();

            il.DeclareLocal(typeof(object)); //result ����Ϊ0

            //if(�ֶ�==null){������Ϣ}

            il.Emit(OpCodes.Ldarg_0);//this
            
            il.Emit(OpCodes.Ldfld, finfo);//��ȡ�ֶ�ֵ
            il.Emit(OpCodes.Ldnull);//��null�ŵ��ڶ���λ��
            il.Emit(OpCodes.Ceq);//�Ƚ����(����򷵻�1��������򷵻�0)
            il.Emit(OpCodes.Ldc_I4_0);//����ֵ0���͵�ջ
            il.Emit(OpCodes.Ceq);//�Ƚ����(����򷵻�1��������򷵻�0)
            //il.Emit(OpCodes.Stloc_1);
            //il.Emit(OpCodes.Ldloc_1);

            Label falseLabel = il.DefineLabel();//��Ϊnullʱ�����ת��ǩ
            il.Emit(OpCodes.Brtrue_S, falseLabel);

            //������亯��
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyName);//����propertyName
            il.Emit(OpCodes.Call, updateMethod);//����updateMethod

            il.MarkLabel(falseLabel);

            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 1; i <= parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, i);//���صڼ�������
            }
            il.Emit(OpCodes.Call, methodInfo);
            //������ֵѹ�� �ֲ�����1result void��ѹ��null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//��ֵ����װ��
            }

            il.Emit(OpCodes.Stloc_0);

            

            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc_0);//��voidȡ���ֲ�����1 result
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//��ֵ���Ͳ���
                }
            }
            il.Emit(OpCodes.Ret);
        }

        private void BuildCtor(Type classType,  TypeBuilder typeBuilder)
        {

            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(
                MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);

            ILGenerator il = ctorBuilder.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Call, classType.GetConstructor(Type.EmptyTypes));//����base��Ĭ��ctor
            il.Emit(OpCodes.Ret);

        }
    }
}
