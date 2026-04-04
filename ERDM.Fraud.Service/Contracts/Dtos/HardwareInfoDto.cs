namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class HardwareInfoDto
    {
        public string DeviceModel { get; set; } = string.Empty;
        public string DeviceBrand { get; set; } = string.Empty;
        public string CpuCores { get; set; } = string.Empty;
        public string RamSize { get; set; } = string.Empty;
        public string GraphicsCard { get; set; } = string.Empty;
        public bool IsEmulator { get; set; }
        public bool IsRooted { get; set; }
    }
}
