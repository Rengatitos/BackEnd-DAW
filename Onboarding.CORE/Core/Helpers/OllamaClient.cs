using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Onboarding.CORE.Helpers
{
    public class OllamaClient
    {
        private readonly HttpClient _httpClient;

        public OllamaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerarRespuestaAsync(string prompt)
        {
            try
            {
                var json = $@"{{
                    ""model"": ""tinyllama"",
                    ""prompt"": ""{prompt}"",
                    ""stream"": false
                }}";    

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("generate", content);
                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return $"⚠️ Error ({response.StatusCode}): {raw}";

                using var doc = JsonDocument.Parse(raw);

                if (doc.RootElement.TryGetProperty("response", out var resp))
                    return resp.GetString() ?? "⚠️ Respuesta vacía.";

                return "⚠️ No se encontró la propiedad 'response'.";
            }
            catch (Exception ex)
            {
                return $"❌ Error al conectar con Ollama: {ex.Message}";
            }
        }
    }
}
