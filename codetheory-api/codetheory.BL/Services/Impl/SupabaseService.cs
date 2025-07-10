using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Config;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace codetheory.BL.Services.Impl
{
    public class SupabaseService : ISupabaseService
    {
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return ConfigManager.DefaultImagePath;
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var uploadUrl = ConfigManager.SupabaseUploadUrl(fileName);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ConfigManager.SupabaseServiceRoleKey);

            using var content = new StreamContent(file.OpenReadStream());
            content.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            var response = await httpClient.PutAsync(uploadUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Upload failed: {error}");
            }

            return ConfigManager.SupabasePublicUrl(fileName);
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl) || imageUrl.Contains("neutral_profile.png"))
            {
                return;
            }

            var basePublicUrl = $"{ConfigManager.SupabasePublicBaseUrl}/";
            var fileName = imageUrl.Replace(basePublicUrl, "");

            var deleteUrl = ConfigManager.SupabaseDeleteUrl(fileName);

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", ConfigManager.SupabaseServiceRoleKey);

            await httpClient.DeleteAsync(deleteUrl);
        }
    }
}
