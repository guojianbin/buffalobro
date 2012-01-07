using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalobro.DB.EntityInfos
{
    /// <summary>
    /// ӳ���ö����
    /// </summary>
    public class MappingInfoEnumerator : IEnumerator<EntityMappingInfo>
    {
        private Dictionary<string, EntityMappingInfo>.Enumerator enumCurrent;
        private Dictionary<string, EntityMappingInfo> dicCurrent;
        public MappingInfoEnumerator(Dictionary<string, EntityMappingInfo> dicCurrent)
        {
            this.dicCurrent = dicCurrent;
            this.enumCurrent = dicCurrent.GetEnumerator();
        }

        #region IEnumerator<EntityPropertyInfo> ��Ա

        public EntityMappingInfo Current
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
            Dispose();
            enumCurrent = dicCurrent.GetEnumerator();
        }

        #endregion
    }
}
