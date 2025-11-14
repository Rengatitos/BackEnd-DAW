using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Onboarding.CORE.Entities
{
    public class InteraccionChat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("usuario_ref")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioRef { get; set; }

        [BsonElement("mensaje_usuario")]
        public string MensajeUsuario { get; set; }

        [BsonElement("respuesta_chatbot")]
        public string RespuestaChatbot { get; set; }

        [BsonElement("fecha_hora")]
        public DateTime FechaHora { get; set; }

        [BsonElement("contexto")]
        public string Contexto { get; set; }

        [BsonElement("respuesta_corregida")]
        public string? RespuestaCorregida { get; set; }

        [BsonElement("es_corregida")]
        public bool EsCorregida { get; set; }
    }
}
