using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class NetworkInfo
    {
        [BsonElement("ipAddress")]
        public string IpAddress { get; set; }

        [BsonElement("macAddress")]
        public string MacAddress { get; set; }

        [BsonElement("isp")]
        public string Isp { get; set; }

        [BsonElement("country")]
        public string Country { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("isProxy")]
        public bool IsProxy { get; set; }

        [BsonElement("isVpn")]
        public bool IsVpn { get; set; }

        [BsonElement("isTor")]
        public bool IsTor { get; set; }

        // Public parameterless constructor for MongoDB
        public NetworkInfo()
        {
            IpAddress = string.Empty;
            MacAddress = string.Empty;
            Isp = string.Empty;
            Country = string.Empty;
            City = string.Empty;
        }

        public NetworkInfo(string ipAddress, string macAddress, string isp,
            string country, string city, bool isProxy, bool isVpn, bool isTor)
        {
            IpAddress = ipAddress;
            MacAddress = macAddress;
            Isp = isp;
            Country = country;
            City = city;
            IsProxy = isProxy;
            IsVpn = isVpn;
            IsTor = isTor;
        }
    }

}
