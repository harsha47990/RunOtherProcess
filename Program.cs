using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunOtherProcess
{
    class Program
    {

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        static void Main(string[] args)
        {

            string ProcessName = ConfigurationManager.AppSettings["ProcessName"].ToString();
            string WindowName = ConfigurationManager.AppSettings["WindowName"].ToString();
            string Steps = ConfigurationManager.AppSettings["Steps"].ToString();

            Process p = Process.Start(ProcessName);
            Thread.Sleep(5000);
            p.WaitForInputIdle();

            try
            {
                IntPtr handle = FindWindow(null, WindowName);
                if (handle == IntPtr.Zero)
                {
                    return;
                }
                SetForegroundWindow(handle);
          
                foreach (string s in Steps.Split(','))
                {
                    SendKeys.SendWait(s);
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } 


        }
    }
}
