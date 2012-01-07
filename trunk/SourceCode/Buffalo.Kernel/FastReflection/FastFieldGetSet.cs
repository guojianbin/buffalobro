using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// ���������ֶ�ֵ��ί��
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="value">ֵ</param>
    public delegate void SetFieldValueHandle(object obj, object value);



    /// <summary>
    /// ���ٻ�ȡ�ֶ�ֵ��ί��
    /// </summary>
    /// <param name="obj">����</param>
    /// <returns></returns>
    public delegate object GetFieldValueHandle(object obj);
    public class FastFieldGetSet
    {
        /// <summary>
        /// ��ȡ�ֶλ�ȡֵ��ί��
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public static GetFieldValueHandle GetGetValueHandle(FieldInfo info) 
        {
            Type sourceType = info.FieldType;

            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, info.DeclaringType.Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();
            //Label labRet = il.DefineLabel();
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, info);
            if (sourceType.IsValueType)
            {
                il.Emit(OpCodes.Box, sourceType);
            }
            il.Emit(OpCodes.Ret);
            return (GetFieldValueHandle)dynamicMethod.CreateDelegate(typeof(GetFieldValueHandle));
        }

        /// <summary>
        /// ��ȡ�ֶλ�ȡֵ��ί��
        /// </summary>
        /// <param name="objType">��������</param>
        /// <param name="fieldName">�ֶ���</param>
        /// <returns></returns>
        public static GetFieldValueHandle GetGetValueHandle(Type objType, string fieldName)
        {
            FieldInfo info = objType.GetField(fieldName, FastValueGetSet.allBindingFlags);
            return GetGetValueHandle(info);
        }

        /// <summary>
        /// ��ȡ�ֶ�����ֵ��ί��
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public static SetFieldValueHandle GetSetValueHandle(FieldInfo info) 
        {
            Type targetType = info.FieldType;
            //MethodInfo call = typeof(objType).GetMethod("WriteLine", new Type[] { typeof(string), typeof(string) });
            DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[] { typeof(object), typeof(object) }, info.DeclaringType.Module, true);
            ILGenerator il = dynamicMethod.GetILGenerator();

            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if (targetType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, targetType);
            }
            else
            {
                il.Emit(OpCodes.Castclass, targetType);
            }
            il.Emit(OpCodes.Stfld, info);
            il.Emit(OpCodes.Ret);
            return (SetFieldValueHandle)dynamicMethod.CreateDelegate(typeof(SetFieldValueHandle));
        }
        /// <summary>
        /// ��ȡ�ֶ�����ֵ��ί��
        /// </summary>
        /// <param name="objType">��������</param>
        /// <param name="fieldName">�ֶ���</param>
        /// <returns></returns>
        public static SetFieldValueHandle GetSetValueHandle(Type objType, string fieldName)
        {
            FieldInfo info = objType.GetField(fieldName, FastValueGetSet.allBindingFlags);
            return GetSetValueHandle(info);
        }
    }
}
