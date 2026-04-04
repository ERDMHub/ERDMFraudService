using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public class IdentityVerificationService : IIdentityVerificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IdentityVerificationService> _logger;
        private readonly IConfiguration _configuration;

        public IdentityVerificationService(
            IHttpClientFactory httpClientFactory,
            ILogger<IdentityVerificationService> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<VerificationResult> VerifyDocument(string documentImageUrl, string selfieImageUrl, string documentType)
        {
            try
            {
                _logger.LogInformation("Starting document verification for type {DocumentType}", documentType);

                // Step 1: Validate document authenticity
                var documentValidation = await ValidateDocument(documentImageUrl, documentType);

                // Step 2: Extract data from document
                var extractedData = await ExtractDocumentData(documentImageUrl);

                // Step 3: Match face from selfie to document
                var faceMatch = await MatchFaceToDocument(selfieImageUrl, documentImageUrl);

                // Step 4: Calculate overall score
                var score = CalculateOverallScore(documentValidation, faceMatch, extractedData);

                // Step 5: Build verification details
                var details = new VerificationDetailsDto
                {
                    DocumentAuthentic = documentValidation.IsValid,
                    FaceMatch = faceMatch.IsMatch,
                    FaceMatchScore = faceMatch.MatchScore,
                    DataConsistent = ValidateDataConsistency(extractedData),
                    Flags = documentValidation.ValidationFlags,
                    Warnings = documentValidation.Warnings
                };

                return new VerificationResult
                {
                    Score = score,
                    Details = details
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying document");
                return new VerificationResult
                {
                    Score = 0,
                    Details = new VerificationDetailsDto
                    {
                        Flags = new List<string> { "VerificationFailed" },
                        Warnings = new List<string> { ex.Message }
                    }
                };
            }
        }

        public async Task<DocumentValidationResult> ValidateDocument(string imageUrl, string documentType)
        {
            // In production, this would call a third-party IDV service like:
            // - Onfido
            // - Jumio
            // - IDnow
            // - Veriff

            await Task.Delay(100); // Simulate API call

            // Simulate validation logic
            var random = new Random();
            var isValid = random.Next(100) > 10; // 90% success rate for demo

            return new DocumentValidationResult
            {
                IsValid = isValid,
                AuthenticityScore = isValid ? random.Next(70, 100) : random.Next(20, 69),
                ValidationFlags = isValid ? new List<string>() : new List<string> { "DocumentTampered", "HologramMissing" },
                Warnings = isValid ? new List<string>() : new List<string> { "Security features not detected" }
            };
        }

        public async Task<FaceMatchResult> MatchFaceToDocument(string selfieUrl, string documentImageUrl)
        {
            // In production, this would call a facial recognition API
            await Task.Delay(100); // Simulate API call

            var random = new Random();
            var matchScore = random.Next(0, 100);

            return new FaceMatchResult
            {
                IsMatch = matchScore >= 70,
                MatchScore = matchScore,
                FacialFeatures = new Dictionary<string, decimal>
                {
                    { "EyeDistance", random.Next(40, 60) },
                    { "NoseWidth", random.Next(20, 40) },
                    { "MouthWidth", random.Next(30, 50) }
                }
            };
        }

        public async Task<DataExtractionResult> ExtractDocumentData(string imageUrl)
        {
            // In production, this would use OCR and data extraction
            await Task.Delay(100); // Simulate API call

            return new DataExtractionResult
            {
                FullName = "John Doe",
                DocumentNumber = "AB123456",
                DateOfBirth = new DateTime(1990, 1, 1),
                IssueDate = DateTime.UtcNow.AddYears(-5),
                ExpiryDate = DateTime.UtcNow.AddYears(5),
                Address = "123 Main St, Cityville",
                ExtractedFields = new Dictionary<string, string>
                {
                    { "IssuingAuthority", "Government Authority" },
                    { "DocumentClass", "Standard" }
                }
            };
        }

        private int CalculateOverallScore(DocumentValidationResult documentValidation, FaceMatchResult faceMatch, DataExtractionResult extractedData)
        {
            decimal score = 0;

            // Document authenticity contributes 40%
            score += documentValidation.AuthenticityScore * 0.4m;

            // Face match contributes 40%
            score += faceMatch.MatchScore * 0.4m;

            // Data consistency contributes 20%
            var dataConsistent = ValidateDataConsistency(extractedData);
            score += dataConsistent ? 20m : 0m;

            // Cap at 100 and convert to int properly
            return (int)Math.Min(Math.Round(score), 100m);
        }

        private bool ValidateDataConsistency(DataExtractionResult extractedData)
        {
            // Check if extracted data has reasonable values
            if (string.IsNullOrEmpty(extractedData.FullName)) return false;
            if (string.IsNullOrEmpty(extractedData.DocumentNumber)) return false;
            if (!extractedData.DateOfBirth.HasValue) return false;
            if (extractedData.DateOfBirth > DateTime.UtcNow.AddYears(-18)) return false; // Must be at least 18
            if (extractedData.ExpiryDate < DateTime.UtcNow) return false;

            return true;
        }
    }
}
