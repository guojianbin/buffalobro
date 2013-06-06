using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Reflection;
using System.Reflection.Emit;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;
using Buffalo.DB.CommBase;

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
        string pnamespace = null;
         MethodInfo _updateMethod = null;
            MethodInfo _mapupdateMethod = null;
        MethodInfo _fillChildMethod = null;
            MethodInfo _fillParent = null;

        /// <summary>
        /// ��������
        /// </summary>
        public EntityProxyBuilder() 
        {
            pnamespace = "BuffaloProxyBuilder";
            _assemblyName = new AssemblyName(pnamespace);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName,
                                                                            AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(pnamespace);

            Type classType=typeof(EntityBase);
            _updateMethod = classType.GetMethod("OnPropertyUpdated", FastValueGetSet.AllBindingFlags);
            _mapupdateMethod = classType.GetMethod("OnMapPropertyUpdated", FastValueGetSet.AllBindingFlags);
            _fillChildMethod = classType.GetMethod("FillChild", FastValueGetSet.AllBindingFlags);
             _fillParent = classType.GetMethod("FillParent", FastValueGetSet.AllBindingFlags);

        }

        /// <summary>
        /// ���������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Type CreateProxyType(Type classType)
        {

            //string name = classType.Namespace + ".ProxyClass";

            string className = pnamespace+"."+classType.Name + "_" + CommonMethods.GuidToString(Guid.NewGuid());

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
           
            foreach (EntityPropertyInfo pInfo in entityInfo.PropertyInfo) 
            {
                BuildEmit(classType, pInfo.BelongPropertyInfo, typeBuilder, _updateMethod);
            }
            

            foreach (EntityMappingInfo mInfo in entityInfo.MappingInfo)
            {
                FieldInfo finfo = mInfo.BelongFieldInfo;
                if (mInfo.IsParent)
                {

                    BuildEmit(classType, mInfo.BelongPropertyInfo, typeBuilder, _mapupdateMethod);
                    BuildMapEmit(classType, mInfo.BelongPropertyInfo, finfo, typeBuilder, _fillParent);
                }
                else 
                {
                    BuildMapEmit(classType, mInfo.BelongPropertyInfo, finfo, typeBuilder, _fillChildMethod);
                }

            }

           
        }

       

        /// <summary>
        /// ����IL
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="pInfo"></param>
        /// <param name="typeBuilder"></param>
        /// <param name="updateMethod"></param>
        /// <param name="methodName"></param>
        private void BuildEmit(Type classType,PropertyInfo propertyInfo,
            TypeBuilder typeBuilder, MethodInfo updateMethod)
        {

            MethodInfo methodInfo = propertyInfo.GetSetMethod();
            if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
            {
                throw new Exception("�����:" + classType .FullName+ " ������:" + propertyInfo.Name + " ����Ϊvirtual");
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

            LocalBuilder retVal=il.DeclareLocal(typeof(object)); //result ����Ϊ0
            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));//���صڼ�������

            }
            il.Emit(OpCodes.Call, methodInfo);//base.����();  ���������Callvirt�������ѭ�����ñ�����
            //������ֵѹ�� �ֲ�����1result void��ѹ��null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//��ֵ����װ��
            }

            il.Emit(OpCodes.Stloc, retVal);//����ֵ���浽retVal

            //callupdateMethod
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyInfo.Name);//����propertyName

            il.Emit(OpCodes.Callvirt, updateMethod);//����updateMethod

            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc, retVal);//��voidȡ���ֲ�����1 result
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
        private void BuildMapEmit(Type classType, PropertyInfo propertyInfo, FieldInfo finfo,
            TypeBuilder typeBuilder, MethodInfo updateMethod)
        {
            MethodInfo methodInfo = propertyInfo.GetGetMethod();
            if (!methodInfo.IsVirtual && !methodInfo.IsAbstract)
            {
                throw new Exception("�������:" + propertyInfo.Name + "����Ϊvirtual");
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
            il.Emit(OpCodes.Ldstr, propertyInfo.Name);//����propertyName
            il.Emit(OpCodes.Callvirt, updateMethod);//����updateMethod

            il.MarkLabel(falseLabel);

            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, (i + 1));//���صڼ�������
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
