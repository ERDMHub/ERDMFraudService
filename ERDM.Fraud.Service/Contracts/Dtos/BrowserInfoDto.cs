namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class BrowserInfoDto
    {
        public string UserAgent { get; set; } = string.Empty;
        public string BrowserName { get; set; } = string.Empty;
        public string BrowserVersion { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public bool CookiesEnabled { get; set; }
        public bool JavascriptEnabled { get; set; }
        public string ScreenResolution { get; set; } = string.Empty;
        public string Timezone { get; set; } = string.Empty;
    }
}
