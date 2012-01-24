using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
namespace TestAddIn
{
    public partial class ManUsers:TestAddIn.ManBase
    {
        /// <summary>
        /// ����
        /// </summary>
        [Description("����")]
        protected string _name = null;
        
        /// <summary>
        /// �û���ע
        /// </summary>
        [Description("�û���ע")]
        protected string _remark = null;
        

        /// <summary>
        /// �û�����
        /// </summary>
        [Description("�û�����")]
        protected ManUserType _userType;
        
        /// <summary>
        /// �༶ID
        /// </summary>
        [Description("�༶")]
        protected int? _classId = null;

        /// <summary>
        /// �����༶
        /// </summary>
        protected ManClass _belongClass = null;
       
        /// <summary>
        /// ͷ��
        /// </summary>
        [Description("�û�ͷ��")]
        protected byte[] _image = null;

        /// <summary>
        /// ����
        /// </summary>
        protected int _age;

        /// <summary>
        /// ����
        /// </summary>
        protected string _password = null;

        /// <summary>
        /// ��Ϣ
        /// </summary>
        private List<ManMessage> _lstMessage;
       /// <summary>
       /// ����
       /// </summary>
       public string Name
       {
           get
           {
               return _name;
           }
           set
           {
               _name=value;
               OnPropertyUpdated("Name");
           }
       }
       /// <summary>
       /// �û���ע
       /// </summary>
       public string Remark
       {
           get
           {
               return _remark;
           }
           set
           {
               _remark=value;
               OnPropertyUpdated("Remark");
           }
       }
       /// <summary>
       /// �û�����
       /// </summary>
       public ManUserType UserType
       {
           get
           {
               return _userType;
           }
           set
           {
               _userType=value;
               OnPropertyUpdated("UserType");
           }
       }
       /// <summary>
       /// �༶ID
       /// </summary>
       public int? ClassId
       {
           get
           {
               return _classId;
           }
           set
           {
               _classId=value;
               OnPropertyUpdated("ClassId");
           }
       }
       /// <summary>
       /// ͷ��
       /// </summary>
       public byte[] Image
       {
           get
           {
               return _image;
           }
           set
           {
               _image=value;
               OnPropertyUpdated("Image");
           }
       }
       /// <summary>
       /// ����
       /// </summary>
       public string Password
       {
           get
           {
               return _password;
           }
           set
           {
               _password=value;
               OnPropertyUpdated("Password");
           }
       }
       /// <summary>
       /// �����༶
       /// </summary>
       public ManClass BelongClass
       {
           get
           {
              if (_belongClass == null)
              {
                  FillParent("BelongClass");
              }
               return _belongClass;
           }
       }
        /// <summary>
        /// ��Ϣ
        /// </summary>
        public List<ManMessage> LstMessage
        {
            get
            {
               if (_lstMessage == null)
               {
                   FillParent("LstMessage");
               }
                return _lstMessage;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age=value;
                OnPropertyUpdated("Age");
            }
        }
    }
}
