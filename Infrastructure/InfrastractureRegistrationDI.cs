using Ergo.Application.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastractureRegistrationDI
    {
        public static IServiceCollection AddInfrastrutureToDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ErgoContext>(
                options =>
                options.UseNpgsql(
                    configuration.GetConnectionString
                    ("GlobalTicketsConnection"),
                    builder =>
                    builder.MigrationsAssembly(
                        typeof(ErgoContext)
                        .Assembly.FullName)));
            services.AddScoped
                (typeof(IAsyncRepository<>),
                typeof(BaseRepository<>));
            services.AddScoped<
                IUserRepository, UserRepository>();
            services.AddScoped<
                ICommentRepository, CommentRepository>();
            services.AddScoped<
                ITaskItemRepository, TaskItemRepository>();
            services.AddScoped<
                IProjectRepository, ProjectRepository>();
            return services;
        }
    }
}
