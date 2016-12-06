using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace TestCloudUploader.Logic
{
    public class ProfileRunner
    {
        private SaveProfile profile;
        public Process TestCloudProcess;

        public ProfileRunner(SaveProfile profile)
        {
            profile.Validate();
            this.profile = profile;
            this.profile.CaptureVideo = this.profile.IsAndroid;
            var startInfo = new ProcessStartInfo("cmd.exe");
            startInfo.Arguments = $"/K {profile.UploaderPath} {GenerateProfileArguments(profile)}";
            //startInfo.Arguments = "/K " + @"""C:\Users\dawate\Documents\visual studio 2015\Projects\stuff\stuff\bin\Debug\stuff.exe""";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            TestCloudProcess = new Process() { StartInfo = startInfo, EnableRaisingEvents = true };
        }

        public void Run()
        {
            TestCloudProcess.Start();
            TestCloudProcess.BeginOutputReadLine();
            TestCloudProcess.BeginErrorReadLine();
            TestCloudProcess.WaitForExit();
        }

        public string GenerateProfileArguments(SaveProfile p)
        {
            string arguments =
                $"submit \"{p.PathToBinary}\" {p.ApiKey} --devices {p.DeviceSet} --series \"{p.Series}\" --locale \"{p.Locale}\" --user \"{p.UploaderEmail}\" --assembly-dir \"{p.PathToTests}\"";

            if (profile.CaptureVideo)
                arguments += " --test-param screencapture:true";

            return arguments;
        }
    }
}
