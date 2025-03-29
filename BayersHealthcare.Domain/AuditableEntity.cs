using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BayersHealthcare.Domain
{
    public class AuditableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string? Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
