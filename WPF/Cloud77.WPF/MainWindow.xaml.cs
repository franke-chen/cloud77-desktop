using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cloud77.WPF.ViewModel;
using Cloud77.Middleware;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Squirrel;
using ControlzEx.Theming;

namespace Cloud77.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string version;
        private string title;
        private string description;
        private string product;
        
        private MainViewModel context;

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var attr = assembly.GetCustomAttributes<AssemblyTitleAttribute>().FirstOrDefault();
            //this.Title = $"{attr.Title} {version}";

            context = new MainViewModel();

            this.title = $"{DesktopMiddleware.AppVersion}";

            this.DataContext = context;

            Task.Run(() =>
            {
                var t = Task.Delay(2000);
                t.Wait();

                this.message.Invoke(() =>
                {
                    this.message.Content = "Welcome to app";
                });

                this.Dispatcher.Invoke(() =>
                {
                    MyNotifyIcon.ShowBalloonTip("Welcome to app", "Just a notification", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
                    MyNotifyIcon.HideBalloonTip();
                });
            });

            tabNames.Add("home");
            tabNames.Add("test");

            this.home.RaiseEvent(new RoutedEventArgs(ListBoxItem.SelectedEvent));
        }

        private async void AutoUpdate(string path)
        {
            try
            {
                using (var mgr = new UpdateManager(path))
                {
                    UpdateInfo updateInfo = await mgr.CheckForUpdate(false, (progress) =>
                    {
                        this.message.Invoke(() =>
                        {
                            this.message.Content = "Checking updates " + progress.ToString() + "%";
                        });
                    });

                    if (updateInfo.ReleasesToApply == null || updateInfo.ReleasesToApply.Count == 0)
                    {
                        this.message.Invoke(new Action(() =>
                        {
                            this.message.Content = "No new update. You have latest version now.";
                        }));
                        return;
                    }

                    this.progress.Invoke(() =>
                    {
                        this.progress.Value = 0;
                    });

                    await mgr.DownloadReleases(updateInfo.ReleasesToApply, progress =>
                    {
                        this.Dispatcher.Invoke(new Action<int>((p) =>
                        {
                            this.message.Content = "New version is found, download update: " + progress.ToString() + "%";
                            this.progress.Value = progress;
                        }), System.Windows.Threading.DispatcherPriority.Normal, progress);
                    });

                    this.message.Content = "Finish download, applying update...";

                    await mgr.ApplyReleases(updateInfo);

                    await mgr.CreateUninstallerRegistryEntry();
                }

                this.message.Content = "New release is ready and applied, restart to activate new update.";

                this.reload.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                this.message.Content = ex.Message;
            }
        }

        private void MenuItem_CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.Directory.GetCurrentDirectory().EndsWith("\\Debug")) return;

            var local = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Cloud77-Release");
            var remote = "https://www.cloud77.top/statics/cloud77-wpf/stable/releases";
            var path = (bool)this.remote.IsChecked ? remote : local;

            this.message.Content = path;
            AutoUpdate(path);
        }

        private void MenuItem_Restart_Click_(object sender, RoutedEventArgs e)
        {
            Task.Run(() => UpdateManager.RestartApp());
        }

        private void Button_Help_Click(object sender, RoutedEventArgs e)
        {

        }

        private List<string> tabNames = new List<string>();

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            string name = (sender as ListBoxItem).Name;
            var index = tabNames.IndexOf(name);
            if (this.tabs.SelectedIndex != index)
            {
                this.tabs.SelectedIndex = index;
            }
        }

        private void MenuItem_AboutMe_Click(object sender, RoutedEventArgs e)
        {
            this.ShowMessageAsync("hello", "welcome to Cloud77 app");
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string selectedLanguage = "english";

        private void MenuItem_Language_Click(object sender, RoutedEventArgs e)
        {
            string language = (sender as MenuItem).Name.ToString();

            if (language == selectedLanguage) return;

            //"locales\\{0}.xaml"
            ResourceDictionary newDict = new ResourceDictionary();
            newDict.Source = new Uri(string.Format("{0}.xaml", language), UriKind.Relative);
            App.LoadComponent(newDict.Source);
            App.Current.Resources.MergedDictionaries.Add(newDict);

            ResourceDictionary currentDict = App.Current.Resources.MergedDictionaries.Where(a => a.Source.OriginalString == string.Format("{0}.xaml", selectedLanguage)).FirstOrDefault();
            if (currentDict == null) return;
            App.Current.Resources.MergedDictionaries.Remove(currentDict);

            selectedLanguage = language;
        }

        private async  void MenuItem_Dialog_Outside_Click(object sender, RoutedEventArgs e)
        {
            var dialog = (BaseMetroDialog)this.Resources["CustomDialogTest"];
            dialog.DialogSettings.ColorScheme = MetroDialogOptions.ColorScheme;
            dialog = dialog.ShowDialogExternally();
            await Task.Delay(5000);
            await dialog.RequestCloseAsync();
        }

        private async void MenuItem_Dialog_Login_Click(object sender, RoutedEventArgs e)
        {
            LoginDialogData result = await this.ShowLoginAsync("Authentication", "Enter your credentials",
                new LoginDialogSettings
                {
                    ColorScheme = this.MetroDialogOptions.ColorScheme,
                    InitialUsername = "MahApps",
                    RememberCheckBoxVisibility = Visibility.Visible
                });
            if (result == null)
            {

            }
            else
            {
                MessageDialogResult messageResult = await this.ShowMessageAsync("Authentication Information", String.Format("Username: {0}\nPassword: {1}", result.Username, result.Password));
            }
        }

        private void MenuItem_Theme_Click(object sender, RoutedEventArgs e)
        {
            ThemeManager.Current.ChangeTheme(Application.Current, $"Light.{(sender as MenuItem).Name.ToString()}");
        }

        private void Button_Customer_Click(object sender, RoutedEventArgs e)
        {
            var window = new CustomerWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }
        
        private int count = 0;

        private void Button_New_Click(object sender, RoutedEventArgs e)
        {
            var window = new CaseTypeWindow();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (window.ShowDialog() == true)
            {
                count++;

                var item = new TabItem();
                item.Name = $"case{count}";
                
                var caseA = new CaseAUserControl();
                item.Content = caseA;
                this.tabs.Items.Add(item);

                this.tabNames.Add(item.Name);

                var listboxitem = new ListBoxItem();
                listboxitem.Name = $"case{count}";
                listboxitem.Content = $"Case A {count}";
                listboxitem.Selected += ListBoxItem_Selected;

                listboxitem.Template = (ControlTemplate)this.Resources["myItem"];
                listboxitem.Style = (Style)this.Resources["myItemStyle"];

                this.listbox.Items.Add(listboxitem);

                listboxitem.RaiseEvent(new RoutedEventArgs(ListBoxItem.SelectedEvent));
            }
        }
    }
}
