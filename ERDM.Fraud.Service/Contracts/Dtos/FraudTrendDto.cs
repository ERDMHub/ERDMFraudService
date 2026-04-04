namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudTrendDto
    {
        public DateTime Date { get; set; }
        public int CasesCount { get; set; }
        public decimal AverageRiskScore { get; set; }
    }
}
