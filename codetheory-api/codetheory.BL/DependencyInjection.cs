using codetheory.BL.Services.Impl;
using codetheory.BL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace codetheory.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBlServices(this IServiceCollection services)
        {
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ILessonContentService, LessonContentService>();

            return services;
        }
    }
}
