using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class HardwareInfo
    {
        [BsonElement("deviceModel")]
        public string DeviceModel { get; set; }

        [BsonElement("deviceBrand")]
        public string DeviceBrand { get; set; }

        [BsonElement("cpuCores")]
        public string CpuCores { get; set; }

        [BsonElement("ramSize")]
        public string RamSize { get; set; }

        [BsonElement("graphicsCard")]
        public string GraphicsCard { get; set; }

        [BsonElement("isEmulator")]
        public bool IsEmulator { get; set; }

        [BsonElement("isRooted")]
        public bool IsRooted { get; set; }

        // Public parameterless constructor for MongoDB
        public HardwareInfo()
        {
            DeviceModel = string.Empty;
            DeviceBrand = string.Empty;
            CpuCores = string.Empty;
            RamSize = string.Empty;
            GraphicsCard = string.Empty;
        }

        public HardwareInfo(string deviceModel, string deviceBrand, string cpuCores,
            string ramSize, string graphicsCard, bool isEmulator, bool isRooted)
        {
            DeviceModel = deviceModel;
            DeviceBrand = deviceBrand;
            CpuCores = cpuCores;
            RamSize = ramSize;
            GraphicsCard = graphicsCard;
            IsEmulator = isEmulator;
            IsRooted = isRooted;
        }
    }
}
