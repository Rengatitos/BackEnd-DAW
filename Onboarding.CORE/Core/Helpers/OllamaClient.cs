using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Onboarding.CORE.Helpers
{
    public class OllamaClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private readonly string _defaultModel = "llama3.2:3b-instruct-q4_K_M";
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(120);

        public OllamaClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> GenerarRespuestaAsync(
            string prompt,
            string? model = null,
            bool stream = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                prompt ??= string.Empty;

                // Normalizar saltos de línea
                prompt = prompt.Replace("\r\n", "\n").Replace("\r", "\n");

                var payload = new Dictionary<string, object?>
                {
                    ["model"] = model ?? _defaultModel,
                    ["prompt"] = prompt,
                    ["stream"] = stream // boolean real, NO string
                };

                var json = JsonSerializer.Serialize(payload, _jsonOptions);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(_defaultTimeout);

                // Llamada POST a Ollama (BaseAddress ya está configurada en Program.cs)
                var response = await _httpClient.PostAsync("api/generate", content, cts.Token);
                var body = await response.Content.ReadAsStringAsync(cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    var safe = (body ?? string.Empty)
                        .Replace("\r", "\\r")
                        .Replace("\n", "\\n");

                    return $"⚠️ Error ({response.StatusCode}): {safe}";
                }

                // Parseo de respuesta JSON
                try
                {
                    using var doc = JsonDocument.Parse(body ?? string.Empty);

                    if (doc.RootElement.TryGetProperty("response", out var resp))
                        return resp.GetString() ?? string.Empty;

                    if (doc.RootElement.TryGetProperty("results", out var results) &&
                        results.ValueKind == JsonValueKind.Array &&
                        results.GetArrayLength() > 0)
                    {
                        var first = results[0];
                        if (first.TryGetProperty("content", out var contentProp))
                            return contentProp.GetString() ?? string.Empty;
                    }

                    return (body ?? string.Empty).Trim();
                }
                catch (JsonException)
                {
                    return (body ?? string.Empty).Trim();
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                return "❌ Timeout al conectar con Ollama.";
            }
            catch (Exception ex)
            {
                return $"❌ Error al conectar con Ollama: {ex.Message}";
            }
        }
    }
}