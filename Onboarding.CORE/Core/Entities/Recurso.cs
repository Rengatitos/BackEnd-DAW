using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Onboarding.CORE.Entities
{
    public class Recurso 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [BsonElement("link")]
        public string Link { get; set; } = string.Empty;

        [BsonElement("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [BsonElement("fecha_subida")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

        [BsonElement("admin_ref")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? AdminRef { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } = "Activo";
    }
}
