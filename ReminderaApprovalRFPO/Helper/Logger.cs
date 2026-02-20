using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReminderaApprovalRFPO.Helper
{
    internal class Logger
    {

        private static readonly string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error_log.txt");

        public static void Log(string message)
        {
            File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}");
        }

        public static void Log(Exception ex)
        {
            Log(ex.ToString());
        }




    }
}
