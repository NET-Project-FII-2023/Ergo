using Ergo.Application.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfrastructureRegistrationDI
    {
        public static IServiceCollection AddInfrastructureToDI(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ErgoContext>(
                options =>
                options.UseNpgsql(
                    configuration.GetConnectionString
                    ("ErgoConnection"),
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
            services.AddScoped<IInboxItemRepository, InboxItemRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            return services;
        }
    }
}
