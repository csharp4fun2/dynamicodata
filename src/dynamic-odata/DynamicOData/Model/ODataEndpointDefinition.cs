using System.Collections.Generic;

namespace DynamicOData.Model
{
    public class ODataEndpointDefinition
    {
        public IList<ModelDefinition> Entities { get; } = new List<ModelDefinition>();
    }

    public class ModelDefinition
    {
        public string Name { get; set; }

        public IList<PropertyDefinition> Properties { get; } = new List<PropertyDefinition>();
    }

    public class PropertyDefinition
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
