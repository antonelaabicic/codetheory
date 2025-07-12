using Microsoft.JSInterop;
using System.Text.Json;
using System.Text;

namespace codetheory.Web.Services
{
    public class JwtService
    {
        private readonly IJSRuntime _js;
        public JwtService(IJSRuntime js)
        {
            _js = js;
        }
        public async Task<string?> GetUsernameAsync()
        {
            var token = await _js.InvokeAsync<string>("sessionStorage.getItem", "jwt");
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("JWT token not found in session storage.");
            }

            var payload = token.Split('.')[1];
            var json = JsonSerializer.Deserialize<JsonElement>(
                Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(payload)))
            );

            return json.TryGetProperty("unique_name", out var u) ? u.GetString() : null;
        }

        private string PadBase64(string input)
        {
            return input.PadRight(input.Length + (4 - input.Length % 4) % 4, '=');
        }
    }
}
