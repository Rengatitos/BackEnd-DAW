using MongoDB.Bson.Serialization.Attributes;

namespace Onboarding.CORE.Core.Entities
{
    public class EstadoOnboardingIA
    {
        [BsonElement("pasoActual")]
        public string PasoActual { get; set; } = string.Empty;

        [BsonElement("haVistoDocumentos")]
        public bool HaVistoDocumentos { get; set; }

        [BsonElement("haConsultadoSupervisor")]
        public bool HaConsultadoSupervisor { get; set; }

        [BsonElement("haSolicitadoAdmin")]
        public bool HaSolicitadoAdmin { get; set; }
    }
}