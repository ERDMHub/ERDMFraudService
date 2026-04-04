namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudCaseNoteReadModel
    {
        public string NoteId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedByName { get; set; } = string.Empty;
        public bool IsInternal { get; set; }
        public List<string> Mentions { get; set; } = new();
    }
}
