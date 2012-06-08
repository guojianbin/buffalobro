using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Buffalo.Kernel.HttpServerModel
{
    /// <summary>
    /// 请求处理委托
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    public delegate void RequestProcessingHandle(RequestInfo request,ResponseInfo response);

    /// <summary>
    /// 模拟Web服务器模块
    /// </summary>
    public class ServerModel
    {
        private TcpListener _listener;
        /// <summary>
        /// 请求处理呃事件
        /// </summary>
        public event RequestProcessingHandle OnRequestProcessing;
        Thread _lisThread = null;
        bool isrunning=false;
        int _port = 0;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port
        {
            get { return _port; }
        }
        IPAddress _ip;

        /// <summary>
        /// IP
        /// </summary>
        public IPAddress IP
        {
            get { return _ip; }
        }
        static readonly Encoding DefaultEncoding = System.Text.Encoding.UTF8;

        /// <summary>
        /// 初始化服务模块
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="lisPort">端口</param>
        /// <param name="encoding">编码</param>
        public ServerModel(string ip,int lisPort) 
        {
            if (string.IsNullOrEmpty(ip))
            {
                _ip = Dns.GetHostByName(Dns.GetHostName()).AddressList[0];
            }
            else 
            {
                _ip = IPAddress.Parse(ip);
            }
            _port = lisPort;
            
        }


        /// <summary>
        /// 开始监听
        /// </summary>
        public void StarServer() 
        {
            _listener = new TcpListener(_ip, _port);
            isrunning = true;
            _lisThread = new Thread(new ThreadStart(ModelListen));
            _listener.Start();
            _lisThread.Start();
        }

        /// <summary>
        /// 监听
        /// </summary>
        private void ModelListen() 
        {
            while (isrunning)
            {
                //接受新连接
                using (Socket requestSocket = _listener.AcceptSocket())
                {
                    try
                    {
                        byte[] receiveContent = null;//接收的内容
                        if (requestSocket.Connected)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                Byte[] bufferContent = new Byte[1024];
                                int receiveBytes = 0;
                                do
                                {
                                    receiveBytes = requestSocket.Receive(bufferContent, bufferContent.Length, 0);
                                    ms.Write(bufferContent, 0, receiveBytes);
                                } while (receiveBytes >= bufferContent.Length);
                                if (ms.Length <= 0)
                                {
                                    continue;
                                }
                                receiveContent = ms.ToArray();
                                

                            }
                        }
                        else
                        {
                            continue;
                        }

                        string content = DefaultEncoding.GetString(receiveContent);
                        RequestInfo info = new RequestInfo(content);
                        if (OnRequestProcessing != null)
                        {
                            Encoding encoding = GetEncoding(info);
                            using (ResponseInfo resInfo = new ResponseInfo(encoding))
                            {
                                OnRequestProcessing(info, resInfo);
                                byte[] rcontent = resInfo.ResponseContent;
                                string header = CreateHeader(info.HttpVersion, resInfo.MimeType, rcontent.Length, info.Connection,encoding);
                                byte[] headContent = encoding.GetBytes(header);

                                requestSocket.Send(headContent);
                                requestSocket.Send(rcontent);
                            }
                        }
                    }
                    catch(Exception ex) 
                    {
#if DEBUG
                        Debug.WriteLine(ex.Message);
#endif
                    }
                }
            }
        }

        /// <summary>
        /// 获取请求的编码
        /// </summary>
        /// <param name="resInfo"></param>
        /// <returns></returns>
        private Encoding GetEncoding(RequestInfo resInfo) 
        {
            if (string.IsNullOrEmpty(resInfo.AcceptEncoding)) 
            {
                return DefaultEncoding;
            }
            try
            {
                Encoding encoding = Encoding.GetEncoding(resInfo.AcceptEncoding);
                if (encoding != null)
                {
                    return encoding;
                }
            }
            catch { }
            return DefaultEncoding;
        }

        /// <summary>
        /// 发送头
        /// </summary>
        /// <param name="sHttpVersion"></param>
        /// <param name="sMIMEHeader"></param>
        /// <param name="iTotBytes"></param>
        /// <param name="sStatusCode"></param>
        private string CreateHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, Encoding encoding)
        {

            StringBuilder sBuffer = new StringBuilder();

            if (string.IsNullOrEmpty(sMIMEHeader))
            {
                sMIMEHeader = "text/html"; // 默认 text/html
            }

            sBuffer.Append(sHttpVersion + sStatusCode + "\r\n");
            sBuffer.Append("Content-Type: " + sMIMEHeader + ";charset=" + encoding.WebName + "\r\n");
            sBuffer.Append("Accept-Ranges: bytes\r\n");
            sBuffer.Append("Content-Encoding: " + encoding.WebName + "\r\n");
            sBuffer.Append("Content-Length: " + iTotBytes + "\r\n\r\n");
            
            return sBuffer.ToString();
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void StopServer() 
        {
            if (_listener != null) 
            {
                _listener.Stop();
            }
           
            isrunning = false;
            if (_lisThread != null)
            {
                _lisThread.Abort();

            }
        }
    }
}
