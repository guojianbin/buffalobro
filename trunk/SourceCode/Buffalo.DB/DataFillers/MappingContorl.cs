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

            //string propertyName = GetPropertyName();
            Type senderType = sender.GetType();//�����������
            //PropertyInfo pinf = senderType.GetProperty(propertyName);//��ȡ��Ԫ�ؼ��ϵ�����
            Dictionary<string, List<object>> dicElement = new Dictionary<string, List<object>>();//����
            ///��ȡ�����Ե�ӳ����Ϣ
            //TableMappingAttribute tma = GetMappingParam(pinf);
            EntityInfoHandle senderHandle = EntityInfoManager.GetEntityHandle(senderType);//��ȡ���������Ϣ
            IList baseList = (IList)senderHandle.BaseListInfo.GetValue(sender);
            if (baseList == null)
            {
                baseList = new ArrayList();
                baseList.Add(sender);
            }
            EntityMappingInfo mappingInfo = senderHandle.MappingInfo[propertyName];
            if (mappingInfo != null)
            {
                
                //PropertyInfoHandle childListHandle = EntityInfoManager.GetPropertyInfoHandle(propertyName, senderType);//�����get����
                EntityPropertyInfo pkHandle = mappingInfo.SourceProperty;//��ȡʵ���������Ծ��
                //EntityPropertyInfo curChildListSetter = classHandle[propertyName];//��ȡ��ǰ�б����
                IList pks = CollectPks(baseList, pkHandle, mappingInfo, dicElement, senderHandle.DBInfo);
                EntityInfoHandle childHandle = mappingInfo.TargetProperty.BelongInfo;//��ȡ��Ԫ�ص���Ϣ
                FillChilds(pks, childHandle, mappingInfo, dicElement,sender,propertyName);
            }

        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="childHandle">��Ԫ�ص���Ϣ���</param>
        /// <param name="mappingInfo">ӳ����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        private static void FillChilds(IList pks, EntityInfoHandle childHandle, EntityMappingInfo mappingInfo, Dictionary<string, List<object>> dicElement, EntityBase sender,string propertyName)
        {
            EntityInfoHandle childInfo = mappingInfo.TargetProperty.BelongInfo;
            DBInfo db = childInfo.DBInfo;
            if (pks.Count>0)
            {
                DataBaseOperate oper = StaticConnection.GetStaticOperate(childHandle.DBInfo);
                BQLDbBase dao = new BQLDbBase(oper);
                ScopeList lstScope = new ScopeList();
                
                sender.OnFillChild(propertyName, lstScope);
                lstScope.AddIn(mappingInfo.TargetProperty.PropertyName, pks);
                //StringBuilder where =new StringBuilder(500);
                
                //where.Append(db.CurrentDbAdapter.FormatParam(mappingInfo.TargetProperty.ParamName) + " in(" + pks + ")");
                //if (lstScope.Count > 0)
                //{
                //    where.Append(DataAccessCommon.FillCondition(childInfo, null, lstScope));
                //}
                //string sql = "select * from " + db.CurrentDbAdapter.FormatTableName(childHandle.TableName) + " where " + where.ToString();
                
                //DataBaseOperate oper = StaticConnection.GetStaticOperate(db);
                //oper.ConnectionStringKey = childHandle.ConnectionKey;

                IDataReader reader = dao.QueryReader(lstScope, childInfo.EntityType);

                //IDataReader reader = oper.Query(sql, null);
                try
                {
                    string fullName = mappingInfo.TargetProperty.BelongInfo.EntityType.FullName;
                    Type childType = mappingInfo.TargetProperty.BelongInfo.EntityType;

                    //��ȡ�ӱ��get�б�
                    List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, childInfo);//����һ��������ֵ�б�

                    IList baseList = new ArrayList();//����ĸ��б�
                    while (reader.Read())
                    {
                        object obj = childInfo.CreateInstance();
                        string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                        List<object> curElementObjs = null;


                        if (dicElement.TryGetValue(fk, out curElementObjs))
                        {
                            foreach (object curElementObj in curElementObjs)
                            {
                                IList lst = (IList)mappingInfo.GetValue(curElementObj);
                                CacheReader.FillObjectFromReader(reader, lstParamNames, obj, db);
                                lst.Add(obj);
                                baseList.Add(obj);
                                childInfo.BaseListInfo.SetValue(obj, baseList);
                            }
                            
                        }
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
        private static IList CollectPks(IList curList, EntityPropertyInfo pkHandle,
            EntityMappingInfo mappingInfo, Dictionary<string, List<object>> dicElement,DBInfo db)
        {
            CreateInstanceHandler classInfo = FastValueGetSet.GetCreateInstanceHandler(mappingInfo.FieldType);
            ArrayList ret = new ArrayList();
            if (pkHandle != null)
            {
                foreach (object obj in curList)
                {
                    //�ȸ����б���Ԫ��
                    object newObj = classInfo.Invoke();
                    mappingInfo.SetValue(obj, newObj);

                    //��ȡ���������ַ���(�ö��Ÿ���)
                    object curObj = pkHandle.GetValue(obj);
                    if (curObj != null)
                    {
                        //string pk = DataAccessCommon.FormatValue(curObj, pkHandle.SqlType, db);
                        //ret.Add(curObj);
                        ret.Add(curObj);
                        //ret.Append(",");
                        //�ѵ�ǰԪ����������Ϊ��ʶ����Dic���
                        List<object> lst = null;
                        if (!dicElement.TryGetValue(curObj.ToString(), out lst))
                        {
                            lst = new List<object>();
                            dicElement.Add(curObj.ToString(), lst);
                        }
                        lst.Add(obj);
                    }
                    //dicElement.Add(curObj.ToString(), obj);
                }
            }
            //if (ret.Length > 0) 
            //{
            //    ret.Remove(ret.Length - 1, 1);
            //}
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
            //string propertyName = GetPropertyName();
            Type senderType = sender.GetType();//����������
            EntityInfoHandle senderInfo = EntityInfoManager.GetEntityHandle(senderType);//��ȡ���������Ϣ
            IList baseList = (IList)senderInfo.BaseListInfo.GetValue(sender);//��ȡ��һ�β�ѯ�Ľ������
            if (baseList == null) 
            {
                baseList = new ArrayList();
                baseList.Add(sender);
            }
            //PropertyInfo pinf = senderType.GetProperty(propertyName);//��ȡ����Ԫ�ص�����
            Dictionary<string, ArrayList> dicElement = new Dictionary<string, ArrayList>();
            //TableMappingAttribute tma = GetMappingParam(pinf);//��ȡ����ӳ�����Ϣ
            EntityMappingInfo mappingInfo = senderInfo.MappingInfo[propertyName];
            if (mappingInfo != null)
            {

                //PropertyInfoHandle fatherHandle = FastValueGetSet.GetPropertyInfoHandle(propertyName, senderType);//�����Ӧ������Դ���
                EntityPropertyInfo senderHandle = mappingInfo.SourceProperty;//������������Ծ��

                IList pks = CollectFks(baseList, senderHandle, mappingInfo, dicElement, senderInfo.DBInfo);
                EntityInfoHandle fatherInfo =mappingInfo.TargetProperty.BelongInfo;
                FillParent(pks, fatherInfo, dicElement, mappingInfo,propertyName,sender);
            }

        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="fatherInfo">�����Ӧ�����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        /// <param name="mappingInfo">��ǰ�����Ӧ���Ե�ӳ����Ϣ</param>
        private static void FillParent(IList pks, EntityInfoHandle fatherInfo, Dictionary<string, ArrayList> dicElement, EntityMappingInfo mappingInfo, string propertyName, EntityBase sender)
        {
            DBInfo db = fatherInfo.DBInfo;
            if (pks.Count>0)
            {
                DataBaseOperate oper = StaticConnection.GetStaticOperate(fatherInfo.DBInfo);
                BQLDbBase dao = new BQLDbBase(oper);
                ScopeList lstScope = new ScopeList();

                sender.OnFillParent(propertyName, lstScope);
                lstScope.AddIn(mappingInfo.TargetProperty.PropertyName, pks);
                //where.Append(db.CurrentDbAdapter.FormatParam(mappingInfo.TargetProperty.ParamName) + " in(" + pks + ")");
                //if (lstScope.Count > 0)
                //{
                //    where.Append(DataAccessCommon.FillCondition(fatherInfo, null, lstScope));
                //}
                //string sql = "select * from " + db.CurrentDbAdapter.FormatTableName(fatherInfo.TableName) + " where " +where.ToString();
                //DataBaseOperate oper = StaticConnection.GetStaticOperate(db);
                //IDataReader reader = oper.Query(sql, null);
                IDataReader reader = dao.QueryReader(lstScope, fatherInfo.EntityType);
                try
                {
                    //��ȡ�ӱ��get�б�
                    List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, fatherInfo);//����һ��������ֵ�б�
                    IList baseList = new ArrayList();//����ĸ��б�

                    while (reader.Read())
                    {
                        //object obj = createrHandle();
                        string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                        ArrayList lst = null;
                        if (dicElement.TryGetValue(fk, out lst))
                        {
                            //IList lst = (IList)curFatherGetter(curElementObj, new object[] { });
                            foreach (object curObj in lst)
                            {
                                object fatherObj = mappingInfo.GetValue(curObj);
                                CacheReader.FillObjectFromReader(reader, lstParamNames, fatherObj, db);
                                baseList.Add(fatherObj);
                                fatherInfo.BaseListInfo.SetValue(fatherObj, baseList);
                                //((EntityBase)fatherObj).SetBaseList(baseList);
                            }

                        }
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
                    object newObj = classInfo.CreateInstance();
                    mappingInfo.SetValue(obj, newObj);

                    //��ȡ��Ӧ�ֶε�ֵ�Է�������ѯ����
                    object curObj = senderHandle.GetValue(obj);
                    if (curObj != null)
                    {
                        //string fk = DataAccessCommon.FormatValue(curObj, senderHandle.SqlType, db);
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
                //if (ret.Length > 0)
                //{
                //    ret = ret.Substring(0, ret.Length - 1);
                //}
            }
            return ret;

        }


        #endregion
    }
}
