using DynamicOData.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;

namespace DynamicOData.Middleware
{
    public class RewriteUrlValueTransformer : DynamicRouteValueTransformer
    {
        private readonly AnimalDataLoader animalDataLoader;

        public RewriteUrlValueTransformer(AnimalDataLoader animalDataLoader)
        {
            this.animalDataLoader = animalDataLoader;
        }

        public override ValueTask<RouteValueDictionary> TransformAsync(
            HttpContext httpContext, RouteValueDictionary values)
        {
            var language = values["controller"] as string;
            var animal = values["method"] as string;

            values["controller"] = $"{language}_{animal}";
            values["action"] = "Get";
            return ValueTask.FromResult(values);
        }
    }
}
