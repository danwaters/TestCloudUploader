using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestCloudUploader.Logic;

namespace TestCloudUploader.UnitTests
{
    [TestClass]
    public class UploaderTests
    {
        [TestMethod]
        public void Correctly_generate_argument_string()
        {
            var expected = "submit \"C:\\Users\\dawate\\Downloads\\com.xamarin.bluespiral.android.apk\" 6fdea2d1a3158d0c0d893ab0af32c61b --devices 95b83459 --series \"uploaded\" --locale \"en_US\" --user \"dan@xamarin.com\" --assembly-dir \"C:\\Users\\dawate\\Source\\Repos\\EmployeeOrientation\\EmployeeOrientation\\EmployeeOrientation.UITests\\bin\\Debug\"";
            var r = new ProfileRunner(SaveProfile.Example);
            var s = r.GenerateProfileArguments(SaveProfile.Example);
            Assert.AreEqual(expected, s);
        }
    }
}
