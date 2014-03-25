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
        }

        #region �������
        /// <summary>
        /// �����б������
        /// </summary>
        /// <param name="sender">������</param>
        public static void FillChildList(string propertyName, EntityBase sender)
        {

            Type senderType = CH.GetRealType(sender);//�����������
            EntityInfoHandle senderHandle = EntityInfoManager.GetEntityHandle(senderType);//��ȡ���������Ϣ
            EntityMappingInfo mappingInfo = senderHandle.MappingInfo[propertyName];
            //if (mappingInfo.GetValue(sender) != null) 
            //{
            //    return;
            //}
            //Dictionary<string, List<object>> dicElement = new Dictionary<string, List<object>>();//����
            ///��ȡ�����Ե�ӳ����Ϣ
            
            //IList baseList = sender.GetBaseList();
            //if (baseList == null)
            //{
            //    baseList = new ArrayList();
            //    baseList.Add(sender);
            //}
            
            if (mappingInfo != null)
            {
                EntityPropertyInfo pkHandle = mappingInfo.SourceProperty;//��ȡʵ���������Ծ��
                //IList pks = CollectPks(baseList, pkHandle, mappingInfo, dicElement, senderHandle.DBInfo);
                object lst=Activator.CreateInstance(mappingInfo.FieldType) ;
                mappingInfo.SetValue(sender, lst);
                object pk = pkHandle.GetValue(sender);
                EntityInfoHandle childHandle = mappingInfo.TargetProperty.BelongInfo;//��ȡ��Ԫ�ص���Ϣ
                FillChilds(pk, childHandle, mappingInfo,sender,propertyName);
            }

        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="childHandle">��Ԫ�ص���Ϣ���</param>
        /// <param name="mappingInfo">ӳ����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        private static void FillChilds(object pk, EntityInfoHandle childHandle, EntityMappingInfo mappingInfo,
            EntityBase sender,string propertyName)
        {
            EntityInfoHandle childInfo = mappingInfo.TargetProperty.BelongInfo;
            DBInfo db = childInfo.DBInfo;
            //if (pks.Count>0)
            //{
                DataBaseOperate oper = childHandle.DBInfo.DefaultOperate;
                BQLDbBase dao = new BQLDbBase(oper);
                ScopeList lstScope = new ScopeList();
                
                sender.OnFillChild(propertyName, lstScope);
                lstScope.AddEqual(mappingInfo.TargetProperty.PropertyName, pk);


                IDataReader reader = dao.QueryReader(lstScope, childInfo.EntityType);
                try
                {
                    string fullName = mappingInfo.TargetProperty.BelongInfo.EntityType.FullName;
                    Type childType = mappingInfo.TargetProperty.BelongInfo.EntityType;

                    //��ȡ�ӱ��get�б�
                    List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, childInfo);//����һ��������ֵ�б�

                    //IList baseList = new ArrayList();//����ĸ��б�
                    IList lst = (IList)mappingInfo.GetValue(sender);
                    while (reader.Read())
                    {
                        object obj = childInfo.CreateSelectProxyInstance();
                        string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                        List<object> curElementObjs = null;


                        //if (dicElement.TryGetValue(fk, out curElementObjs))
                        //{
                        //foreach (object curElementObj in curElementObjs)
                        //{

                        CacheReader.FillObjectFromReader(reader, lstParamNames, obj, db);
                        lst.Add(obj);
                        //baseList.Add(obj);
                        //(obj as EntityBase).SetBaseList(baseList);
                        //}

                        //}
                    }

                }
                finally
                {
                    reader.Close();
                    oper.AutoClose();
                }
            //}
        }



        /// <summary>
        /// �ռ����б������ֶ�ʵ����
        /// </summary>
        /// <param name="curList">��ǰʵ�����һ�β�ѯ�Ľ������</param>
        /// <param name="pkHandle">��ȡ��ֵ�Ĵ���</param>
        /// <param name="mappingInfo">ӳ���������Ϣ</param>
        /// <param name="dicElement">Ԫ���б�</param>
        /// <returns></returns>
        private static IList CollectPks(IList curList, EntityPropertyInfo pkHandle,
            EntityMappingInfo mappingInfo, Dictionary<string, List<object>> dicElement,DBInfo db)
        {
            CreateInstanceHandler classInfo = FastValueGetSet.GetCreateInstanceHandler(mappingInfo.FieldType);
            ArrayList ret = new ArrayList();
            if (pkHandle != null)
            {
                foreach (object obj in curList)
                {
                    ////�ȸ����б���Ԫ��
                    object newObj = classInfo.Invoke();
                    mappingInfo.SetValue(obj, newObj);

                    //��ȡ���������ַ���(�ö��Ÿ���)
                    object curObj = pkHandle.GetValue(obj);
                    if (curObj != null)
                    {
                        ret.Add(curObj);
                        //�ѵ�ǰԪ����������Ϊ��ʶ����Dic���
                        List<object> lst = null;
                        if (!dicElement.TryGetValue(curObj.ToString(), out lst))
                        {
                            lst = new List<object>();
                            dicElement.Add(curObj.ToString(), lst);
                        }
                        lst.Add(obj);
                    }
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
            foreach (EntityPropertyInfo info in classInfo.PropertyInfo)
            {
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
            Type senderType =CH.GetRealType(sender);//����������
            EntityInfoHandle senderInfo = EntityInfoManager.GetEntityHandle(senderType);//��ȡ���������Ϣ
            EntityMappingInfo mappingInfo = senderInfo.MappingInfo[propertyName];

            //if (mappingInfo.GetValue(sender) != null) 
            //{
            //    return;
            //}
            //IList baseList = sender.GetBaseList();//��ȡ��һ�β�ѯ�Ľ������
            //if (baseList == null) 
            //{
            //    baseList = new ArrayList();
            //    baseList.Add(sender);
            //}
            //Dictionary<string, ArrayList> dicElement = new Dictionary<string, ArrayList>();
            
            if (mappingInfo != null)
            {
                EntityPropertyInfo senderHandle = mappingInfo.SourceProperty;//������������Ծ��

                //IList pks = CollectFks(baseList, senderHandle, mappingInfo, dicElement, senderInfo.DBInfo);
                EntityInfoHandle fatherInfo =mappingInfo.TargetProperty.BelongInfo;
                object pk = senderHandle.GetValue(sender);
                FillParent(pk, fatherInfo, mappingInfo,propertyName,sender);
            }

        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="fatherInfo">�����Ӧ�����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        /// <param name="mappingInfo">��ǰ�����Ӧ���Ե�ӳ����Ϣ</param>
        private static void FillParent(object pk, EntityInfoHandle fatherInfo,
             EntityMappingInfo mappingInfo,string propertyName, EntityBase sender)

        {
            DBInfo db = fatherInfo.DBInfo;
            //if (pks.Count>0)
            //{
                DataBaseOperate oper = fatherInfo.DBInfo.DefaultOperate;
                BQLDbBase dao = new BQLDbBase(oper);
                ScopeList lstScope = new ScopeList();

                sender.OnFillParent(propertyName, lstScope);
                lstScope.AddEqual(mappingInfo.TargetProperty.PropertyName, pk);
                IDataReader reader = dao.QueryReader(lstScope, fatherInfo.EntityType);
                try
                {
                    //��ȡ�ӱ��get�б�
                    List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, fatherInfo);//����һ��������ֵ�б�
                    //IList baseList = new ArrayList();//����ĸ��б�
                    //string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();

                    while (reader.Read())
                    {
                        //object obj = createrHandle();

                        //ArrayList lst = null;
                        //if (dicElement.TryGetValue(fk, out lst))
                        //{
                        //foreach (object curObj in lst)
                        //{
                        object newObj = fatherInfo.CreateSelectProxyInstance();
                        //object fatherObj = mappingInfo.GetValue(curObj);
                        mappingInfo.SetValue(sender, newObj);
                        CacheReader.FillObjectFromReader(reader, lstParamNames, newObj, db);
                        //baseList.Add(newObj);
                        //(newObj as EntityBase).SetBaseList(baseList);
                        //}
                        
                        //}
                    }

                }
                finally
                {
                    reader.Close();
                    oper.AutoClose();
                }
            //}
        }

        

        /// <summary>
        /// �ռ����б������ֶ�ʵ����
        /// </summary>
        /// <param name="curList">��ǰ����</param>
        /// <param name="senderHandle">����Ԫ�ص���Ϣ</param>
        /// <param name="mappingInfo">���ö�ӦԪ�صĵ����Ծ��</param>
        /// <param name="dicElement">Ԫ���б�</param>
        /// <returns></returns>
        private static IList CollectFks(IList curList, EntityPropertyInfo senderHandle, 
            EntityMappingInfo mappingInfo, Dictionary<string, ArrayList> dicElement,DBInfo db)
        {
            EntityInfoHandle classInfo = EntityInfoManager.GetEntityHandle(mappingInfo.FieldType);
            ArrayList ret = new ArrayList();
            if (senderHandle != null)
            {
                foreach (object obj in curList)
                {
                    //ʵ��������Ԫ�ض�Ӧ�࣬�������������������ֵ
                    //object newObj = classInfo.CreateProxyInstance();
                    //mappingInfo.SetValue(obj, newObj);

                    //��ȡ��Ӧ�ֶε�ֵ�Է�������ѯ����
                    object curObj = senderHandle.GetValue(obj);
                    if (curObj != null)
                    {
                        string key = curObj.ToString();
                        ArrayList lst = null;
                        if (!dicElement.TryGetValue(key, out lst))
                        {
                            lst = new ArrayList();
                            //�ѵ�ǰԪ����������Ϊ��ʶ����Dic���
                            dicElement.Add(curObj.ToString(), lst);
                            ret.Add(curObj);
                        }

                        lst.Add(obj);
                    }
                }
            }
            return ret;

        }


        #endregion
    }
}
