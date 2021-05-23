using DynamicOData.Data;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace DynamicOData.Controllers
{
    public class AnimalBaseController : ODataController
    {
        private readonly EntityLoader entityLoader;

        public AnimalBaseController(EntityLoader entityLoader)
        {
            this.entityLoader = entityLoader;
        }

        protected IActionResult Get<T>(string animalName)
        {
            return Ok(entityLoader.LoadEntities<T>(animalName));
        }
    }
}
