using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Cloud77.Middleware;

namespace Cloud77.WPF.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        public MainViewModel()
        {            
            log = new LogMiddleware();
            rest = new RESTAPIMiddleware();
            log.AddLog("main window view model is created.");

            _status = "Network"; // NetworkOff
            _statusText = "Online"; // Online Offline

            System.Threading.Tasks.Task.Run(() =>
            {
                APIKey = rest.GetAPIKey("https://www.cloud77.top");
            });
        }

        private RESTAPIMiddleware rest;
        private LogMiddleware log;

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

        string _email = "demo@demo.com";
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        string _statusText;
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                RaisePropertyChanged("StatusText");
            }
        }

        private bool _menuChecked = true;
        public bool MenuChecked
        {
            get => _menuChecked;
            set
            {
                _menuChecked = value;
                RaisePropertyChanged("MenuChecked");

                MenuKind = value ? "HamburgerMenuBack" : "HamburgerMenu";
            }
        }

        private string _menuKind = "HamburgerMenuBack";
        public string MenuKind
        {
            get => _menuKind;
            set
            {
                _menuKind = value;
                RaisePropertyChanged("MenuKind");
            }
        }

        public string AboutMeTitle = "About Cloud77 Desktop";

        public string AboutMeContent = "This computer program is protected by copyright law. Unauthorized production or distribution of any portion of this program will be prosecuted to the maximum extent possible under law.";
        public int Days { get; set; } = 0;
        public string LicenseMessage
        {
            // "Your license is valid for the coming " + Days.ToString() + " days";
            get => "You get the demo license.";
        }

        string _apikey = "N/A";

        public string APIKey
        {
            get => _apikey;
            set
            {
                _apikey = value;
                RaisePropertyChanged("APIKey");
            }
        }
    }
}
