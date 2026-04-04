namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class CustomerProfileDataDto
    {
        public string FullName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Ssn { get; set; } = string.Empty;
        public string Tin { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty; public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployerName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public decimal MonthlyIncome { get; set; }
        public string EmploymentStatus { get; set; } = string.Empty;
        public int EmploymentYears { get; set; }
        public int EmploymentMonths { get; set; }
        public string MaritalStatus { get; set; } = string.Empty;
        public int Dependents { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
        public string Citizenship { get; set; } = string.Empty;
        public string IdType { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public DateTime IdIssueDate { get; set; }
        public DateTime IdExpiryDate { get; set; }
        public string IdIssuingCountry { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime ProfileCreatedAt { get; set; }
        public DateTime ProfileUpdatedAt { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsAddressVerified { get; set; }
        public Dictionary<string, object> AdditionalAttributes { get; set; } = new();
    }
}
