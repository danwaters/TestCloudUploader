using System.ComponentModel;
using System.IO;

namespace TestCloudUploader.Logic
{
    public class Uploader : INotifyPropertyChanged
    {
        public string UploaderPath { get; private set; }
        public string Status { get; set; }

        public Uploader()
        {
            Status = "New Uploader";
        }

        public Uploader(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not find test-cloud.exe.");

            UploaderPath = path;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateStatus(string status)
        {
            this.Status = status;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Status"));
        }
    }
}
