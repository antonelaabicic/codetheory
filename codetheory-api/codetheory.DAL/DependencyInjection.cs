using codetheory.DAL.Repositories.Impl;
using codetheory.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace codetheory.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonContentRepository, LessonContentRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAnswerRepository, AnswerRepository>();

            return services;
        }
    }
}
