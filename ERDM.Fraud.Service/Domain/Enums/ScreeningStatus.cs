namespace ERDM.Fraud.Service.Domain.Enums
{
    public enum ScreeningStatus
    {
        Clear = 1,
        Hit = 2,
        Pending = 3,
        FalsePositive = 4,
        ConfirmedMatch = 5
    }
}
