using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Cloud77.Middleware;

namespace Cloud77.ViewModel
{
    public class MainWindowViewModel: BaseViewModel
    {
        private static MainWindowViewModel viewmodel;

        public static void Init(string version, string startUp)
        {
            viewmodel = new MainWindowViewModel(version, startUp);
        }

        public static MainWindowViewModel Instance => viewmodel;
        
        private MainWindowViewModel(string version, string startUp)
        {
            Version = version;
            DesktopMiddleware.StartUpPath = startUp;
            DesktopMiddleware.AppVersion = version;
            
            Items = new ObservableCollection<Item>();

            Items.Add(new Item() { Name = "item1" });
            Items.Add(new Item() { Name = "item2" });
            Items.Add(new Item() { Name = "item3" });
            Items.Add(new Item() { Name = "item4" });
            Items.Add(new Item() { Name = "item5" });

            log = new LogMiddleware();

            log.AddLog("main window view model is created.");
        }

        private LogMiddleware log;

        public string StartUpPath
        {
            get => DesktopMiddleware.StartUpPath;
            set
            {
                DesktopMiddleware.StartUpPath = value;
            }
        }

        public class Item
        {
            public string Name { get; set; }
        }

        public string Version { get; private set; }

        public ObservableCollection<Item> Items { get; set; }

        public string BaseTheme { get; set; }

        public string ColorTheme { get; set; }

        public string[] ColorThemes = new string[]
        {
             "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
        };

        public string[] BaseThemes = new string[]
        {
            "Light", "Dark"
        };

        private string _userName = "demo";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged("UserName");
            }
        }
    }
}
