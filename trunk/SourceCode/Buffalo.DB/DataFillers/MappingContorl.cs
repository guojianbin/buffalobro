using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Buffalo.DB.CommBase;
using Buffalo.DB.PropertyAttributes;
using System.Collections;
using Buffalo.DB.EntityInfos;
using System.Diagnostics;
using System.Reflection;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
namespace Buffalo.DB.DataFillers
{
    /// <summary>
    /// �������͸������
    /// </summary>
    public class MappingContorl
    {
        /// <summary>
        /// ��ȡ��ǰ��������������
        /// </summary>
        /// <returns></returns>
        private static string GetPropertyName ()
        {
            StackTrace st = new StackTrace(true);
            for (int i = 1; i < st.FrameCount; i++)
            {
                StackFrame frame = st.GetFrame(i);//��ȡ���ô˺����ĺ���
                MethodBase method = frame.GetMethod();
                
                
                string mname=method.Name;
                if (mname.IndexOf("get_") == 0) 
                {
                    return mname.Substring(4);
                }
            }
            throw new Exception("�Ҳ�������");
            //return null;
        }

        #region �������
        /// <summary>
        /// �����б������
        /// </summary>
        /// <param name="sender">������</param>
        public static void FillChildList(string propertyName, EntityBase sender)
        {


            Type senderType = sender.GetType();//�����������

            ///��ȡ�����Ե�ӳ����Ϣ
            
            EntityInfoHandle senderHandle = EntityInfoManager.GetEntityHandle(senderType);//��ȡ���������Ϣ

            EntityMappingInfo mappingInfo = senderHandle.MappingInfo[propertyName];
            if (mappingInfo != null)
            {
                
                
                EntityPropertyInfo pkHandle = mappingInfo.SourceProperty;//��ȡʵ���������Ծ��

                IList pks = CollectPks(sender, pkHandle, mappingInfo, senderHandle.DBInfo);
                EntityInfoHandle childHandle = mappingInfo.TargetProperty.BelongInfo;//��ȡ��Ԫ�ص���Ϣ
                FillChilds(pks, childHandle, mappingInfo,sender,propertyName);
            }

        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="childHandle">��Ԫ�ص���Ϣ���</param>
        /// <param name="mappingInfo">ӳ����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        private static void FillChilds(IList pks, EntityInfoHandle childHandle, EntityMappingInfo mappingInfo, EntityBase sender,string propertyName)
        {
            EntityInfoHandle childInfo = mappingInfo.TargetProperty.BelongInfo;
            DBInfo db = childInfo.DBInfo;
            if (pks.Count>0)
            {
                DataBaseOperate oper = childHandle.DBInfo.DefaultOperate;
                BQLDbBase dao = new BQLDbBase(oper);
                ScopeList lstScope = new ScopeList();
                
                sender.OnFillChild(propertyName, lstScope);
                lstScope.AddIn(mappingInfo.TargetProperty.PropertyName, pks);
                IDataReader reader = dao.QueryReader(lstScope, childInfo.EntityType);

                try
                {
                    string fullName = mappingInfo.TargetProperty.BelongInfo.EntityType.FullName;
                    Type childType = mappingInfo.TargetProperty.BelongInfo.EntityType;

                    //��ȡ�ӱ��get�б�
                    List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, childInfo);//����һ��������ֵ�б�

                    IList baseList = new ArrayList();//����ĸ��б�
                    IList lst = (IList)mappingInfo.GetValue(sender);
                    while (reader.Read())
                    {
                        object obj = childInfo.CreateInstance();

                        CacheReader.FillObjectFromReader(reader, lstParamNames, obj, db);
                        lst.Add(obj);
                    }

                }
                finally
                {
                    reader.Close();
                    oper.AutoClose();
                }
            }
        }



        /// <summary>
        /// �ռ����б������ֶ�ʵ����
        /// </summary>
        /// <param name="curList">��ǰʵ�����һ�β�ѯ�Ľ������</param>
        /// <param name="pkHandle">��ȡ��ֵ�Ĵ���</param>
        /// <param name="mappingInfo">ӳ���������Ϣ</param>
        /// <param name="dicElement">Ԫ���б�</param>
        /// <returns></returns>
        private static IList CollectPks(object sender, EntityPropertyInfo pkHandle,
            EntityMappingInfo mappingInfo,DBInfo db)
        {
            CreateInstanceHandler classInfo = FastValueGetSet.GetCreateInstanceHandler(mappingInfo.FieldType);
            ArrayList ret = new ArrayList();
            if (pkHandle != null)
            {
                
                    //�ȸ����б���Ԫ��
                    object newObj = classInfo.Invoke();
                    mappingInfo.SetValue(sender, newObj);

                    //��ȡ���������ַ���(�ö��Ÿ���)
                    object curObj = pkHandle.GetValue(sender);
                    if (curObj != null)
                    {
                        ret.Add(curObj);
                        
                    }
                
            }
            return ret;

        }

