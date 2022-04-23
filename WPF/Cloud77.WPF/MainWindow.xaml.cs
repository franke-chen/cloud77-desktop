using System;
using System.Collections.Generic;
using System.Linq;
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
using MahApps.Metro.Controls;
using Squirrel;

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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            this.Title = $"Main window {version}";

            Task.Run(() =>
            {
                var t = Task.Delay(2000);
                t.Wait();

                this.message.Invoke(() =>
                {
                    this.message.Text = "Welcome to app";
                });
            });
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
                            this.message.Text = "Checking updates " + progress.ToString() + "%";
                        });
                    });

                    if (updateInfo.ReleasesToApply == null || updateInfo.ReleasesToApply.Count == 0)
                    {
                        this.message.Invoke(new Action(() =>
                        {
                            this.message.Text = "No new update. You have latest version now.";
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
                            this.message.Text = "New version is found, download update: " + progress.ToString() + "%";
                            this.progress.Value = progress;
                        }), System.Windows.Threading.DispatcherPriority.Normal, progress);
                    });

                    this.message.Text = "Finish download, applying update...";

                    await mgr.ApplyReleases(updateInfo);

                    await mgr.CreateUninstallerRegistryEntry();
                }

                this.message.Text = "New release is ready and applied, restart to activate new update.";

                this.reload.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                this.message.Text = ex.Message;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (System.IO.Directory.GetCurrentDirectory().EndsWith("\\Debug")) return;

            var local = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Cloud77-Release");

            var remote = "https://www.cloud77.top/statics/cloud77-wpf/stable/releases";

            var path = (bool)this.remote.IsChecked ? remote : local;
            this.message.Text = path;
            AutoUpdate(path);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Task.Run(() => UpdateManager.RestartApp());
        }
    }
}
