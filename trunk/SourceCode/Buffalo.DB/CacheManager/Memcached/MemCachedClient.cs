using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
//using System.Net.Sockets;
using MySql.Data.Common;
using MySql.Data.MySqlClient.Properties;
using MySql.Data.MySqlClient.Memcached;
/** 
 * @原作者:pdonald 
 * @创建时间:2013-4-23
 * @链接:https://github.com/pdonald/mysql-connector-net/blob/master/Source/MySql.Data/Memcached/TextClient.cs
 * @说明:MemCached访问类
*/
namespace Buffalo.DB.CacheManager.Memcached
{
    /// <summary>
    /// memcached访问类
    /// </summary>
    public  class MemCachedClient:TextClient
    {
        private static readonly string PROTOCOL_GETS = "gets";
        private static readonly string VALUE = "VALUE";
        private static readonly string END = "END";
        private static readonly string ERR_ERROR = "ERROR";
        private static readonly string ERR_CLIENT_ERROR = "CLIENT_ERROR";
        private static readonly string ERR_SERVER_ERROR = "SERVER_ERROR";

        private Encoding encoding;
        /// <summary>
        /// memcached访问类
        /// </summary>
        public MemCachedClient(string server, uint port)
            : base(server, port)
        {
            encoding = Encoding.UTF8;
        }

        public override KeyValuePair<string, object> Get(string key)
        {
            KeyValuePair<string, object>[] kvp = Gets(key);
            if (kvp.Length == 0)
                return new KeyValuePair<string,object>(key,null);
            else
                return kvp[0];
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public virtual object GetValue(string key) 
        {
            KeyValuePair<string, object>[] kvp = Gets(key);
            if (kvp.Length == 0)
                return null;
            else
                return kvp[0].Value;
        }
        private KeyValuePair<string, object>[] Gets(params string[] keys)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}", PROTOCOL_GETS));
            for (int i = 0; i < keys.Length; i++)
            {
                sb.Append(string.Format(" {0}", keys[i]));
            }
            sb.Append("\r\n");
            SendData(sb.ToString());
            byte[] res = GetResponse();
            return ParseGetResponse(res);
        }
        private void SendData(string sData)
        {
            byte[] data = encoding.GetBytes(sData);
            stream.Write(data, 0, data.Length);
        }
        private KeyValuePair<string, object>[] ParseGetResponse(byte[] input)
        {
            // VALUE key2 10 9 2\r\n111222333\r\nEND\r\n
            string[] sInput = encoding.GetString(input, 0, input.Length).Split(new string[] { "\r\n" }, StringSplitOptions.None);
            List<KeyValuePair<string, object>> l = new List<KeyValuePair<string, object>>();
            int i = 0;
            string key = "";
            KeyValuePair<string, object> kvp;
            while ((sInput[i] != END) && (i < sInput.Length))
            {
                if (sInput[i].StartsWith(VALUE, StringComparison.OrdinalIgnoreCase))
                {
                    key = sInput[i].Split(' ')[1];
                }
                else
                {
                    kvp = new KeyValuePair<string, object>(key, sInput[i]);
                    l.Add(kvp);
                }
                i++;
            }
            return l.ToArray();
        }
        private byte[] GetResponse()
        {
            byte[] res = new byte[1024];
            MemoryStream ms = new MemoryStream();
            int cnt = stream.Read(res, 0, 1024);
            while (cnt > 0)
            {
                ms.Write(res, 0, cnt);
                if (cnt < 1024) break;
                cnt = stream.Read(res, 0, 1024);
            }
            byte[] res2 = ms.ToArray();
            ValidateErrorResponse(res2);
            return res2;
        }
        private void ValidateErrorResponse(byte[] res)
        {
            string s = encoding.GetString(res, 0, res.Length);
            if ((s.StartsWith(ERR_ERROR, StringComparison.OrdinalIgnoreCase)) ||
                (s.StartsWith(ERR_CLIENT_ERROR, StringComparison.OrdinalIgnoreCase)) ||
                (s.StartsWith(ERR_SERVER_ERROR, StringComparison.OrdinalIgnoreCase)))
            {
                throw new MemberAccessException(s);
            }
        }
    }

   
}
