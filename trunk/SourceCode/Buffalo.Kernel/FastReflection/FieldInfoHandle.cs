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
        /// �������Ե���Ϣ��
        /// </summary>
        /// <param name="belong">�ֶ�������������</param>
        /// <param name="getHandle">getί��</param>
        /// <param name="setHandle">setί��</param>
        /// <param name="fieldType">�ֶ���������</param>
        /// <param name="fieldName">�ֶ���</param>
        public FieldInfoHandle(Type belong, GetFieldValueHandle getHandle, SetFieldValueHandle setHandle, Type fieldType, string fieldName)
        {
            this._getHandle = getHandle;
            this._setHandle = setHandle;
            this._fieldType = fieldType;
            this._fieldName = fieldName;
            this._belong = belong;
        }

        /// <summary>
        /// �ֶ���������
        /// </summary>
        public Type Belong
        {
            get
            {
                return _belong;
            }
        }

        /// <summary>
        /// ��ȡ���Ե�����
        /// </summary>
        public Type FieldType
        {
            get
            {
                return _fieldType;
            }
        }
        /// <summary>
        /// ��ȡ���Ե�����
        /// </summary>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
        }

        /// <summary>
        /// ����������ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        public void SetValue(object args, object value) 
        {
            if (_setHandle == null) 
            {
                throw new Exception("������û��Set����");
            }
            _setHandle(args, value);
        }

        /// <summary>
        /// ����������ֵ
        /// </summary>
        /// <param name="args">����</param>
        /// <param name="value">ֵ</param>
        public object GetValue(object args)
        {
            if (_getHandle == null)
            {
                throw new Exception("������û��Get����");
            }
            return _getHandle(args);
        }

        /// <summary>
        /// �Ƿ���Get����
        /// </summary>
        public bool HasGetHandle 
        {
            get 
            {
                return _getHandle != null;
            }
        }

        /// <summary>
        /// �Ƿ���Set����
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
