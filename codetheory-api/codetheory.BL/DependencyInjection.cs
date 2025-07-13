using codetheory.BL.Services.Impl;
using codetheory.BL.Services.Interfaces;
using codetheory.DAL.Config;
using codetheory.DAL.Models;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace codetheory.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBlServices(this IServiceCollection services)
        {
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ILessonContentService, LessonContentService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddSingleton<IEncryptionProvider>(_ => EncryptionService.GetProvider());
            services.AddScoped<ISupabaseService, SupabaseService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserAnswerService, UserAnswerService>();
            services.AddScoped<IUserProgressService, UserProgressService>();

            return services;
        }
    }
}
