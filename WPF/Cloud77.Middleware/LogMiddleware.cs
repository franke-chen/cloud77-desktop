using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cloud77.Middleware
{
    public class LogMiddleware
    {
        private LocalFileMiddleware localFile;

        public LogMiddleware()
        {
            localFile = new LocalFileMiddleware();

            dir = Path.Combine(DesktopMiddleware.LocalApplicationData, DesktopMiddleware.AppFramework.ToLower());
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            path = Path.Combine(dir, "logs.txt");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "");
            }
        }

        private string dir;
        private string path;

        public void AddLog(string log)
        {
            Task.Factory.StartNew(() =>
            {
                File.AppendAllLines(path, new string[] { $"[{DateTime.Now}]: {log}" });
            });
        }
    }
}
