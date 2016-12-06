using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestCloudUploader.Logic;

namespace TestCloudUploader.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StringBuilder OutputText = new StringBuilder();
        private List<SaveProfile> profiles = new List<SaveProfile>();
        private SaveProfile CurrentProfile = new SaveProfile();
        private readonly string profilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XTCProfiles");

        public MainWindow()
        {
            this.DataContext = CurrentProfile;
            InitializeComponent();

            LoadProfilesInPath(profilePath);
        }

        private void LoadProfilesInPath(string path)
        {
            var files = Directory.GetFiles(path, "*.json");
            profiles.Clear();

            foreach (var file in files)
            {
                var profile = SaveProfile.Load(file);
                profiles.Add(profile);
            }

            listView.ItemsSource = profiles;
            //listView.SelectedIndex = 0;
        }

        private void TestCloudProcess_Exited(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Exited");
            Dispatcher.Invoke(() => tbOutput.AppendText("Upload complete."));
        }

        private void TestCloudProcess_ErrorDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Data);
            Dispatcher.Invoke(() => tbOutput.AppendText("Error: " + e.Data + "\r\n"));
            Dispatcher.Invoke(() => tbOutput.ScrollToEnd());
        }

        private void TestCloudProcess_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Data);
            Dispatcher.Invoke(() => tbOutput.AppendText(e.Data + "\r\n"));
            Dispatcher.Invoke(() => tbOutput.ScrollToEnd());
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnProfileDir_Click(object sender, RoutedEventArgs e)
        {
            var d = new FolderBrowserDialog();
            d.SelectedPath = profilePath;
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadProfilesInPath(d.SelectedPath);
            }
        }

        private void btnBrowseTestCloud_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".exe";
            ofd.FileName = "test-cloud.exe";
            if (ofd.ShowDialog() == true)
            {
                CurrentProfile.UploaderPath = ofd.FileName;
                tbTestCloud.Text = CurrentProfile.UploaderPath;
            }
        }

        private void btnBrowseToBinary_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "Apps (*.apk, *.ipa)|*.apk;*.ipa";
            if (ofd.ShowDialog() == true)
            {
                CurrentProfile.PathToBinary = ofd.FileName;
                tbAppBinary.Text = CurrentProfile.PathToBinary;
            }
        }

        private void btnBrowseTestOutput_Click(object sender, RoutedEventArgs e)
        {
            var d = new FolderBrowserDialog();
            d.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentProfile.PathToTests = d.SelectedPath;
                tbTestFolder.Text = CurrentProfile.PathToTests;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            CurrentProfile.SaveToDisk();
            System.Windows.MessageBox.Show("Profile saved.");
            LoadProfilesInPath(profilePath);
        }

        private void ListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems[0];
            CurrentProfile = (SaveProfile)item;
            DataContext = CurrentProfile;
        }

        private void btnRunProfile_Click(object sender, RoutedEventArgs e)
        {
            var runner = new ProfileRunner(CurrentProfile);
            runner.TestCloudProcess.OutputDataReceived += TestCloudProcess_OutputDataReceived;
            runner.TestCloudProcess.ErrorDataReceived += TestCloudProcess_ErrorDataReceived;
            runner.TestCloudProcess.Exited += TestCloudProcess_Exited;
            tbOutput.Clear();
            tbOutput.AppendText("Starting Test Cloud uploader...");
            Task.Run(() => runner.Run());
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            CurrentProfile = new SaveProfile();
            this.DataContext = CurrentProfile;
        }
    }

    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Devices { get; set; }

        public ProfileViewModel(string id, string name, string series, string devices)
        {
            Id = id;
            Name = name;
            Series = series;
            Devices = devices;
        }
    }
}
