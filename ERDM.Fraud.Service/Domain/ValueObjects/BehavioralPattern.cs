using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class BehavioralPattern
    {
        [BsonElement("patternType")]
        public string PatternType { get; set; }

        [BsonElement("patternValue")]
        public string PatternValue { get; set; }

        [BsonElement("detectedAt")]
        public DateTime DetectedAt { get; set; }

        [BsonElement("confidence")]
        public decimal Confidence { get; set; }

        // Public parameterless constructor for MongoDB
        public BehavioralPattern()
        {
            PatternType = string.Empty;
            PatternValue = string.Empty;
        }

        public BehavioralPattern(string patternType, string patternValue, decimal confidence)
        {
            PatternType = patternType;
            PatternValue = patternValue;
            Confidence = confidence;
            DetectedAt = DateTime.UtcNow;
        }
    }

}
