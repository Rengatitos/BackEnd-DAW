using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Onboarding.CORE.Core.Entities
{
    public class EtapaOnboarding
    {
        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("descripcion")]
        public string Descripcion { get; set; } = string.Empty;

        [BsonElement("documentos")]
        public List<string> Documentos { get; set; } = new();

        [BsonElement("urls")]
        public List<string> Urls { get; set; } = new();

        [BsonElement("proximosPasos")]
        public List<string> ProximosPasos { get; set; } = new();

        [BsonElement("consejos")]
        public List<string> Consejos { get; set; } = new();
    }
}