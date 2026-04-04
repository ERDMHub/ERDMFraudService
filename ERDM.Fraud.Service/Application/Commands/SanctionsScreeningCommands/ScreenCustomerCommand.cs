using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class ScreenCustomerCommand : CommandBase<SanctionsScreeningResponseDto>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
        public List<string> ListsToScreen { get; set; } = new();
    }
}
