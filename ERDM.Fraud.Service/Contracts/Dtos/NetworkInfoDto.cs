namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class NetworkInfoDto
    {
        public string IpAddress { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string Isp { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public bool IsProxy { get; set; }
        public bool IsVpn { get; set; }
        public bool IsTor { get; set; }
    }
}
