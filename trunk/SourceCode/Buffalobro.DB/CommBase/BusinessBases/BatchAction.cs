using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.MessageOutPuters;

namespace Buffalobro.DB.CommBase.BusinessBases
{
    /// <summary>
    /// 数据库的批量动作
    /// </summary>
    public class BatchAction : IDisposable
    {

        /// <summary>
        /// 自释放事务类
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="runnow"></param>
        internal BatchAction(DataBaseOperate oper) 
        {
            if (oper.CommitState == CommitState.AutoCommit)
            {
#if DEBUG
                IMessageOutputer outputer=oper.DBInfo.SqlOutputer.DefaultOutputer;
                if ( outputer!= null) 
                {
                    outputer.Output("SQLCommon", oper.DBInfo.Name + "   StarBatchAction");
                }
#endif
                _state = oper.CommitState;
                oper.CommitState = CommitState.UserCommit;
                _oper = oper;
            }
        }


        private DataBaseOperate _oper;
        CommitState _state;

        /// <summary>
        /// 是否当前运行
        /// </summary>
        public bool Runnow
        {
            get { return _oper!=null; }
        }

       

        #region IDisposable 成员

        /// <summary>
        /// 释放事务
        /// </summary>
        public void Dispose()
        {
            EndBatch();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 结束批量操作
        /// </summary>
        private void EndBatch() 
        {
            if (_oper!=null)
            {
#if DEBUG
                IMessageOutputer outputer=_oper.DBInfo.SqlOutputer.DefaultOutputer;
                if (outputer != null)
                {
                    outputer.Output("SQLCommon", _oper.DBInfo.Name + "   EndBatchAction");
                }
#endif
                _oper.CommitState = _state;
                _oper.AutoClose();
            }
        }
        #endregion
    }
}
