using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Onboarding.CORE.Entities
{
    public class CatalogoOnboarding
    {
        [BsonId]
        public string Id { get; set; } = "catalogo_onboarding"; // fixed id string

        [BsonElement("etapas")]
        public List<EtapaOnboarding> Etapas { get; set; } = new();
    }
}