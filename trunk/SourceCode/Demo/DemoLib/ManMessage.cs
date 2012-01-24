using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
namespace TestAddIn
{
    public partial class ManMessage : ManBase
    {
        /// <summary>
        /// 标题
        /// </summary>
        private string _title;

        /// <summary>
        /// 内容
        /// </summary>
        private string _content;


        private int _belongUser;

        private ManUsers _belong;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyUpdated("Title");
            }
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyUpdated("Content");
            }
        }
       /// <summary>
       /// 
       /// </summary>
       public int BelongUser
       {
           get
           {
               return _belongUser;
           }
           set
           {
               _belongUser=value;
               OnPropertyUpdated("BelongUser");
           }
       }
       /// <summary>
       /// 
       /// </summary>
       public ManUsers Belong
       {
           get
           {
              if (_belong == null)
              {
                  FillParent("Belong");
              }
               return _belong;
           }
       }
    }
}
