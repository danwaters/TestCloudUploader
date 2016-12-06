using System;
using System.IO;

namespace TestCloudUploader.Logic
{
    public class UploadProfile
    {
        public string Name { get; set; }
        public string PathToCsproj { get; set; }
        public DeviceSet Devices { get; set; }
        public Uploader Uploader { get; set; }
        public string Series { get; set; }
        public string Locale { get; set; }

        public UploadProfile()
        {
            Devices = new DeviceSet("", "");
            Uploader = new Uploader();
        }

        public string ApiKey { get; set; }
        public string UploaderEmail { get; set; }
        public string PathToBinary { get; set; }
        public string PathToTests { get; set; }
        public bool CaptureVideo { get; set; }

        private bool IsAndroid
        {
            get
            {
                return PathToBinary.EndsWith(".apk");
            }
        } 

        private bool IsIOS
        {
            get
            {
                return PathToBinary.EndsWith(".ipa");
            }
        }

        public static UploadProfile Example = new UploadProfile()
        {
            Uploader = new Uploader(@"C:\Users\dawate\Source\Repos\EmployeeOrientation\EmployeeOrientation\packages\Xamarin.UITest.1.3.8\tools\test-cloud.exe"),
            ApiKey = "6fdea2d1a3158d0c0d893ab0af32c61b",
            Devices = new DeviceSet("95b83459", "Sample 10 Android"),
            Series = "uploaded",
            UploaderEmail = "dan@xamarin.com",
            Locale = "en_US",
            PathToBinary = @"C:\Users\dawate\Downloads\com.xamarin.bluespiral.android.apk",
            PathToTests = @"C:\Users\dawate\Source\Repos\EmployeeOrientation\EmployeeOrientation\EmployeeOrientation.UITests\bin\Release"

            /*
             * submit "C:\Users\dawate\Downloads\com.xamarin.bluespiral.android.apk" 6fdea2d1a3158d0c0d893ab0af32c61b --devices 95b83459 --series "uploaded" --locale "en_US" --user "dan@xamarin.com" --assembly-dir "C:\Users\dawate\Source\Repos\EmployeeOrientation\EmployeeOrientation\EmployeeOrientation.UITests\bin\Debug"
             */
        };
    }
}
