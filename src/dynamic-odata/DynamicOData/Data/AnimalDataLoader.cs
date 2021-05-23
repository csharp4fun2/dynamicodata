using DynamicOData.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DynamicOData.Data
{
    public class AnimalDataLoader
    {
        private Dictionary<string, AnimalDefinition> animalData;
        public AnimalDefinition GetAnimalData(string name)
        {
            EnsureDataLoaded();
            return animalData[name];
        }

        public List<AnimalDefinition> GetLocalizedAnimalData(string language)
        {
            EnsureDataLoaded();
            var foundAnimals = animalData.Where(a => a.Value.Languages.Any(
                l => string.Compare(l.Language, language, true) == 0)).Select(pair => pair.Value).ToList();

            return foundAnimals;
        }

        public AnimalDefinition GetLocalizedAnimalData(string language, string animal)
        {
            EnsureDataLoaded();
            var foundAnimal = animalData.FirstOrDefault(a =>
                string.Compare(a.Value.Name, animal, true) == 0 &&
                a.Value.Languages.Any(l => string.Compare(l.Language, language, true) == 0)
                ).Value;

            return foundAnimal;
        }

        private void EnsureDataLoaded()
        {
            if (animalData == null)
            {
                animalData = new Dictionary<string, AnimalDefinition>();
                foreach (var animalDefinitionFile in Directory.GetFiles("Resources", "*.json"))
                {
                    var animalName = Path.GetFileNameWithoutExtension(animalDefinitionFile);
                    var json = File.ReadAllText(animalDefinitionFile);
                    var def = JsonSerializer.Deserialize<AnimalDefinition>(json);
                    animalData.Add(animalName, def);
                }
            }
        }
    }
}
