using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using System.Management;

namespace Buffalo.Kernel.NetClients
{
    public delegate void ClientReceive(NetConnection conn, byte[] data);
    public class NetListener
    {
        public event ClientReceive OnClientReceive;
        IPEndPoint endPoint;

        
        Thread lisThd;
        Socket listener;
        ISynchronizeInvoke _synInvoker;
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="port">�����˿�</param>
        /// <param name="synInvoker">��Ҫ�߳�ͬ��������(�磺��ǰ����)</param>
        public NetListener(int port, ISynchronizeInvoke synInvoker)
            : this(null, port, synInvoker)
        {
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="port">�����˿�</param>
        public NetListener(int port)
            : this(null, port, null)
        {
        }
        /// <summary>
        ///  ���������
        /// </summary>
        /// <param name="port">�����˿�</param>
        public NetListener()
            : this(null, 0,null)
        {
        }
        public IPEndPoint EndPoint
        {
            get { return endPoint; }
            
        }
        
        /// <summary>
        ///  ���������
        /// </summary>
        /// <param name="ipAddress">IP��ַ</param>
        /// <param name="port">�����˿�</param>
        /// <param name="synInvoker">��Ҫ�߳�ͬ��������(�磺��ǰ����)</param>
        public NetListener(string ipAddress, int port, ISynchronizeInvoke synInvoker)
        {
            IPAddress ipAddr = null;
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddr = IPAddress.Parse(GetActiveIPAddress());
            }
            else
            {
                ipAddr = IPAddress.Parse(ipAddress);
            }
            endPoint = new IPEndPoint(ipAddr, port);
            _synInvoker = synInvoker;
        }

        /// <summary>
        /// ��ȡ�IP
        /// </summary>
        /// <returns></returns>
        public static string GetActiveIPAddress()
        {
            
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string str = " ";
            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"] == true)
                {
                    return (mo["IPAddress"] as string[])[0];
                }

            }
            return "";

        }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        public void StarListen() 
        {
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(endPoint);
            endPoint = listener.LocalEndPoint as IPEndPoint;

            lisThd = new Thread(new ThreadStart(MainListener));
            //lisThd.ApartmentState = ApartmentState.STA;
            lisThd.Start();
        }
        
        /// <summary>
        /// �رռ���
        /// </summary>
        public void StopListener()
        {
            if (lisThd != null)
            {
                lisThd.Abort();
                lisThd = null;
            }
            if (listener != null)
            {
                listener.Close();
                listener = null;
                endPoint = null;
            }
        }
        /// <summary>
        /// �������߳�
        /// </summary>
        private void MainListener() 
        {
            listener.Listen(0);
            Socket client = null;

            while (true)
            {
                client = listener.Accept();

                ParameterizedThreadStart start = new ParameterizedThreadStart(DoAccept);
                Thread thdAccept = new Thread(start);
                thdAccept.Start(client);

            }
        }

        

        /// <summary>
        /// ������Ϣ����ʱ��
        /// </summary>
        private void DoAccept(object objClient) 
        {
            Socket client = objClient as Socket;

            if (client == null)
            {
                return;
            }
            
            NetConnection conn = new NetConnection(client);
            byte[] tmpByte = conn.Receive();
            if (OnClientReceive != null)
            {
                if (_synInvoker != null)
                {
                    _synInvoker.Invoke(OnClientReceive, new object[] { conn, tmpByte });
                }
                else
                {
                    OnClientReceive(conn, tmpByte);
                }
            }
            conn.CloseConnection();
        }
        ~NetListener() 
        {
            StopListener();
        }

        
    }
}

//**************ʾ��*************

//***********��Ҫ�ؼ�ͬ��(�ڴ�����)**********
//NetListener lis = new NetListener(9090, this);
//lis.StarListen();

//**********����Ҫͬ��*************
//NetListener lis = new NetListener(9090);