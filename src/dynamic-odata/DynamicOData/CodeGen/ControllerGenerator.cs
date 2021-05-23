using DynamicOData.Data;
using DynamicOData.Model;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace DynamicOData.CodeGen
{
    public class ControllerGenerator
    {
        private readonly AnimalDataLoader animalDataLoader;

        public ControllerGenerator(AnimalDataLoader animalDataLoader)
        {
            this.animalDataLoader = animalDataLoader;
        }

        public Assembly Generate(string language)
        {
            var foundAnimals = animalDataLoader.GetLocalizedAnimalData(language);

            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);
            var code = GetCode(foundAnimals, language);
            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(SourceText.From(code), options);
            var outputPath = Path.GetFullPath(language + ".dll");

            if (!File.Exists(outputPath))
            {
                var references = new List<MetadataReference>();
                foreach (var referenced in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (referenced.IsDynamic) continue;
                    references.Add(MetadataReference.CreateFromFile(referenced.Location));
                }

                var compilation = CSharpCompilation.Create(language,
                    new[] { parsedSyntaxTree },
                    references: references,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
                var compilationResult = compilation.Emit(outputPath);

                if (!compilationResult.Success) throw new Exception("Error at compilation");
            }
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(outputPath);
        }

        private static string GetCode(List<AnimalDefinition> allDefs, string language)
        {
            var code = new StringBuilder();

            code.AppendLine($"using DynamicOData.Controllers;");
            code.AppendLine($"using DynamicOData.Data;");
            code.AppendLine($"using DynamicOData.Model;");
            code.AppendLine($"using Microsoft.AspNet.OData;");
            code.AppendLine($"using Microsoft.AspNetCore.Mvc;");
            code.AppendLine($"using System.ComponentModel.DataAnnotations;");
            code.AppendLine($"using System.Runtime.Serialization;");
            code.AppendLine($"");
            code.AppendLine($"namespace DynamicOData.Generated.{language}");
            code.AppendLine($"{{");

            foreach (var def in allDefs)
            {
                var lang = def.Languages.First(pair => string.Compare(pair.Language, language, true) == 0);
                var name = lang.Value;

                code.AppendLine($"    public class {language}_{name}Controller : AnimalBaseController");
                code.AppendLine($"    {{");
                code.AppendLine($"        private const string AnimalName = \"{def.Name}\";");
                code.AppendLine($"");
                code.AppendLine($"        public {language}_{name}Controller(EntityLoader entityLoader) : base(entityLoader)");
                code.AppendLine($"        {{");
                code.AppendLine($"        }}");
                code.AppendLine($"");
                code.AppendLine($"        [EnableQuery]");
                code.AppendLine($"        public IActionResult Get()");
                code.AppendLine($"        {{");
                code.AppendLine($"            return Get<{def.Name}>(AnimalName);");
                code.AppendLine($"        }}");
                code.AppendLine($"    }}");
                code.AppendLine($"");
                code.AppendLine($"    [DataContract]");
                code.AppendLine($"    public class {def.Name} : BaseEntity");
                code.AppendLine($"    {{");
                foreach (var prop in def.Properties)
                {
                    if (prop.IsKey)
                    {
                        code.AppendLine($"        [Key]");
                    }
                    code.AppendLine($"        [DataMember(Name = \"{prop.TranslatedNamesDict[language]}\")]");
                    code.AppendLine($"        public {prop.DataType} {prop.Name} {{ get; set; }}");
                }
                code.AppendLine($"    }}");
            }
            code.AppendLine($"}}");

            var codeString = code.ToString();
            return codeString;
        }
    }
}
