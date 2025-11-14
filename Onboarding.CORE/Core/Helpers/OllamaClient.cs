using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Onboarding.CORE.Helpers
{
    public class OllamaClient
    {
        private readonly HttpClient _httpClient;

        public OllamaClient()
        {
            // 🧩 Conecta con el servidor local de Ollama
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:11434/")
            };
        }

        /// <summary>
        /// Genera una respuesta del modelo de IA local ejecutado en Ollama.
        /// Por defecto usa el modelo "tinyllama" (ligero y rápido).
        /// </summary>
        /// <param name="prompt">Texto del usuario o instrucción</param>
        /// <param name="modelo">Nombre del modelo a usar (por defecto: tinyllama)</param>
        /// <returns>Texto generado por la IA</returns>
        public async Task<string> GenerarRespuestaAsync(string prompt, string modelo = "tinyllama")
        {
            try
            {
                // 🔹 Construir cuerpo del request
                var body = new
                {
                    model = modelo,
                    prompt = prompt
                };

                // 🔹 Enviar solicitud a Ollama
                var response = await _httpClient.PostAsJsonAsync("api/generate", body);
                var raw = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return $"⚠️ Error al generar respuesta ({response.StatusCode}): {raw}";

                // 🔹 Procesar línea por línea para manejar streams o JSON parcial
                var sb = new StringBuilder();
                var lineas = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var linea in lineas)
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(linea);
                        if (doc.RootElement.TryGetProperty("response", out var resp))
                            sb.Append(resp.GetString());
                    }
                    catch
                    {
                        // Si no es JSON válido, agrega la línea tal cual
                        sb.Append(linea);
                    }
                }

                var final = sb.ToString().Trim();
                if (string.IsNullOrEmpty(final))
                    return "⚠️ No se recibió respuesta del modelo.";

                // 🧠 Mostrar también por consola (para debug)
                Console.WriteLine($"[Ollama ✅] Respuesta generada:\n{final}\n");

                return final;
            }
            catch (Exception ex)
            {
                return $"❌ Error en la conexión con Ollama: {ex.Message}";
            }
        }
    }
}
