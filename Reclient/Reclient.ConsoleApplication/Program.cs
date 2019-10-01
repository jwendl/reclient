using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reclient.Azure.Clients.Extensions;
using Reclient.Azure.Clients.Models;
using Reclient.Azure.Clients.ResourceGroups;
using System;
using System.Threading.Tasks;

namespace Reclient.ConsoleApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddClientDependencies(configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var resourceGroupRequest = new ResourceGroupRequest()
            {
                Location = "westus",
            };

            var resourceGroupClient = serviceProvider.GetRequiredService<IResourceGroupClient>();
            var resourceGroup = await resourceGroupClient.CreateResourceGroupAsync("27339114-1084-4083-91c5-eecdd911de26", "TestGroup", resourceGroupRequest).ConfigureAwait(false);

            Console.WriteLine($"Created resource group {resourceGroup.Name}");
        }
    }
}
