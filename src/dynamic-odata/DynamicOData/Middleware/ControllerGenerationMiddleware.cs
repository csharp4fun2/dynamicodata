using DynamicOData.CodeGen;
using DynamicOData.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DynamicOData.Middleware
{
    public class ControllerGenerationMiddleware
    {
        private readonly RequestDelegate requestDel;
        private readonly AnimalDataLoader animalDataLoader;
        private readonly ControllerGenerator controllerGenerator;
        private readonly PartManager partManager;
        private readonly ODataManager oDataManager;

        public ControllerGenerationMiddleware(RequestDelegate requestDel,
            AnimalDataLoader animalDataLoader,
            ControllerGenerator controllerGenerator,
            PartManager partManager,
            ODataManager oDataManager)
        {
            this.requestDel = requestDel;
            this.animalDataLoader = animalDataLoader;
            this.controllerGenerator = controllerGenerator;
            this.partManager = partManager;
            this.oDataManager = oDataManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                var language = parts[0];

                var assembly = controllerGenerator.Generate(language);
                partManager.AddPart(assembly);
                oDataManager.MapODataRoute(assembly, language);
            }
            await requestDel.Invoke(httpContext);
        }
    }
}
