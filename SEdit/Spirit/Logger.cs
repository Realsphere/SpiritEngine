using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Realsphere.Spirit
{
    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }

    public static class Logger
    {
        static StreamWriter writr;
        internal static string path1;
        internal static void init(string path)
        {
            path1 = path;
            writr = new StreamWriter(path);
        }

        internal static void Close()
        {
            writr.Close();
            writr.Dispose();
        }

        internal static void PrintSysInfo()
        {
            // Get System Info
            string sysInfo = "";

            string f = Path.GetTempFileName();
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = $"/C dxdiag /t \"{f}\""
            };
            p.Start();
            p.WaitForExit();
            sysInfo = File.ReadAllText(f);

            File.WriteAllText(Path.GetDirectoryName(path1) + "\\sysinfo.txt", sysInfo);
            writr.WriteLine("---SYSTEM INFORMATION---");
            writr.WriteLine("Screen Count: " + Screen.AllScreens.Length);
            foreach (Screen scr in Screen.AllScreens)
            {
                writr.WriteLine("Screen " + Screen.AllScreens.ToList().IndexOf(scr) + ": " + scr.Bounds.ToString());
            }
        }

        public static void Log(string msg, LogLevel ll)
        {
            string zeroifyNum(int num)
            {
                if (num < 10)
                    return "0" + num;
                else
                    return "" + num;
            }

            DateTime dt = DateTime.Now;
            string write = $"[{zeroifyNum(dt.Day)}/{zeroifyNum(dt.Month)}/{zeroifyNum(dt.Year)} {zeroifyNum(dt.Hour)}:{zeroifyNum(dt.Minute)}:{zeroifyNum(dt.Second)} ";

            switch(ll)
            {
                case LogLevel.Information: write += "INFO"; break;
                case LogLevel.Warning: write += "WARN"; break;
                case LogLevel.Error: write += "ERROR"; break;
                default: throw new ArgumentException("Log(msg, ll) ll requires to be LogLevel.Information, LogLevel.Warning or LogLevel.Error.");
            }

            write += "] ";
            write += msg;
            writr.WriteLine(write);
#if DEBUG
            Debug.WriteLine(write);
#endif
            System.Console.WriteLine(write);
        }
    }
}
