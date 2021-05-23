using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace DynamicOData.CodeGen
{
    public class DynamicActionDescriptorChangeProvider : IActionDescriptorChangeProvider
    {
        private static DynamicActionDescriptorChangeProvider instance;
        public static DynamicActionDescriptorChangeProvider Instance => instance ??= new DynamicActionDescriptorChangeProvider();

        public CancellationTokenSource TokenSource { get; private set; }
        public bool HasChanged { get; set; }
        public IChangeToken GetChangeToken()
        {
            TokenSource = new CancellationTokenSource();
            return new CancellationChangeToken(TokenSource.Token);
        }
    }
}
