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
        [DllImport("user32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        static void Main(string[] args)
        {
            string ProcessName = ConfigurationManager.AppSettings["ProcessName"].ToString();
            string Steps = ConfigurationManager.AppSettings["Steps"].ToString();

            Process p = Process.Start(ProcessName);
            p.WaitForInputIdle();

            //try keeping it on top window
            try
            {
                IntPtr h = p.MainWindowHandle;
                SetForegroundWindow(h);
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
            //Send keys
            foreach (string s in Steps.Split(','))
            {
                SendKeys.SendWait(s);
                Thread.Sleep(1000);
            }

            //wait untill process completes and closes the application
            while (true)
            {
                if (p.WaitForInputIdle())
                {
                    SendKeys.SendWait("%{F4}");
                    p.WaitForExit(5000);
                    return;
                }
            }



        }
    }
}
