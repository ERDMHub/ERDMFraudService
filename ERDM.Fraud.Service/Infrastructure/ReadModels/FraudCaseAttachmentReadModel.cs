namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudCaseAttachmentReadModel
    {
        public string AttachmentId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
    }

}
