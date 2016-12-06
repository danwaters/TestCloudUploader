using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using NLog;
using System.Reflection;
using Microsoft.Win32;
using System.Windows;

namespace TestCloudUploader.Logic
{
    [DataContract]
    public class SaveProfile
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DeviceSet { get; set; }

        [DataMember]
        public string UploaderPath { get; set; }

        [DataMember]
        public string Series { get; set; }

        [DataMember]
        public string Locale { get; set; }

        [DataMember]
        public string ApiKey { get; set; }

        [DataMember]
        public string UploaderEmail { get; set; }

        [DataMember]
        public string PathToBinary { get; set; }

        [DataMember]
        public string PathToTests { get; set; }

        [DataMember]
        public bool CaptureVideo { get; set; }

        public bool IsAndroid { get { return PathToBinary.EndsWith(".apk"); } }
        public bool IsIOS { get { return PathToBinary.EndsWith(".ipa"); } }
        private static Logger log = LogManager.GetCurrentClassLogger();
        private readonly string profilePath = "";

        public SaveProfile()
        {
            Id = Guid.NewGuid().ToString().Replace("{", "").Replace("}", "");
            Name = "New Profile";
            Locale = "en_US";
            Series = "master";
            profilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
        
        public static SaveProfile Load(string path)
        {
            if (!File.Exists(path) || !path.EndsWith(".json"))
                throw new FileNotFoundException("A matching json profile was not found.");

            var serializer = new DataContractJsonSerializer(typeof(SaveProfile));

            SaveProfile profile = new SaveProfile();
            using (var reader = new FileStream(path, FileMode.Open))
            {
                try
                {
                    profile = (SaveProfile)serializer.ReadObject(reader);
                    log.Info($"Loaded profile id {profile.Id} ({profile.Name})");
                }
                catch (Exception ex)
                {
                    log.Error(ex, "An error occurred trying to load the profile.");
                }
                finally
                {
                    reader.Close();
                }
            }

            return profile;
        }

        public static SaveProfile Example = new SaveProfile
        {
            UploaderPath = @"C:\Users\dawate\Source\Repos\EmployeeOrientation\EmployeeOrientation\packages\Xamarin.UITest.1.3.8\tools\test-cloud.exe",
            ApiKey = "6fdea2d1a3158d0c0d893ab0af32c61b",
            DeviceSet = "95b83459",
            Series = "uploaded",
            UploaderEmail = "dan@xamarin.com",
            Locale = "en_US",
            PathToBinary = @"C:\Users\dawate\Downloads\com.xamarin.bluespiral.android.apk",
            PathToTests = @"C:\Users\dawate\Source\Repos\EmployeeOrientation\EmployeeOrientation\EmployeeOrientation.UITests\bin\Release"
        };

        public void Validate()
        {
            if (!File.Exists(UploaderPath))
                throw new ArgumentOutOfRangeException("Uploader path could not be found.");

            if (!Directory.Exists(PathToTests))
                throw new ArgumentOutOfRangeException("Path to tests does not exist on disk.");

            if (!File.Exists(PathToBinary))
                throw new ArgumentOutOfRangeException("App binary not found at the specified path.");
        }

        public void SaveToDisk()
        {
            var serializer = new DataContractJsonSerializer(typeof(SaveProfile));
            var destinationPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            destinationPath = Path.Combine(destinationPath, "XTCProfiles");
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);

            using (var stream = new FileStream(Path.Combine(destinationPath, Id+".json"), FileMode.Create))
            {
                try
                {
                    serializer.WriteObject(stream, this);
                    log.Info("Updated profile: " + this.Id);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                }
                finally
                {
                    stream.Close();
                }
            }
                
        }
    }
}
