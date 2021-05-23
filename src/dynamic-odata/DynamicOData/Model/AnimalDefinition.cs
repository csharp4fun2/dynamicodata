using System.Collections.Generic;
using System.Linq;

namespace DynamicOData.Model
{
    public class AnimalDefinition
    {
        public string Name { get; set; }
        public List<LanguageDefinition> Languages { get; set; }
        public List<AnimalProperty> Properties { get; set; }
        public List<DataDefinition> Data { get; set; }
    }

    public class LanguageDefinition
    {
        public string Language { get; set; }
        public string Value { get; set; }
    }

    public class AnimalProperty
    {
        public string Name { get; set; }
        public string TranslatedNames { get; set; }
        public Dictionary<string, string> TranslatedNamesDict => TranslatedNames.Split(';')
            .ToDictionary(s => s.Split("=")[0], s => s.Split("=")[1]);
        public string DataType { get; set; }
        public bool IsKey { get; set; }
    }

    public class DataDefinition
    {
        public List<EntryDefinition> Entries { get; set; } = new List<EntryDefinition>();
    }

    public class EntryDefinition
    {

        public string Property { get; set; }
        public string Value { get; set; }
    }
}
