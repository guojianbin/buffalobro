using System;
using System.Runtime.InteropServices;
namespace Buffalo.Kernel
{
    /// <summary>
    /// DllInvoke 的摘要说明
    /// </summary>
    public class DllInvoke:IDisposable
    {

        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);
        [DllImport("kernel32.dll")]
        private extern static IntPtr GetProcAddress(IntPtr lib, String funcName);
        [DllImport("kernel32.dll")]
        private extern static bool FreeLibrary(IntPtr lib);

        private IntPtr hLib;
        public DllInvoke(String DLLPath)
        {
            hLib = LoadLibrary(DLLPath);
            if (hLib == IntPtr.Zero) 
            {
                throw new Exception("加载" + DLLPath + "失败\n错误码：" + Marshal.GetLastWin32Error());
            }

        }
        ~DllInvoke()
        {
            Dispose();
        }
        //将要执行的函数转换为委托
        public Delegate Invoke(String APIName, Type t)
        {
            IntPtr api = GetProcAddress(hLib, APIName);
            return (Delegate)Marshal.GetDelegateForFunctionPointer(api, t);
        }

        #region IDisposable 成员

        public void Dispose()
        {
            FreeLibrary(hLib);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}