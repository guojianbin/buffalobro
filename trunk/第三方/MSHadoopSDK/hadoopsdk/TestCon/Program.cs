using Microsoft.Hadoop.WebHDFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestCon
{
    class Program
    {
        static List<DirectoryEntry> lstDir = new List<DirectoryEntry>();
        static void Main(string[] args)
        {

            Uri myUri = new Uri("http://192.168.1.50:50070");
            string userName = "hadoop";
            WebHDFSClient myClient = new WebHDFSClient(myUri, userName);
            Task<DirectoryListing> tsk=myClient.GetDirectoryStatus("/");
            tsk.ContinueWith(LoadDir);
            while (!tsk.IsCompleted) 
            {
                Thread.Sleep(300);
            }
            foreach (DirectoryEntry d in lstDir) 
            {
                Console.WriteLine(d.PathSuffix);
            }
            Console.Read();
        }

        static void LoadDir(Task<DirectoryListing> dirs) 
        {
            lstDir.Clear();
            foreach(DirectoryEntry d in dirs.Result.Directories)
            {
                lstDir.Add(d);
            }
        }
    }
}
