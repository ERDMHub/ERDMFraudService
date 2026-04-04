namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class VerificationDetails
    {
        public bool DocumentAuthentic { get; set; }
        public bool FaceMatch { get; set; }
        public decimal FaceMatchScore { get; set; }
        public bool DataConsistent { get; set; }
        public List<string> Flags { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}
