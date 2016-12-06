using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestCloudUploader.Portable
{
    public class Uploader
    {
        public string UploaderPath { get; private set; }

        public Uploader(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not find test-cloud.exe.");

            UploaderPath = path;
        }
    }
}
