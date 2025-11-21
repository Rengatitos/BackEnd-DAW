using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Onboarding.CORE.Entities
{
    public class NivelOnboarding
    {
        [BsonElement("etapa")]
        public string Etapa { get; set; } = string.Empty;

        [BsonElement("porcentaje")]
        public int Porcentaje { get; set; }

        [BsonElement("ultimaActualizacion")]
        public DateTime UltimaActualizacion { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } = string.Empty;
    }
}