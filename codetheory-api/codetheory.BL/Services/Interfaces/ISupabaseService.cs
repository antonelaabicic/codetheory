using Microsoft.AspNetCore.Http;

namespace codetheory.BL.Services.Interfaces
{
    public interface ISupabaseService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task DeleteImageAsync(string imageUrl);
    }
}
