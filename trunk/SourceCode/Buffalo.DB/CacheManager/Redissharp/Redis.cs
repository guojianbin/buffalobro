//
// redis-sharp.cs: ECMA CLI Binding to the Redis key-value storage system
//
// Authors:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010 Novell, Inc.
//
// Licensed under the same terms of reddis: new BSD license.
//
#define DEBUG

using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using System.Data;
using MemcacheClient;
//using System.Linq;
namespace Redissharp
{
    public enum KeyType
    {
        None, String, List, Set
    }

    public class Redis : IDisposable
    {
        Socket _socket;
        BufferedStream _bstream;

        
        public class ResponseException : Exception
        {
            public ResponseException(string code)
                : base("Response error")
            {
                _code = code;
            }
            private string _code;
            public string Code { get { return _code; } private set { _code = value; } }
        }
        private SockPool _pool;

        public Redis(SockPool pool)
        {
            
            _pool = pool;
            
        }
        public string _host;
        public string Host { get { return _host; } private set { _host = value; } }
        public int _port;
        public int Port { get { return _port; } private set { _port = value; } }
        public int _retryTimeout;
        public int RetryTimeout { get { return _retryTimeout; } set { _retryTimeout = value; } }
        public int _retryCount;
        public int RetryCount { get { return _retryCount; } set { _retryCount = value; } }
        public int _sendTimeout;
        public int SendTimeout { get { return _sendTimeout; } set { _sendTimeout = value; } }
        public string _password;
        public string Password { get { return _password; } set { _password = value; } }

        int db;
        public int Db
        {
            get
            {
                return db;
            }

            set
            {
                db = value;
                SendExpectSuccess("SELECT {0}\r\n", db);
            }
        }
        /// <summary>
        /// 开启连接
        /// </summary>
        public void Open(string key) 
        {
            _socket = _pool.GetSock(key);
            _bstream = new BufferedStream(new NetworkStream(_socket), 16 * 1024);
        }
        /// <summary>
        /// 开启连接
        /// </summary>
        public void Open()
        {
            Open("anyKey");
        }
        public void Close() 
        {
            if (_socket != null)
            {
                _pool.CheckIn(_socket);
                
                _bstream.Flush();
                _bstream.Dispose();
                _socket = null;
                _bstream = null;
            }
        }
        /// <summary>
        /// closes socket and all streams connected to it 
        /// </summary>
        public void TrueClose()
        {
            try
            {
                SendCommand("QUIT\r\n");
            }
            catch (Exception ex) { }
            finally
            {
                _socket.Close();
                _socket = null;
            }
        }

        public string this[string key]
        {
            get { return GetString(key); }
            set { Set(key, value); }
        }

        public void Set(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            Set(key, Encoding.UTF8.GetBytes(value));
        }

        public void Set(string key, byte[] value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length > 1073741824)
                throw new ArgumentException("value exceeds 1G", "value");

            if (!SendDataCommand(value, "SET {0} {1}\r\n", key, value.Length))
                throw new Exception("Unable to connect");
            ExpectSuccess();
        }

        public bool SetNX(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            return SetNX(key, Encoding.UTF8.GetBytes(value));
        }

        public bool SetNX(string key, byte[] value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length > 1073741824)
                throw new ArgumentException("value exceeds 1G", "value");

