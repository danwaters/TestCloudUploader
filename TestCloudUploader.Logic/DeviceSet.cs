namespace TestCloudUploader.Logic
{
    public class DeviceSet
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }

        public DeviceSet(string id, string name)
        {
            Identifier = id;
            DisplayName = name;
        }
    }
}
