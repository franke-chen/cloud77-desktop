using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Cloud77.Middleware
{
    public class DesktopMiddleware
    {
        static DesktopMiddleware()
        {

        }

        public static void Init()
        {
            TesterDbContext dbcontext = new TesterDbContext(Path.Combine(startUpPath, "database", "tester.db"));

            if (!dbcontext.Testers.Any())
            {
                dbcontext.Testers.Add(new Tester()
                {
                    Guid = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                });

                dbcontext.SaveChanges();
            }
        }

        public static string AppFramework = "WPF";

        public static string AppVersion = "";

        private static string startUpPath = "";
        private static string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static string localApplicationData;
        public static string StartUpPath
        {
            get => startUpPath;
            set
            {
                startUpPath = value;
                if (!string.IsNullOrEmpty(StartUpPath))
                {
                    if (StartUpPath.StartsWith(Path.Combine(appData, "Cloud77_")))
                    {
                        if (StartUpPath.StartsWith(Path.Combine(appData, "Cloud77_Local")))
                        {
                            mode = "Local";
                        }
                        if (StartUpPath.StartsWith(Path.Combine(appData, "Cloud77_Alpha")))
                        {
                            mode = "Alpha";
                        }
                        if (StartUpPath.StartsWith(Path.Combine(appData, "Cloud77_Beta")))
                        {
                            mode = "Beta";
                        }
                    }
                    else
                    {
                        if (StartUpPath.StartsWith(Path.Combine(appData, "Cloud77")))
                        {
                            mode = "Prod";
                        }
                    }
                }
                localApplicationData = Path.Combine(appData, "Cloud77_" + mode);
                localApplicationData = localApplicationData.Replace("_Prod", "");
            }
        }
        public static string LocalApplicationData => localApplicationData;

        private static string mode = "Debug";
        public static string Mode => mode;
    }
}
