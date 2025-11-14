using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Onboarding.CORE.Entities
{
    public class Rol
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [BsonElement("permisos")]
        public List<string> Permisos { get; set; } = new();

        [BsonElement("estado")]
        public string Estado { get; set; } = "Activo";
    }
}
