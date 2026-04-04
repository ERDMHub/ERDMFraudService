namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public interface IBiometricService
    {
        Task<LivenessResult> DetectLiveness(string videoUrl);
        Task<string> GenerateBiometricHash(byte[] biometricData);
        Task<bool> CompareBiometrics(string hash1, string hash2, decimal threshold = 0.8m);
        Task<BiometricEnrollmentResult> EnrollBiometric(string customerId, byte[] biometricData, string biometricType);
        Task<BiometricVerificationResult> VerifyBiometric(string customerId, byte[] biometricData);
    }

    public class LivenessResult
    {
        public bool IsAlive { get; set; }
        public decimal ConfidenceScore { get; set; }
        public List<string> ChecksPassed { get; set; } = new();
        public List<string> ChecksFailed { get; set; } = new();
        public string Method { get; set; } = string.Empty;
    }

    public class BiometricEnrollmentResult
    {
        public bool Success { get; set; }
        public string BiometricHash { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class BiometricVerificationResult
    {
        public bool IsMatch { get; set; }
        public decimal MatchScore { get; set; }
        public string Message { get; set; } = string.Empty;
    }
    
}
