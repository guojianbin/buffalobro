using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.FastReflection
{
    public class FieldInfoHandle
    {
        private GetFieldValueHandle _getHandle;
        private SetFieldValueHandle _setHandle;
        private Type _fieldType;
        private string _fieldName;
        private Type _belong;
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
    }
}
