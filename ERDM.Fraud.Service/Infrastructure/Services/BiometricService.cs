namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public class BiometricService : IBiometricService
    {
        private readonly ILogger<BiometricService> _logger;
        private readonly ICacheService _cacheService;

        public BiometricService(ILogger<BiometricService> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<LivenessResult> DetectLiveness(string videoUrl)
        {
            try
            {
                _logger.LogInformation("Performing liveness detection on video {VideoUrl}", videoUrl);

                // In production, this would call a liveness detection service like:
                // - FaceTec
                // - iProov
                // - Onfido

                await Task.Delay(150); // Simulate processing

                var random = new Random();
                var isAlive = random.Next(100) > 5; // 95% success rate for demo

                var checksPassed = new List<string>();
                var checksFailed = new List<string>();

                // Simulate various liveness checks
                var eyeBlinkDetected = random.Next(100) > 10;
                var headMovementDetected = random.Next(100) > 15;
                var lightingConsistent = random.Next(100) > 5;
                var noSpoofingDetected = random.Next(100) > 8;

                if (eyeBlinkDetected) checksPassed.Add("EyeBlinkDetection");
                else checksFailed.Add("EyeBlinkDetection");

                if (headMovementDetected) checksPassed.Add("HeadMovementDetection");
                else checksFailed.Add("HeadMovementDetection");

                if (lightingConsistent) checksPassed.Add("LightingConsistency");
                else checksFailed.Add("LightingConsistency");

                if (noSpoofingDetected) checksPassed.Add("SpoofingDetection");
                else checksFailed.Add("SpoofingDetection");

                var confidenceScore = isAlive ? random.Next(70, 100) : random.Next(10, 69);

                return new LivenessResult
                {
                    IsAlive = isAlive && confidenceScore >= 70,
                    ConfidenceScore = confidenceScore,
                    ChecksPassed = checksPassed,
                    ChecksFailed = checksFailed,
                    Method = "PassiveLiveness"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting liveness");
                return new LivenessResult
                {
                    IsAlive = false,
                    ConfidenceScore = 0,
                    ChecksFailed = new List<string> { "ProcessingError" }
                };
            }
        }

        public async Task<string> GenerateBiometricHash(byte[] biometricData)
        {
            // In production, use a secure hashing algorithm
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = sha256.ComputeHash(biometricData);
            return Convert.ToBase64String(hash);
        }

        public async Task<bool> CompareBiometrics(string hash1, string hash2, decimal threshold = 0.8m)
        {
            // In production, use proper biometric matching algorithm
            await Task.Delay(50);

            if (string.IsNullOrEmpty(hash1) || string.IsNullOrEmpty(hash2))
                return false;

            // Simulate comparison - in reality, you'd use Hamming distance or similar
            var similarity = CalculateSimilarity(hash1, hash2);
            return similarity >= threshold;
        }

        public async Task<BiometricEnrollmentResult> EnrollBiometric(string customerId, byte[] biometricData, string biometricType)
        {
            try
            {
                _logger.LogInformation("Enrolling biometric for customer {CustomerId}, type {BiometricType}", customerId, biometricType);

                var biometricHash = await GenerateBiometricHash(biometricData);

                // Store in cache for quick retrieval
                await _cacheService.SetAsync($"biometric:{customerId}:{biometricType}", biometricHash, TimeSpan.FromDays(365));

                return new BiometricEnrollmentResult
                {
                    Success = true,
                    BiometricHash = biometricHash,
                    Message = "Biometric enrolled successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enrolling biometric for customer {CustomerId}", customerId);
                return new BiometricEnrollmentResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BiometricVerificationResult> VerifyBiometric(string customerId, byte[] biometricData)
        {
            try
            {
                _logger.LogInformation("Verifying biometric for customer {CustomerId}", customerId);

                var newHash = await GenerateBiometricHash(biometricData);
                var storedHash = await _cacheService.GetAsync<string>($"biometric:{customerId}:face");

                if (string.IsNullOrEmpty(storedHash))
                {
                    return new BiometricVerificationResult
                    {
                        IsMatch = false,
                        MatchScore = 0,
                        Message = "No biometric enrolled for this customer"
                    };
                }

                var isMatch = await CompareBiometrics(storedHash, newHash);
                var matchScore = isMatch ? new Random().Next(70, 100) : new Random().Next(0, 69);

                return new BiometricVerificationResult
                {
                    IsMatch = isMatch,
                    MatchScore = matchScore,
                    Message = isMatch ? "Biometric verified" : "Biometric does not match"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying biometric for customer {CustomerId}", customerId);
                return new BiometricVerificationResult
                {
                    IsMatch = false,
                    Message = ex.Message
                };
            }
        }

        private decimal CalculateSimilarity(string hash1, string hash2)
        {
            // Simple similarity calculation for demo
            var minLength = Math.Min(hash1.Length, hash2.Length);
            var matchingChars = 0;

            for (int i = 0; i < minLength; i++)
            {
                if (hash1[i] == hash2[i])
                    matchingChars++;
            }

            return (decimal)matchingChars / minLength;
        }
    }

}
