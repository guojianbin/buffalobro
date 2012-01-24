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
        /// 名称
        /// </summary>
        [Description("名称")]
        protected string _name = null;
        
        /// <summary>
        /// 用户备注
        /// </summary>
        [Description("用户备注")]
        protected string _remark = null;
        

        /// <summary>
        /// 用户类型
        /// </summary>
        [Description("用户类型")]
        protected ManUserType _userType;
        
        /// <summary>
        /// 班级ID
        /// </summary>
        [Description("班级")]
        protected int? _classId = null;

        /// <summary>
        /// 所属班级
        /// </summary>
        protected ManClass _belongClass = null;
       
        /// <summary>
        /// 头像
        /// </summary>
        [Description("用户头像")]
        protected byte[] _image = null;

        /// <summary>
        /// 年龄
        /// </summary>
        protected int _age;

        /// <summary>
        /// 密码
        /// </summary>
        protected string _password = null;

        /// <summary>
        /// 信息
        /// </summary>
        private List<ManMessage> _lstMessage;
       /// <summary>
       /// 名称
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
       /// 用户备注
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
       /// 用户类型
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
       /// 班级ID
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
       /// 头像
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
       /// 密码
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
       /// 所属班级
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
        /// 信息
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
        /// 年龄
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
