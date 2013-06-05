using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Reflection;
using System.Reflection.Emit;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.DB.ProxyBuilder
{
    public class EntityProxyBuilder
    {
       private static readonly Type VoidType = Type.GetType("System.Void");
        AssemblyName _assemblyName ;
        AssemblyBuilder _assemblyBuilder;
        ModuleBuilder _moduleBuilder;
        /// <summary>
        /// 代理建造类
        /// </summary>
        public EntityProxyBuilder(string classNamespace) 
        {
            _assemblyName = new AssemblyName(classNamespace);
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(_assemblyName,
                                                                            AssemblyBuilderAccess.RunAndSave);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(classNamespace);
        }

        /// <summary>
        /// 建造代理类
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
        /// 建造类
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
            //定义类型
            TypeBuilder typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType);
            ////定义字段 _inspector
            //FieldBuilder inspectorFieldBuilder = typeBuilder.DefineField("_inspector", typeof(IInterceptor),
            //                                                    FieldAttributes.Public | FieldAttributes.InitOnly);
            ////构造函数
            //BuildCtor(classType, typeBuilder);

            //构造方法
            BuildMethod(classType, typeBuilder);
            Type aopType = typeBuilder.CreateType();
            return aopType;
        }
        /// <summary>
        /// 建方法
        /// </summary>
        /// <param name="classType"></param>
        /// <param name="inspectorFieldBuilder"></param>
        /// <param name="typeBuilder"></param>
        private void BuildMethod(Type classType,  TypeBuilder typeBuilder)
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(classType);
            MethodInfo updateMethod = classType.GetMethod(
                "OnPropertyUpdated", FastValueGetSet.AllBindingFlags);
            foreach (EntityPropertyInfo pInfo in entityInfo.PropertyInfo) 
            {
                BuildEmit(classType, pInfo.PropertyName, typeBuilder, updateMethod, "set_" + pInfo.PropertyName);
            }
            MethodInfo fillChildMethod = classType.GetMethod(
                "FillChild", new Type[] { typeof(string) });
            MethodInfo fillParent = classType.GetMethod(
                "FillParent", new Type[] { typeof(string) });

            foreach (EntityMappingInfo mInfo in entityInfo.MappingInfo)
            {

                BuildEmit(classType, mInfo.PropertyName, typeBuilder, updateMethod, "set_" + mInfo.PropertyName);

            }
        }
        
        /// <summary>
        /// 创建IL
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
                throw new Exception("请把属性:" + propertyName + "设置为virtual");
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

            il.DeclareLocal(typeof(object)); //result 索引为0
            //Call methodInfo
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 1; i <= parameterLength; i++)
            {
                il.Emit(OpCodes.Ldarg_S, i);//加载第几个参数
            }
            il.Emit(OpCodes.Call, methodInfo);
            //将返回值压入 局部变量1result void就压入null
            if (!hasResult)
            {
                il.Emit(OpCodes.Ldnull);
            }
            else if (methodInfo.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Box, methodInfo.ReturnType);//对值类型装箱
            }

            il.Emit(OpCodes.Stloc_0);

            //callupdateMethod
            il.Emit(OpCodes.Ldarg_0);//this
            il.Emit(OpCodes.Ldstr, propertyName);//参数propertyName

            il.Emit(OpCodes.Call, updateMethod);//调用updateMethod

            //result
            if (hasResult)
            {
                il.Emit(OpCodes.Ldloc_0);//非void取出局部变量1 result
                if (methodInfo.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//对值类型拆箱
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
            il.Emit(OpCodes.Call, classType.GetConstructor(Type.EmptyTypes));//调用base的默认ctor
            //il.Emit(OpCodes.Ldarg_0);
            ////将typeof(classType)压入计算堆
            //il.Emit(OpCodes.Ldtoken, classType);
            //il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
            //调用DefaultInterceptorFactory.Create(type)

            //il.Emit(OpCodes.Call, _getDefaultMethod);
            //将结果保存到字段_inspector
            //il.Emit(OpCodes.Stfld, inspectorFieldBuilder);
            il.Emit(OpCodes.Ret);

        }
    }
}