            return SendDataExpectInt(value, "SETNX {0} {1}\r\n", key, value.Length) > 0 ? true : false;
        }

        //public void Set (IDictionary<string,string> dict)
        //{
        //  Set(dict.ToDictionary(k => k.Key, v => Encoding.UTF8.GetBytes(v.Value)));
        //}

        private static readonly byte[] _nl = Encoding.UTF8.GetBytes("\r\n");

        /// <summary>
        /// 设置字节数组
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetBytes(string key, byte[] val) 
        {
            MemoryStream ms = new MemoryStream();
            byte[] kLength = Encoding.UTF8.GetBytes("$" + key.Length + "\r\n");
            byte[] k = Encoding.UTF8.GetBytes(key + "\r\n");
            byte[] vLength = Encoding.UTF8.GetBytes("$" + val.Length + "\r\n");
            ms.Write(kLength, 0, kLength.Length);
            ms.Write(k, 0, k.Length);
            ms.Write(vLength, 0, vLength.Length);
            ms.Write(val, 0, val.Length);
            ms.Write(_nl, 0, _nl.Length);
            SendDataCommand(ms.ToArray(), "*" + (2 + 1) + "\r\n$4\r\nMSET\r\n");
            ExpectSuccess();
        }
        static Encoding _IntEncoding = Encoding.ASCII;
        /// <summary>
        /// 设置值类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetValue(string key, object val)
        {
            byte[] bval = _IntEncoding.GetBytes(val.ToString());
            SetBytes(key, bval);
        }

        public void Set(IDictionary<string, byte[]> dict)
        {
            if (dict == null)
                throw new ArgumentNullException("dict");

            //byte[] nl = Encoding.UTF8.GetBytes("\r\n");

            MemoryStream ms = new MemoryStream();
            foreach (string key in dict.Keys)
            {
                byte[] val = dict[key];

                byte[] kLength = Encoding.UTF8.GetBytes("$" + key.Length + "\r\n");
                byte[] k = Encoding.UTF8.GetBytes(key + "\r\n");
                byte[] vLength = Encoding.UTF8.GetBytes("$" + val.Length + "\r\n");
                ms.Write(kLength, 0, kLength.Length);
                ms.Write(k, 0, k.Length);
                ms.Write(vLength, 0, vLength.Length);
                ms.Write(val, 0, val.Length);
                ms.Write(_nl, 0, _nl.Length);
            }

            SendDataCommand(ms.ToArray(), "*" + (dict.Count * 2 + 1) + "\r\n$4\r\nMSET\r\n");
            ExpectSuccess();
        }
        /// <summary>
        /// 设置DataSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public void SetDataSet(string key, DataSet val)
        {
            byte[] bval = MemDataSerialize.DataSetToBytes(val);
            SetBytes(key, bval);
        }
        /// <summary>
        /// 获取DataSet
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string key)
        {
            if (!SendDataCommand(null, "GET " + key + "\r\n"))
                throw new Exception("Unable to connect");
            string r = ReadLine();

            if (r.Length == 0)
                throw new ResponseException("Zero length respose");

            char c = r[0];
            if (c == '-')
                throw new ResponseException(r.StartsWith("-ERR") ? r.Substring(5) : r.Substring(1));

            if (c == '$')
            {
                if (r == "$-1")
                    return null;
                int n;

                if (Int32.TryParse(r.Substring(1), out n))
                {
                    DataSet ds = MemDataSerialize.LoadDataSet(_bstream);
                    return ds;
                }
                throw new ResponseException("Invalid length");
            }

           
            throw new ResponseException("Unexpected reply: " + r);
        }

        public byte[] Get(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectData(null, "GET " + key + "\r\n");
        }

        public string GetString(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            byte[] content = Get(key);
            if (content == null || content.Length <= 0) 
            {
                return null;
            }
            return Encoding.UTF8.GetString(content);
        }

        public byte[][] Sort(SortOptions options)
        {
            return SendDataCommandExpectMultiBulkReply(null, options.ToCommand() + "\r\n");
        }

        public byte[] GetSet(string key, byte[] value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length > 1073741824)
                throw new ArgumentException("value exceeds 1G", "value");

            if (!SendDataCommand(value, "GETSET {0} {1}\r\n", key, value.Length))
                throw new Exception("Unable to connect");

            return ReadData();
        }

        public string GetSet(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (value == null)
                throw new ArgumentNullException("value");
            return Encoding.UTF8.GetString(GetSet(key, Encoding.UTF8.GetBytes(value)));
        }

        string ReadLine()
        {
            StringBuilder sb = new StringBuilder();
            int c;

            while ((c = _bstream.ReadByte()) != -1)
            {
                if (c == '\r')
                    continue;
                if (c == '\n')
                    break;
                sb.Append((char)c);
            }
            return sb.ToString();
        }

       

        byte[] end_data = new byte[] { (byte)'\r', (byte)'\n' };

        bool SendDataCommand(byte[] data, string cmd, params object[] args)
        {
            
            if (_socket == null)
                return false;

            string s = args.Length > 0 ? String.Format(cmd, args) : cmd;
            byte[] r = Encoding.UTF8.GetBytes(s);
            try
            {

                _socket.Send(r);
                if (data != null)
                {
                    _socket.Send(data);
                    _socket.Send(end_data);
                }
            }
            catch (SocketException)
            {
                // timeout;
                TrueClose();
                

                return false;
            }
            return true;
        }

        bool SendCommand(string cmd, params object[] args)
        {
            
            if (_socket == null)
                return false;

            string s = args != null && args.Length > 0 ? String.Format(cmd, args) : cmd;
            byte[] r = Encoding.UTF8.GetBytes(s);
            try
            {

                _socket.Send(r);
            }
            catch (SocketException)
            {
                // timeout;
                TrueClose();
                

                return false;
            }
            return true;
        }



        void ExpectSuccess()
        {
            int c = _bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();

            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR") ? s.Substring(4) : s);
        }

        void SendExpectSuccess(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            ExpectSuccess();
        }

        int SendDataExpectInt(byte[] data, string cmd, params object[] args)
        {
            if (!SendDataCommand(data, cmd, args))
                throw new Exception("Unable to connect");

            int c = _bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();

            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR") ? s.Substring(4) : s);
            if (c == ':')
            {
                int i;
                if (int.TryParse(s, out i))
                    return i;
            }
            throw new ResponseException("Unknown reply on integer request: " + c + s);
        }

        int SendExpectInt(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            int c = _bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();

            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR") ? s.Substring(4) : s);
            if (c == ':')
            {
                int i;
                if (int.TryParse(s, out i))
                    return i;
            }
            throw new ResponseException("Unknown reply on integer request: " + c + s);
        }

        string SendExpectString(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            int c = _bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();

            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR") ? s.Substring(4) : s);
            if (c == '+')
                return s;

            throw new ResponseException("Unknown reply on integer request: " + c + s);
        }

        //
        // This one does not throw errors
        //
        string SendGetString(string cmd, params object[] args)
        {
            if (!SendCommand(cmd, args))
                throw new Exception("Unable to connect");

            return ReadLine();
        }

        byte[] SendExpectData(byte[] data, string cmd, params object[] args)
        {
            if (!SendDataCommand(data, cmd, args))
                throw new Exception("Unable to connect");

            return ReadData();
        }

        byte[] ReadData()
        {
            string r = ReadLine();

            if (r.Length == 0)
                return null;

            char c = r[0];
            if (c == '-')
                throw new ResponseException(r.StartsWith("-ERR") ? r.Substring(5) : r.Substring(1));

            if (c == '$')
            {
                if (r == "$-1")
                    return null;
                int n;

                if (Int32.TryParse(r.Substring(1), out n))
                {
                    byte[] retbuf = new byte[n];

                    int bytesRead = 0;
                    do
                    {
                        int read = _bstream.Read(retbuf, bytesRead, n - bytesRead);
                        if (read < 1)
                            throw new ResponseException("Invalid termination mid stream");
                        bytesRead += read;
                    }
                    while (bytesRead < n);
                    if (_bstream.ReadByte() != '\r' || _bstream.ReadByte() != '\n')
                        throw new ResponseException("Invalid termination");
                    return retbuf;
                }
                throw new ResponseException("Invalid length");
            }

            //returns the number of matches
            if (c == '*')
            {
                int n;
                if (Int32.TryParse(r.Substring(1), out n))
                    return n <= 0 ? new byte[0] : ReadData();

                throw new ResponseException("Unexpected length parameter" + r);
            }

            throw new ResponseException("Unexpected reply: " + r);
        }

        public bool ContainsKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("EXISTS " + key + "\r\n") == 1;
        }

        public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("DEL " + key + "\r\n", key) == 1;
        }

        public int Remove(params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");
            return SendExpectInt("DEL " + string.Join(" ", args) + "\r\n");
        }

        public int Increment(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("INCR " + key + "\r\n");
        }

        public int Increment(string key, int count)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("INCRBY {0} {1}\r\n", key, count);
        }

        public int Decrement(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("DECR " + key + "\r\n");
        }

        public int Decrement(string key, int count)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("DECRBY {0} {1}\r\n", key, count);
        }

        public KeyType TypeOf(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            switch (SendExpectString("TYPE {0}\r\n", key))
            {
                case "none":
                    return KeyType.None;
                case "string":
                    return KeyType.String;
                case "set":
                    return KeyType.Set;
                case "list":
                    return KeyType.List;
            }
            throw new ResponseException("Invalid value");
        }

        public string RandomKey()
        {
            return SendExpectString("RANDOMKEY\r\n");
        }

        public bool Rename(string oldKeyname, string newKeyname)
        {
            if (oldKeyname == null)
                throw new ArgumentNullException("oldKeyname");
            if (newKeyname == null)
                throw new ArgumentNullException("newKeyname");
            return SendGetString("RENAME {0} {1}\r\n", oldKeyname, newKeyname)[0] == '+';
        }

        public bool Expire(string key, int seconds)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("EXPIRE {0} {1}\r\n", key, seconds) == 1;
        }

        public bool ExpireAt(string key, int time)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("EXPIREAT {0} {1}\r\n", key, time) == 1;
        }

        public int TimeToLive(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            return SendExpectInt("TTL {0}\r\n", key);
        }

        public int DbSize
        {
            get
            {
                return SendExpectInt("DBSIZE\r\n");
            }
        }

        public string Save()
        {
            return SendGetString("SAVE\r\n");
        }

        public void BackgroundSave()
        {
            SendGetString("BGSAVE\r\n");
        }

        public void Shutdown()
        {
            SendGetString("SHUTDOWN\r\n");
        }

        public void FlushAll()
        {
            SendGetString("FLUSHALL\r\n");
        }

        public void FlushDb()
        {
            SendGetString("FLUSHDB\r\n");
        }

        const long UnixEpoch = 621355968000000000L;

        public DateTime LastSave
        {
            get
            {
                int t = SendExpectInt("LASTSAVE\r\n");

                return new DateTime(UnixEpoch) + TimeSpan.FromSeconds(t);
            }
        }

        public Dictionary<string, string> GetInfo()
        {
            byte[] r = SendExpectData(null, "INFO\r\n");
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (string line in Encoding.UTF8.GetString(r).Split('\n'))
            {
                int p = line.IndexOf(':');
                if (p == -1)
                    continue;
                dict.Add(line.Substring(0, p), line.Substring(p + 1));
            }
            return dict;
        }

        public string[] Keys
        {
            get
            {
                string commandResponse = Encoding.UTF8.GetString(SendExpectData(null, "KEYS *\r\n"));
                if (commandResponse.Length < 1)
                    return new string[0];
                else
                    return commandResponse.Split(' ');
            }
        }

        public string[] GetKeys(string pattern)
        {
            if (pattern == null)
                throw new ArgumentNullException("key");
            byte[] keys = SendExpectData(null, "KEYS {0}\r\n", pattern);
            if (keys.Length == 0)
                return new string[0];
            return Encoding.UTF8.GetString(keys).Split(' ');
        }

        public byte[][] GetKeys(params string[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException("key1");
            if (keys.Length == 0)
                throw new ArgumentException("keys");

            return SendDataCommandExpectMultiBulkReply(null, "MGET {0}\r\n", string.Join(" ", keys));
        }


        public byte[][] SendDataCommandExpectMultiBulkReply(byte[] data, string command, params object[] args)
        {
            if (!SendDataCommand(data, command, args))
                throw new Exception("Unable to connect");
            int c = _bstream.ReadByte();
            if (c == -1)
                throw new ResponseException("No more data");

            string s = ReadLine();

            if (c == '-')
                throw new ResponseException(s.StartsWith("ERR") ? s.Substring(4) : s);
            if (c == '*')
            {
                int count;
                if (int.TryParse(s, out count))
                {
                    byte[][] result = new byte[count][];

                    for (int i = 0; i < count; i++)
                        result[i] = ReadData();

                    return result;
                }
            }
            throw new ResponseException("Unknown reply on multi-request: " + c + s);
        }

        #region List commands
        public byte[][] ListRange(string key, int start, int end)
        {
            return SendDataCommandExpectMultiBulkReply(null, "LRANGE {0} {1} {2}\r\n", key, start, end);
        }

        public void LeftPush(string key, string value)
        {
            SendExpectSuccess("LPUSH {0} {1}\r\n{2}\r\n", key, value.Length, value);
        }

        public void RightPush(string key, string value)
        {
            SendExpectSuccess("RPUSH {0} {1}\r\n{2}\r\n", key, value.Length, value);
        }

        public int ListLength(string key)
        {
            return SendExpectInt("LLEN {0}\r\n", key);
        }

        public byte[] ListIndex(string key, int index)
        {
            SendCommand("LINDEX {0} {1}\r\n", key, index);
            return ReadData();
        }

        public byte[] LeftPop(string key)
        {
            SendCommand("LPOP {0}\r\n", key);
            return ReadData();
        }
        #endregion

        #region Set commands
        public bool AddToSet(string key, byte[] member)
        {
            return SendDataExpectInt(member, "SADD {0} {1}\r\n", key, member.Length) > 0;
        }

        public bool AddToSet(string key, string member)
        {
            return AddToSet(key, Encoding.UTF8.GetBytes(member));
        }

        public int CardinalityOfSet(string key)
        {
            return SendDataExpectInt(null, "SCARD {0}\r\n", key);
        }

        public bool IsMemberOfSet(string key, byte[] member)
        {
            return SendDataExpectInt(member, "SISMEMBER {0} {1}\r\n", key, member.Length) > 0;
        }

        public bool IsMemberOfSet(string key, string member)
        {
            return IsMemberOfSet(key, Encoding.UTF8.GetBytes(member));
        }

        public byte[][] GetMembersOfSet(string key)
        {
            return SendDataCommandExpectMultiBulkReply(null, "SMEMBERS {0}\r\n", key);
        }

        public byte[] GetRandomMemberOfSet(string key)
        {
            return SendExpectData(null, "SRANDMEMBER {0}\r\n", key);
        }

        public byte[] PopRandomMemberOfSet(string key)
        {
            return SendExpectData(null, "SPOP {0}\r\n", key);
        }

        public bool RemoveFromSet(string key, byte[] member)
        {
            return SendDataExpectInt(member, "SREM {0} {1}\r\n", key, member.Length) > 0;
        }

        public bool RemoveFromSet(string key, string member)
        {
            return RemoveFromSet(key, Encoding.UTF8.GetBytes(member));
        }

        public byte[][] GetUnionOfSets(params string[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException();

            return SendDataCommandExpectMultiBulkReply(null, "SUNION " + string.Join(" ", keys) + "\r\n");

        }

        void StoreSetCommands(string cmd, string destKey, params string[] keys)
        {
            if (String.IsNullOrEmpty(cmd))
                throw new ArgumentNullException("cmd");

            if (String.IsNullOrEmpty(destKey))
                throw new ArgumentNullException("destKey");

            if (keys == null)
                throw new ArgumentNullException("keys");

            SendExpectSuccess("{0} {1} {2}\r\n", cmd, destKey, String.Join(" ", keys));
        }

        public void StoreUnionOfSets(string destKey, params string[] keys)
        {
            StoreSetCommands("SUNIONSTORE", destKey, keys);
        }

        public byte[][] GetIntersectionOfSets(params string[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException();

            return SendDataCommandExpectMultiBulkReply(null, "SINTER " + string.Join(" ", keys) + "\r\n");
        }

        public void StoreIntersectionOfSets(string destKey, params string[] keys)
        {
            StoreSetCommands("SINTERSTORE", destKey, keys);
        }

        public byte[][] GetDifferenceOfSets(params string[] keys)
        {
            if (keys == null)
                throw new ArgumentNullException();

            return SendDataCommandExpectMultiBulkReply(null, "SDIFF " + string.Join(" ", keys) + "\r\n");
        }

        public void StoreDifferenceOfSets(string destKey, params string[] keys)
        {
            StoreSetCommands("SDIFFSTORE", destKey, keys);
        }

        public bool MoveMemberToSet(string srcKey, string destKey, byte[] member)
        {
            return SendDataExpectInt(member, "SMOVE {0} {1} {2}\r\n", srcKey, destKey, member.Length) > 0;
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Redis()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
               
                Close();
                
            }
        }
    }

    public class SortOptions
    {
        private string _key;
        public string Key { get { return _key; } set { _key = value; } }
        private bool _descending;
        public bool Descending { get { return _descending; } set { _descending = value; } }
        private bool _lexographically;
        public bool Lexographically { get { return _lexographically; } set { _lexographically = value; } }
        private Int32 _lowerLimit;
        public Int32 LowerLimit { get { return _lowerLimit; } set { _lowerLimit = value; } }
        private Int32 _upperLimit;
        public Int32 UpperLimit { get { return _upperLimit; } set { _upperLimit = value; } }
        private string _by;
        public string By { get { return _by; } set { _by = value; } }
        private string _storeInKey;
        public string StoreInKey { get { return _storeInKey; } set { _storeInKey = value; } }
        private string _get;
        public string Get { get { return _get; } set { _get = value; } }

        public string ToCommand()
        {
            StringBuilder command = new StringBuilder();
            //string command = "SORT " + this.Key;
            command.Append("SORT ");
            command.Append(this.Key);
            if (LowerLimit != 0 || UpperLimit != 0)
            {
                command.Append(" LIMIT ");
                command.Append(LowerLimit);
                command.Append(" ");
                command.Append(UpperLimit);
            }
            //command += " LIMIT " + LowerLimit + " " + UpperLimit;
            if (Lexographically)
            //command += " ALPHA";
            {
                command.Append(" ALPHA");
            }
            if (!string.IsNullOrEmpty(By))
            //command += " BY " + By;
            {
                command.Append(" BY ");
                command.Append(By);
            }
            if (!string.IsNullOrEmpty(Get))
            //command += " GET " + Get;
            {
                command.Append(" GET ");
                command.Append(Get);
            }
            if (!string.IsNullOrEmpty(StoreInKey))
            //command += " STORE " + StoreInKey;
            {
                command.Append(" STORE ");
                command.Append(StoreInKey);
            }
            return command.ToString();
        }
    }

}