using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public class SyntheticIdentityDetectionService : ISyntheticIdentityDetectionService
    {
        private readonly ILogger<SyntheticIdentityDetectionService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public SyntheticIdentityDetectionService(
            ILogger<SyntheticIdentityDetectionService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SyntheticIdentityDetectionResult> DetectSyntheticIdentity(CustomerProfileData profileData)
        {
            try
            {
                _logger.LogInformation("Detecting synthetic identity for customer {FullName}", profileData.FullName);

                var factorScores = new Dictionary<string, decimal>();
                var indicators = new List<string>();
                var recommendations = new List<string>();

                // Factor 1: Validate SSN (if provided)
                if (!string.IsNullOrEmpty(profileData.Ssn))
                {
                    var ssnScore = await ValidateSSN(profileData.Ssn);
                    factorScores["SSN_Validation"] = ssnScore;
                    if (ssnScore < 30) indicators.Add("InvalidSSN");
                }

                // Factor 2: Check email domain
                var emailScore = ValidateEmailDomain(profileData.Email);
                factorScores["Email_Domain"] = emailScore;
                if (emailScore < 40) indicators.Add("SuspiciousEmailDomain");

                // Factor 3: Check phone number
                var phoneScore = await ValidatePhoneNumber(profileData.PhoneNumber);
                factorScores["Phone_Validation"] = phoneScore;
                if (phoneScore < 50) indicators.Add("InvalidPhoneNumber");

                // Factor 4: Check address validity
                var addressScore = await ValidateAddress(profileData.Address);
                factorScores["Address_Validation"] = addressScore;
                if (addressScore < 40) indicators.Add("InvalidAddress");

                // Factor 5: Check age reasonableness
                var ageScore = ValidateAge(profileData.DateOfBirth);
                factorScores["Age_Validation"] = ageScore;
                if (ageScore < 50) indicators.Add("SuspiciousAge");

                // Factor 6: Check account age
                var accountAgeScore = ValidateAccountAge(profileData.ProfileCreatedAt);
                factorScores["Account_Age"] = accountAgeScore;
                if (accountAgeScore < 30) indicators.Add("NewAccount");

                // Factor 7: Check for known synthetic patterns
                var patternScore = await DetectKnownPatterns(profileData);
                factorScores["Synthetic_Patterns"] = patternScore;
                if (patternScore < 40) indicators.Add("KnownSyntheticPattern");

                // Calculate overall confidence score
                var overallScore = factorScores.Values.Average();

                // Determine if synthetic
                var isSynthetic = overallScore < 50;

                // Build recommendations
                if (isSynthetic)
                {
                    recommendations.Add("Request additional identity verification documents");
                    recommendations.Add("Perform enhanced due diligence");
                    recommendations.Add("Cross-reference with credit bureau data");
                    recommendations.Add("Review application manually");
                }
                else if (overallScore < 70)
                {
                    recommendations.Add("Review application with caution");
                    recommendations.Add("Verify selected information");
                }
                else
                {
                    recommendations.Add("Proceed with standard verification");
                }

                return new SyntheticIdentityDetectionResult
                {
                    IsSynthetic = isSynthetic,
                    ConfidenceScore = overallScore,
                    Indicators = indicators,
                    Recommendations = recommendations,
                    FactorScores = factorScores
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting synthetic identity");
                return new SyntheticIdentityDetectionResult
                {
                    IsSynthetic = true,
                    ConfidenceScore = 100,
                    Indicators = new List<string> { "DetectionError" },
                    Recommendations = new List<string> { "Manual review required" }
                };
            }
        }

        public async Task<List<SyntheticIdentityIndicator>> GetIndicators()
        {
            await Task.Delay(50);

            return new List<SyntheticIdentityIndicator>
            {
                new SyntheticIdentityIndicator { IndicatorId = "INV_SSN", Name = "Invalid SSN", Description = "Social Security Number does not exist or is invalid", Weight = 25 },
                new SyntheticIdentityIndicator { IndicatorId = "SUS_EMAIL", Name = "Suspicious Email", Description = "Email domain associated with disposable or temporary email services", Weight = 15 },
                new SyntheticIdentityIndicator { IndicatorId = "INV_PHONE", Name = "Invalid Phone", Description = "Phone number is disconnected or invalid", Weight = 20 },
                new SyntheticIdentityIndicator { IndicatorId = "INV_ADDR", Name = "Invalid Address", Description = "Address does not exist or is a commercial mail receiving agency", Weight = 20 },
                new SyntheticIdentityIndicator { IndicatorId = "NEW_ACCT", Name = "New Account", Description = "Account created very recently", Weight = 10 },
                new SyntheticIdentityIndicator { IndicatorId = "PATTERN_MATCH", Name = "Pattern Match", Description = "Matches known synthetic identity patterns", Weight = 30 }
            };
        }

        public async Task<IdentityValidationResult> ValidateIdentityAttributes(CustomerProfileData profileData)
        {
            var result = new IdentityValidationResult
            {
                IsValid = true,
                AttributeChecks = new Dictionary<string, bool>()
            };

            // Validate SSN format
            if (!string.IsNullOrEmpty(profileData.Ssn))
            {
                var isValidSSN = System.Text.RegularExpressions.Regex.IsMatch(profileData.Ssn, @"^\d{3}-\d{2}-\d{4}$");
                result.AttributeChecks["SSN_Format"] = isValidSSN;
                if (!isValidSSN) result.ValidationErrors.Add("Invalid SSN format");
            }

            // Validate email format
            var isValidEmail = System.Text.RegularExpressions.Regex.IsMatch(profileData.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            result.AttributeChecks["Email_Format"] = isValidEmail;
            if (!isValidEmail) result.ValidationErrors.Add("Invalid email format");

            // Validate phone format
            var isValidPhone = System.Text.RegularExpressions.Regex.IsMatch(profileData.PhoneNumber, @"^\+?[\d\s-]{10,}$");
            result.AttributeChecks["Phone_Format"] = isValidPhone;
            if (!isValidPhone) result.Warnings.Add("Phone number format unusual");

            // Validate age
            if (DateTime.TryParse(profileData.DateOfBirth, out var dob))
            {
                var age = DateTime.UtcNow.Year - dob.Year;
                var isValidAge = age >= 18 && age <= 120;
                result.AttributeChecks["Age_Range"] = isValidAge;
                if (!isValidAge) result.ValidationErrors.Add("Age out of valid range");
            }
            else
            {
                result.ValidationErrors.Add("Invalid date of birth");
            }

            result.IsValid = !result.ValidationErrors.Any();

            return result;
        }

        private async Task<decimal> ValidateSSN(string ssn)
        {
            // In production, call SSA API or use SSN validation service
            await Task.Delay(50);

            // Check SSN format
            if (!System.Text.RegularExpressions.Regex.IsMatch(ssn, @"^\d{3}-\d{2}-\d{4}$"))
                return 0;

            // Check for invalid SSN patterns
            var parts = ssn.Split('-');
            var area = int.Parse(parts[0]);
            var group = int.Parse(parts[1]);

            if (area == 0 || area == 666 || area > 772) return 20;
            if (group == 0) return 30;

            return new Random().Next(60, 100);
        }

        private decimal ValidateEmailDomain(string email)
        {
            var suspiciousDomains = new[] { "tempmail.com", "throwaway.com", "guerrillamail.com", "10minutemail.com" };
            var domain = email.Split('@').LastOrDefault()?.ToLower();

            if (string.IsNullOrEmpty(domain))
                return 0;

            if (suspiciousDomains.Contains(domain))
                return 10;

            if (domain.Contains("gmail") || domain.Contains("yahoo") || domain.Contains("outlook"))
                return 80;

            return 50;
        }

        private async Task<decimal> ValidatePhoneNumber(string phoneNumber)
        {
            await Task.Delay(30);

            // Check for obvious fake numbers
            var fakePatterns = new[] { "1234567890", "1111111111", "0000000000", "9999999999" };
            var cleaned = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (fakePatterns.Contains(cleaned))
                return 0;

            if (cleaned.Length < 10)
                return 20;

            return new Random().Next(50, 100);
        }

        private async Task<decimal> ValidateAddress(string address)
        {
            await Task.Delay(40);

            if (string.IsNullOrEmpty(address))
                return 0;

            // Check for PO Box (often used in fraud)
            if (address.ToLower().Contains("po box") || address.ToLower().Contains("p.o. box"))
                return 30;

            // Check for commercial mail receiving agencies
            var suspiciousTerms = new[] { "suite", "ste", "#", "unit", "apt" };
            if (suspiciousTerms.Any(t => address.ToLower().Contains(t)))
                return 60;

            return new Random().Next(50, 100);
        }

        private decimal ValidateAge(string dateOfBirth)
        {
            if (!DateTime.TryParse(dateOfBirth, out var dob))
                return 0;

            var age = DateTime.UtcNow.Year - dob.Year;

            if (age < 18) return 0;
            if (age < 21) return 40;
            if (age > 100) return 10;

            return 80;
        }

        private decimal ValidateAccountAge(DateTime createdAt)
        {
            var age = DateTime.UtcNow - createdAt;

            if (age.TotalDays < 1) return 10;
            if (age.TotalDays < 7) return 30;
            if (age.TotalDays < 30) return 50;
            if (age.TotalDays < 90) return 70;

            return 90;
        }

        private async Task<decimal> DetectKnownPatterns(CustomerProfileData profileData)
        {
            await Task.Delay(60);

            var random = new Random();
            var score = random.Next(0, 100);

            // Check for sequential numbers in SSN
            if (!string.IsNullOrEmpty(profileData.Ssn))
            {
                var numbers = profileData.Ssn.Replace("-", "");
                if (numbers == "123456789") score -= 30;
            }

            // Check for generic names
            var genericNames = new[] { "John Doe", "Jane Doe", "Test User", "Demo" };
            if (genericNames.Contains(profileData.FullName))
                score -= 40;

            // Check for impossible ages
            if (DateTime.TryParse(profileData.DateOfBirth, out var dob))
            {
                if (dob > DateTime.UtcNow || dob.Year < 1900)
                    score -= 50;
            }

            return Math.Max(0, Math.Min(100, score));
        }
    }
}
