using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// ϵͳģ��
    /// </summary>
    public partial class BPSysModel:BPBase
    {
        public BPSysModel() { }
        /// <summary>
        /// ϵͳģ��
        /// </summary>
        /// <param name="modelIdentify">ģ���ʶ</param>
        /// <param name="modelName">ģ����</param>
        /// <param name="modelType">ģ������</param>
        /// <param name="modelDescription">ģ��ע��</param>
        public static BPSysModel CreateSysModel(string modelIdentify, string modelName, 
            string modelType,string modelDescription) 
        {
            BPSysModel model = new BPSysModel();
            model.ModelIdentify = modelIdentify;
            model.ModelName = modelName;
            model.ModelType = modelType;
            model.ModelDescription = modelDescription;

            return model;
        }

        /// <summary>
        /// ģ���ʶ
        /// </summary>
        private string _modelIdentify;
        /// <summary>
        /// ģ������
        /// </summary>
        private string _modelName;

        /// <summary>
        /// ģ��ע��
        /// </summary>
        private string _modelDescription;

        /// <summary>
        /// ģ�����
        /// </summary>
        private string _modelType;
        /// <summary>
        /// ģ������
        /// </summary>
        public string ModelName
        {
            get
            {
                return _modelName;
            }
            set
            {
                _modelName=value;
                OnPropertyUpdated("ModelName");
            }
        }
        /// <summary>
        /// ģ��ע��
        /// </summary>
        public string ModelDescription
        {
            get
            {
                return _modelDescription;
            }
            set
            {
                _modelDescription=value;
                OnPropertyUpdated("ModelDescription");
            }
        }
        /// <summary>
        /// ģ�����
        /// </summary>
        public string ModelType
        {
            get
            {
                return _modelType;
            }
            set
            {
                _modelType=value;
                OnPropertyUpdated("ModelType");
            }
        }
        /// <summary>
        /// Ĭ��Ȩ��
        /// </summary>
        private BPModelRights _defaultModelRight;
        

        private List<BPSysModelItem> _lstModelItem;




        private static ModelContext<BPSysModel> _____baseContext=new ModelContext<BPSysModel>();
        /// <summary>
        /// ��ȡ��ѯ������
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPSysModel> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// 
        /// </summary>
        public List<BPSysModelItem> LstModelItem
        {
            get
            {
               if (_lstModelItem == null)
               {
                   FillChild("LstModelItem");
               }
                return _lstModelItem;
            }
        }

        /// <summary>
        /// ����ģ������
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="description"></param>
        public void AddModelItem(string itemIdentify,string itemName, string description) 
        {
            if(_lstModelItem==null)
            {
                _lstModelItem=new List<BPSysModelItem>();
            }

            BPSysModelItem item = new BPSysModelItem();
            item.BelongModel = this;
            item.ItemName = itemName;
            item.ItemIdentify = itemIdentify;
            item.ItemDescription = description;
            
            _lstModelItem.Add(item);
        }

        /// <summary>
        /// ģ���ʶ
        /// </summary>
        public string ModelIdentify
        {
            get
            {
                return _modelIdentify;
            }
            set
            {
                _modelIdentify=value;
                OnPropertyUpdated("ModelIdentify");
            }
        }
        /// <summary>
        /// Ĭ��Ȩ��
        /// </summary>
        public BPModelRights DefaultModelRight
        {
            get
            {
                return _defaultModelRight;
            }
            set
            {
                _defaultModelRight=value;
                OnPropertyUpdated("DefaultModelRight");
            }
        }
    }
}