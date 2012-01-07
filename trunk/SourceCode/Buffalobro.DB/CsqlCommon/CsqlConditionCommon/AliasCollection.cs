using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class AliasCollection : IEnumerable<QueryParamCollection>
    {
        private Dictionary<string, QueryParamCollection> dicAliass = new Dictionary<string, QueryParamCollection>();//�˲�ѯ�����ص��ֶ�

        /// <summary>
        /// ��ȡ��������Ӧ���ֶ���
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal QueryParamCollection this[string aliasName]
        {
            get
            {
                QueryParamCollection ret = null;
                dicAliass.TryGetValue(aliasName, out ret);
                return ret;
            }
            set
            {
                dicAliass[aliasName] = value;
            }
        }

        #region IEnumerable<string> ��Ա

        public IEnumerator<QueryParamCollection> GetEnumerator()
        {
            return new AliasEnumerator(dicAliass);
        }

        #endregion

        #region IEnumerable ��Ա

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new AliasEnumerator(dicAliass);
        }

        #endregion
  
    }
}
