using DynamicOData.CodeGen;
using DynamicOData.Data;
using DynamicOData.Middleware;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamicOData
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ControllerGenerator>();
            services.AddSingleton<PartManager>();
            services.AddSingleton<AnimalDataLoader>();

            services.AddSingleton<IActionDescriptorChangeProvider>(DynamicActionDescriptorChangeProvider.Instance);
            services.AddSingleton(DynamicActionDescriptorChangeProvider.Instance);

            services.AddSingleton<RewriteUrlValueTransformer>();
            services.AddSingleton<EntityLoader>();
            services.AddSingleton<ODataManager>();

            return services;
        }
    }
}
