using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AzureBlobStorageExample.Services;
using AzureBlobStorageExample.Options;

namespace AzureBlobStorageExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IBlobService, BlobService>();
            builder.Services.AddScoped(sp =>
            {
                var blobStorage = new BlobStorage();
                builder.Configuration.GetSection("AzureBlobStorage").Bind(blobStorage); 

                return blobStorage; 
            });
            //builder.Services.Configure<BlobStorage>(builder.Configuration.GetSection("AzureBlobStorage"));

            await builder.Build().RunAsync();
        }
    }
}
