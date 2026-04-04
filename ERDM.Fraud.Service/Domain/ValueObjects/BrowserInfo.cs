using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class BrowserInfo
    {
        [BsonElement("userAgent")]
        public string UserAgent { get; set; }

        [BsonElement("browserName")]
        public string BrowserName { get; set; }

        [BsonElement("browserVersion")]
        public string BrowserVersion { get; set; }

        [BsonElement("language")]
        public string Language { get; set; }

        [BsonElement("cookiesEnabled")]
        public bool CookiesEnabled { get; set; }

        [BsonElement("javascriptEnabled")]
        public bool JavascriptEnabled { get; set; }

        [BsonElement("screenResolution")]
        public string ScreenResolution { get; set; }

        [BsonElement("timezone")]
        public string Timezone { get; set; }

        // Public parameterless constructor for MongoDB
        public BrowserInfo()
        {
            UserAgent = string.Empty;
            BrowserName = string.Empty;
            BrowserVersion = string.Empty;
            Language = string.Empty;
            ScreenResolution = string.Empty;
            Timezone = string.Empty;
        }

        public BrowserInfo(string userAgent, string browserName, string browserVersion,
            string language, bool cookiesEnabled, bool javascriptEnabled,
            string screenResolution, string timezone)
        {
            UserAgent = userAgent;
            BrowserName = browserName;
            BrowserVersion = browserVersion;
            Language = language;
            CookiesEnabled = cookiesEnabled;
            JavascriptEnabled = javascriptEnabled;
            ScreenResolution = screenResolution;
            Timezone = timezone;
        }
    }
}