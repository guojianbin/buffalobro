using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Buffalo.Kernel.FastReflection
{
    public class FieldInfoHandle
    {
        private GetFieldValueHandle _getHandle;


        private SetFieldValueHandle _setHandle;

        
        protected Type _fieldType;
        protected string _fieldName;
        protected Type _belong;
        /// <summary>
        /// 创建属性的信息类
        /// </summary>
        /// <param name="belong">字段所属的类类型</param>
        /// <param name="getHandle">get委托</param>
        /// <param name="setHandle">set委托</param>
        /// <param name="fieldType">字段数据类型</param>
        /// <param name="fieldName">字段名</param>
        public FieldInfoHandle(Type belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, Type fieldType, string fieldName)
        {
            this._getHandle = getHandle;
            this._setHandle = setHandle;
            this._fieldType = fieldType;
            this._fieldName = fieldName;
            this._belong = belong;
        }

        /// <summary>
        /// 字段所属的类
        /// </summary>
        public Type Belong
        {
            get
            {
                return _belong;
            }
        }
        /// <summary>
        /// Set句柄
        /// </summary>
        public SetFieldValueHandle SetHandle
        {
            get { return _setHandle; }
        }
        /// <summary>
        /// Get句柄
        /// </summary>
        public GetFieldValueHandle GetHandle
        {
            get { return _getHandle; }
        }
        /// <summary>
        /// 获取属性的类型
        /// </summary>
        public Type FieldType
        {
            get
            {
                return _fieldType;
            }
        }
        /// <summary>
        /// 获取属性的名字
        /// </summary>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }

        /// <summary>
        /// 给对象设置值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        public void SetValue(object args, object value) 
        {
            if (_setHandle == null) 
            {
                throw new Exception("此类型没有Set方法");
            }
            _setHandle(args, value);
        }

        /// <summary>
        /// 给对象设置值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        public object GetValue(object args)
        {
            if (_getHandle == null)
            {
                throw new Exception("此类型没有Get方法");
            }
            return _getHandle(args);
        }

        /// <summary>
        /// 是否有Get方法
        /// </summary>
        public bool HasGetHandle 
        {
            get 
            {
                return _getHandle != null;
            }
        }

        /// <summary>
        /// 是否有Set方法
        /// </summary>
        public bool HasSetHandle
        {
            get
            {
                return _setHandle != null;
            }
        }

        /// <summary>
        /// 获取字段集合
        /// </summary>
        /// <param name="objType">类型</param>
        /// <param name="inner">是否</param>
        /// <returns></returns>
        public static List<FieldInfoHandle> GetFieldInfos(Type objType, BindingFlags flags, bool fillBase)
        {
            List<FieldInfoHandle> lstRet = new List<FieldInfoHandle>();

            Type curType = objType;
            Stack<Type> stkType = new Stack<Type>();
            stkType.Push(curType);
            if (fillBase) 
            {
                curType = curType.BaseType;
                while (curType != null) 
                {
                    stkType.Push(curType);
                    curType = curType.BaseType;
                }
            }
            Dictionary<string, bool> dicExists = new Dictionary<string, bool>();
            while (stkType.Count > 0)
            {
                curType = stkType.Pop();
                FillFieldInfos(curType, flags, lstRet,dicExists);
            }
            return lstRet;
        }
        /// <summary>
        /// 填充值
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="flags"></param>
        /// <param name="fillBase"></param>
        private static void FillFieldInfos(Type objType, BindingFlags flags, List<FieldInfoHandle> lstRet, Dictionary<string, bool> dicExists) 
        {
            FieldInfo[] infos = objType.GetFields(flags);
            foreach (FieldInfo info in infos)
            {
                if (dicExists.ContainsKey(info.Name)) 
                {
                    continue;
                }
                GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(info);
                SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(info);
                FieldInfoHandle handle = new FieldInfoHandle(objType, getHandle, setHandle, info.FieldType, info.Name);
                lstRet.Add(handle);
                dicExists[info.Name] = true;
            }
        }
    }
}
