namespace ERDM.Fraud.Service.Domain.Enums
{
    public enum FraudType
    {
        SyntheticIdentity = 1,
        DocumentForgery = 2,
        AccountTakeover = 3,
        ApplicationFraud = 4,
        FirstPartyFraud = 5,
        ThirdPartyFraud = 6,
        MoneyMuling = 7,
        SanctionsViolation = 8
    }

}
