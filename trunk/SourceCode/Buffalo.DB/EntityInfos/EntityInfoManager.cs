using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CacheManager;
using System.Reflection;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;


namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ʵ�����Թ���
    /// </summary>
    public class EntityInfoManager
    {
        private static Dictionary<string, EntityInfoHandle> _dicClass = new Dictionary<string, EntityInfoHandle>();//��¼�Ѿ���ʼ����������

        /// <summary>
        /// ��ȡʵ������ߵ�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type)
        {
            string fullName = type.FullName;
            EntityInfoHandle classHandle = null;

            if (!DataAccessLoader.HasInit)
            {
                DataAccessLoader.InitConfig();
            }
            if (!_dicClass.TryGetValue(fullName, out classHandle))
            {
                throw new Exception("�Ҳ���ʵ��" + fullName + "���������ļ�");
            }

            return classHandle;
        }

        /// <summary>
        /// ����ʵ�����Ϣ
        /// </summary>
        internal static Dictionary<string, EntityInfoHandle> AllEntity
        {
            get 
            {
                return _dicClass;
            }
        }

        /// <summary>
        /// ���ö�����Ϣ
        /// </summary>
        /// <param name="type"></param>
        /// <param name="db"></param>
        internal static void SetEntityHandle(Type type, DBInfo db) 
        {
            InitEntityPropertyInfos(type, db);
        }

        /// <summary>
        /// ��ʼ�����͵�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>����Ѿ���ʼ�����෵��false</returns>
        private static void InitEntityPropertyInfos(Type type,DBInfo db)
        {
            IDBAdapter idb = db.CurrentDbAdapter;
            string fullName = type.FullName;
            TableAttribute tableAtt = AttributesGetter.GetTableAttribute(type);
            //ʵ���������͵ľ��
            CreateInstanceHandler createrHandle = FastValueGetSet.GetCreateInstanceHandlerWithOutCache(type);
            Dictionary<string, EntityPropertyInfo> dicPropertys = new Dictionary<string, EntityPropertyInfo>();
            Dictionary<string, EntityMappingInfo> dicMapping = new Dictionary<string, EntityMappingInfo>();
            //������Ϣ���
            FieldInfo[] destproper = type.GetFields(FastValueGetSet.allBindingFlags);
            

            EntityInfoHandle classInfo = new EntityInfoHandle(type, createrHandle,tableAtt.TableName, db);

            FieldInfoHandle baseListHandle = null;
            using (DataBaseOperate oper = new DataBaseOperate(db))
            {
                ///��ȡ���Ա���
                foreach (FieldInfo finf in destproper)
                {
                    if (finf.Name == "_search_baselist_")
                    {
                        GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                        SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                        baseListHandle = new FieldInfoHandle(classInfo.EntityType,getHandle, setHandle, finf.FieldType, finf.Name);
                    }
                    else
                    {
                        ///ͨ������������
                        EntityParam ep = AttributesGetter.GetEntityParam(finf);


                        if (ep != null)
                        {
                            if (tableAtt.IsParamNameUpper)
                            {
                                ep.ParamName = ep.ParamName.ToUpper();
                            }

                            if (ep.Identity) //��Oracle������������
                            {
                                string seqName = idb.GetSequenceName(tableAtt.TableName, ep.ParamName);
                                if (seqName != null)//�����Oracle��
                                {
                                    idb.InitSequence(seqName, oper);
                                }
                            }
                            string proName = ep.PropertyName;
                            GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                            SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                            if (getHandle != null || setHandle != null)
                            {
                                EntityPropertyInfo entityProperty = new EntityPropertyInfo(classInfo,getHandle, setHandle, ep, finf.FieldType, finf.Name,db);
                                dicPropertys.Add(proName, entityProperty);
                            }
                        }
                        else
                        {
                            TableRelationAttribute tableMappingAtt = GetMappingParam(finf);

                            if (tableMappingAtt != null)
                            {
                                Type targetType=DefaultType.GetRealValueType(finf.FieldType);
                                tableMappingAtt.SetEntity(type, targetType);
                                GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                                SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                                EntityMappingInfo entityMappingInfo = new EntityMappingInfo(type,getHandle, setHandle, tableMappingAtt, finf.Name, finf.FieldType);
                                dicMapping.Add(tableMappingAtt.PropertyName, entityMappingInfo);
                            }
                            
                        }
                    }
                }
            }
            classInfo.SetInfoHandles(dicPropertys, dicMapping, baseListHandle);

            _dicClass[fullName]=classInfo;
        }

        

        /// <summary>
        /// ��ȡĳ������������
        /// </summary>
        /// <param name="finf"></param>
        /// <returns></returns>
        private static TableRelationAttribute GetMappingParam(FieldInfo finf)
        {
            object entityParam = FastInvoke.GetPropertyAttribute(finf, typeof(TableRelationAttribute));
            if (entityParam != null)
            {
                return (TableRelationAttribute)entityParam;
            }
            return null;
        }
    }
}
