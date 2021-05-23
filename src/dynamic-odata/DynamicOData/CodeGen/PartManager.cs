using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Reflection;

namespace DynamicOData.CodeGen
{
    public class PartManager
    {
        private ApplicationPartManager applicationPartManager;

        public PartManager(ApplicationPartManager applicationPartManager)
        {
            this.applicationPartManager = applicationPartManager;
        }

        public void AddPart(Assembly assembly)
        {
            applicationPartManager.ApplicationParts.Add(new AssemblyPart(assembly));

            //Notify change
            DynamicActionDescriptorChangeProvider.Instance.HasChanged = true;
            DynamicActionDescriptorChangeProvider.Instance.TokenSource.Cancel();
        }
    }
}