        /// <summary>
        /// ��ȡָ����Ӧ�ֶ��������Ծ��
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="pkName">�ֶ���</param>
        /// <returns></returns>
        private static EntityPropertyInfo GetPkHandle(Type type, string pkName)
        {
            EntityInfoHandle classInfo = EntityInfoManager.GetEntityHandle(type);
            //Dictionary<string, EntityPropertyInfo>.Enumerator enums = classInfo.PropertyInfo.GetPropertyEnumerator();
            foreach (EntityPropertyInfo info in classInfo.PropertyInfo)
            {
                //EntityPropertyInfo info = enums.Current.Value;
                if (pkName == info.ParamName) 
                {
                    return info;
                }
            }
            return null;
        }

        
        #endregion

        #region ��丸��

        /// <summary>
        /// �����б������
        /// </summary>
        /// <param name="sender">������</param>
        public static void FillParent(string propertyName, EntityBase sender)
        {

            Type senderType = sender.GetType();//����������
            EntityInfoHandle senderInfo = EntityInfoManager.GetEntityHandle(senderType);//��ȡ���������Ϣ

            EntityMappingInfo mappingInfo = senderInfo.MappingInfo[propertyName];
            if (mappingInfo != null)
            {

                
                EntityPropertyInfo senderHandle = mappingInfo.SourceProperty;//������������Ծ��

                IList pks = CollectFks(sender, senderHandle, mappingInfo, senderInfo.DBInfo);
                EntityInfoHandle fatherInfo =mappingInfo.TargetProperty.BelongInfo;
                FillParent(pks, fatherInfo, mappingInfo,propertyName,sender);
            }

        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="fatherInfo">�����Ӧ�����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        /// <param name="mappingInfo">��ǰ�����Ӧ���Ե�ӳ����Ϣ</param>
        private static void FillParent(IList pks, EntityInfoHandle fatherInfo, 
            EntityMappingInfo mappingInfo, string propertyName, EntityBase sender)
        {
            DBInfo db = fatherInfo.DBInfo;
            if (pks.Count>0)
            {
                DataBaseOperate oper = fatherInfo.DBInfo.DefaultOperate;
                BQLDbBase dao = new BQLDbBase(oper);
                ScopeList lstScope = new ScopeList();

                sender.OnFillParent(propertyName, lstScope);
                lstScope.AddIn(mappingInfo.TargetProperty.PropertyName, pks);

                IDataReader reader = dao.QueryReader(lstScope, fatherInfo.EntityType);
                try
                {
                    //��ȡ�ӱ��get�б�
                    List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, fatherInfo);//����һ��������ֵ�б�
                    IList baseList = new ArrayList();//����ĸ��б�

                    if (reader.Read())
                    {
                        string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                        object fatherObj = mappingInfo.GetValue(sender);
                        CacheReader.FillObjectFromReader(reader, lstParamNames, fatherObj, db);
                    }

                }
                finally
                {
                    reader.Close();
                    oper.AutoClose();
                }
            }
        }

        

        /// <summary>
        /// �ռ����б������ֶ�ʵ����
        /// </summary>
        /// <param name="curList">��ǰ����</param>
        /// <param name="senderHandle">����Ԫ�ص���Ϣ</param>
        /// <param name="mappingInfo">���ö�ӦԪ�صĵ����Ծ��</param>
        /// <param name="dicElement">Ԫ���б�</param>
        /// <returns></returns>
        private static IList CollectFks(EntityBase sender, EntityPropertyInfo senderHandle,
            EntityMappingInfo mappingInfo, DBInfo db)
        {
            EntityInfoHandle classInfo = EntityInfoManager.GetEntityHandle(mappingInfo.FieldType);
            ArrayList ret = new ArrayList();
            if (senderHandle != null)
            {

                //ʵ��������Ԫ�ض�Ӧ�࣬�������������������ֵ
                object newObj = classInfo.CreateInstance();
                mappingInfo.SetValue(sender, newObj);

                //��ȡ��Ӧ�ֶε�ֵ�Է�������ѯ����
                object curObj = senderHandle.GetValue(sender);
                if (curObj != null)
                {
                    ret.Add(curObj);
                }
            }
            return ret;

        }


        #endregion
    }
}
