using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Onboarding.CORE.Entities
{
    public class Usuario 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("correo")]
        public string Correo { get; set; } = string.Empty;

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } = string.Empty;

        [BsonElement("area")]
        public string? Area { get; set; }

        [BsonElement("rol_ref")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RolRef { get; set; }

        [BsonElement("nivel_onboarding")]
        public NivelOnboarding NivelOnboarding { get; set; } = new();

        [BsonElement("estado")]
        public string Estado { get; set; } = "Activo";
    }

    public class NivelOnboarding
    {
        [BsonElement("etapa")]
        public string Etapa { get; set; } = "Inicial";

        [BsonElement("porcentaje")]
        public int Porcentaje { get; set; } = 0;

        [BsonElement("ultima_actualizacion")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime UltimaActualizacion { get; set; } = DateTime.UtcNow;
    }
}
