using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Onboarding.CORE.Entities
{
    public class Actividad
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        // ✔ valor por defecto correcto
        [BsonElement("tipo")]
        public string Tipo { get; set; } = "actividad";

        [BsonElement("fecha_inicio")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaInicio { get; set; }

        [BsonElement("fecha_fin")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime FechaFin { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } = "Pendiente";

        // ✔ ahora si coincide con tu DTO
        [BsonElement("usuario_ref")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UsuarioRef { get; set; }
    }
}
