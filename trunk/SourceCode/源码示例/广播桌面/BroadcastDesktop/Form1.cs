using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.HttpServerModel;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel;
using System.IO;
using System.Threading;
using BroadcastDesktop.Properties;
using System.Net;

namespace BroadcastDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        ServerModel sm ;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            Listening(false);
            BindIP();
        }

        private void BindIP() 
        {
            IPAddress[] ips= Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ips) 
            {
                cmbIP.Items.Add(ip.ToString());
            }
            if (cmbIP.Items.Count > 0) 
            {
                cmbIP.SelectedIndex = 0;
            }
        }



        void sm_OnRequestProcessing(RequestInfo request, ResponseInfo response)
        {
            string url = request.Path;
            if (url.Equals("/desktop", StringComparison.CurrentCultureIgnoreCase))
            {
                
                //response.MimeType = "image/jpeg";
                response.Write(Resources.model.Replace("<%=url%>", sm.IP.ToString() + ":" + sm.Port));
                return;
            }
            if (url.Equals("/getdesktop", StringComparison.CurrentCultureIgnoreCase))
            {
                response.MimeType = "image/jpeg";
                if (DesktopCache.CurrentDesktop != null)
                {
                    response.Write(DesktopCache.CurrentDesktop);
                }
                return;
            }
            response.Write("没有数据");
        }

        private void Listening(bool isListen)
        {
            btnListen.Enabled = !isListen;
            btnStop.Enabled = isListen;
            cmbIP.Enabled = !isListen;
            nupPort.Enabled = !isListen;
        }

        

        

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (sm != null)
            {
                try
                {
                    sm.StopServer();

                    DesktopCache.StopUpdate();
                    Listening(false);
                }
                catch { }
                Thread.Sleep(1000);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnStop_Click(btnStop, new EventArgs());
            
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            sm = new ServerModel(cmbIP.Text, (int)nupPort.Value);
            txturl.Text = "http://" + cmbIP.Text + ":" + (int)nupPort.Value + "/desktop";
            sm.OnRequestProcessing += new RequestProcessingHandle(sm_OnRequestProcessing);
            sm.StarServer();
            DesktopCache.StarUpdate();
            Listening(true);
        }
    }
}