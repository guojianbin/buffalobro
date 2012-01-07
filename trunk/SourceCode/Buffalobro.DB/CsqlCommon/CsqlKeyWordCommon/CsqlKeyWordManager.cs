using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    public delegate string DelKeyWordHandle(CsqlQuery handle);
    public class CsqlKeyWordManager
    {

        /// <summary>
        /// 创建查询的中转信息类
        /// </summary>
        /// <returns></returns>
        private static KeyWordInfomation CreateKeywordInfo(DBInfo db) 
        {
            KeyWordInfomation info = new KeyWordInfomation();
            info.DBInfo = db;
            info.Infos = new CsqlInfos();
            return info;
        }
        /// <summary>
        /// 输出关键字转换后的SQL语句
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static AbsCondition ToCondition(CsqlQuery item, DBInfo db, TableAliasNameManager aliasManager, bool isPutPropertyName)
        {
            KeyWordInfomation info = CreateKeywordInfo(db);
            info.AliasManager = aliasManager;
            info.Infos.IsPutPropertyName = isPutPropertyName;
            return DoConver(info, item);
        }
        /// <summary>
        /// 进行转换
        /// </summary>
        /// <param name="info"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static AbsCondition DoConver(KeyWordInfomation info, CsqlQuery item) 
        {
            KeyWordConver conver = new KeyWordConver();
            AbsCondition con=conver.ToConver(item, info);
            con.AliasManager = info.AliasManager;
            con.DbParamList = info.ParamList;
            return con;
        }
    }
}
