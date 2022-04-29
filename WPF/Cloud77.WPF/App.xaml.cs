using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Cloud77.Middleware;

namespace Cloud77.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var assembly = Assembly.GetExecutingAssembly();
            DesktopMiddleware.StartUpPath = System.IO.Directory.GetCurrentDirectory();
            DesktopMiddleware.AppVersion = assembly.GetName().Version.ToString(3);
            DesktopMiddleware.Init();
        }
    }
}
