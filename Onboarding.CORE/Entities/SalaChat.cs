using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Onboarding.CORE.Entities
{
    public class SalaChat
    {
        [BsonId]
        public string Id { get; set; } = string.Empty; // will be 'sala_{usuarioRef}'

        [BsonElement("usuarioRef")]
        public string UsuarioRef { get; set; } = string.Empty;

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("correo")]
        public string Correo { get; set; } = string.Empty;

        [BsonElement("area")]
        public string Area { get; set; } = string.Empty;

        [BsonElement("rolRef")]
        public string RolRef { get; set; } = string.Empty;

        [BsonElement("nivelOnboarding")]
        public NivelOnboarding NivelOnboarding { get; set; } = new();

        [BsonElement("estadoOnboardingIA")]
        public EstadoOnboardingIA EstadoOnboardingIA { get; set; } = new();

        [BsonElement("contextoPersistente")]
        public string ContextoPersistente { get; set; } = string.Empty;

        [BsonElement("ultimoMensaje")]
        public string UltimoMensaje { get; set; } = string.Empty;

        [BsonElement("ultimaActualizacion")]
        public DateTime UltimaActualizacion { get; set; } = DateTime.UtcNow;
    }
}