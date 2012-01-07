using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.EntityInfos
{
    /// <summary>
    /// ���Ե�ö����
    /// </summary>
    public class PropertyEnumerator : IEnumerator<EntityPropertyInfo>
    {
        private Dictionary<string, EntityPropertyInfo>.Enumerator enumCurrent;
        private Dictionary<string, EntityPropertyInfo> dicCurrent;
        public PropertyEnumerator(Dictionary<string, EntityPropertyInfo> dicCurrent)
        {
            this.dicCurrent = dicCurrent;
            this.enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> ��Ա

        public EntityPropertyInfo Current
        {
            get 
            {
                return enumCurrent.Current.Value;
            }
        }

        #endregion

        #region IDisposable ��Ա

        public void Dispose()
        {
            enumCurrent.Dispose();
        }

        #endregion

        #region IEnumerator ��Ա

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                return enumCurrent.Current.Value;
            }
        }

        public bool MoveNext()
        {
            return enumCurrent.MoveNext();
        }

        public void Reset()
        {
            enumCurrent = dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
