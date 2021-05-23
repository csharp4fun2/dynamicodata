using System;
using System.Collections.Generic;

namespace DynamicOData.Data
{
    public class EntityLoader
    {
        private readonly AnimalDataLoader animalDataLoader;

        public EntityLoader(AnimalDataLoader animalDataLoader)
        {
            this.animalDataLoader = animalDataLoader;
        }

        public List<T> LoadEntities<T>(string animalName)
        {

            var def = animalDataLoader.GetAnimalData(animalName);
            var list = new List<T>();
            foreach (var data in def.Data)
            {
                var animalInstance = Activator.CreateInstance<T>();
                foreach (var entry in data.Entries)
                {
                    var prop = animalInstance.GetType().GetProperty(entry.Property);

                    object value;
                    if (prop.PropertyType == typeof(string))
                    {
                        value = entry.Value;
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        value = double.Parse(entry.Value);
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        value = int.Parse(entry.Value);
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        value = DateTime.Parse(entry.Value);
                    }
                    else
                    {
                        throw new Exception("Unknown datatype");
                    }
                    prop.SetValue(animalInstance, value);
                }

                list.Add(animalInstance);
            }

            return list;
        }
    }
}
