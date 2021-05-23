using DynamicOData.Model;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.Edm;
using System;
using System.Linq;
using System.Reflection;

namespace DynamicOData.Data
{
    public class ODataManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly AnimalDataLoader animalDataLoader;

        public ODataManager(IServiceProvider serviceProvider, AnimalDataLoader animalDataLoader)
        {
            this.serviceProvider = serviceProvider;
            this.animalDataLoader = animalDataLoader;
        }

        public static IEndpointRouteBuilder Builder { get; set; }

        public void MapODataRoute(Assembly assembly, string language)
        {
            var model = GetEdmModel(assembly, language);
            Builder.MapODataRoute(language, language, model);
        }

        private IEdmModel GetEdmModel(Assembly assembly, string language)
        {
            var builder = new ODataConventionModelBuilder(serviceProvider);

            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (typeof(BaseEntity).IsAssignableFrom(type))
                {
                    var entitySet = typeof(ODataConventionModelBuilder).GetMethod("EntitySet");
                    var genericMethod = entitySet.MakeGenericMethod(type);

                    var foundAnimal = animalDataLoader.GetLocalizedAnimalData(language, type.Name);
                    var lang = foundAnimal.Languages.First(l => string.Compare(l.Language, language, true) == 0);
                    var translatedName = lang.Value;
                    genericMethod.Invoke(builder, new object[] { translatedName });
                }
            }

            return builder.GetEdmModel();
        }
    }
}
