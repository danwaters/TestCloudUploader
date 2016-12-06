using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestCloudUploader.Portable
{
    public class UploadProfile
    {
        public string Name { get; set; }
        public string PathToCsproj { get; set; }
        public DeviceSet Devices { get; set; }
        public Uploader Uploader { get; set; }
        public string Series { get; set; }
        public string Locale { get; set; }
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
            ApiKey = "6fdea2d1a3158d0c0d893ab0af32c61b",
            Devices = new DeviceSet("95b83459", "Sample 10 Android"),
            Series = "uploaded",
            UploaderEmail = "dan@xamarin.com",
            Locale = "en_US",
            PathToTests = ""
        };
    }
}
