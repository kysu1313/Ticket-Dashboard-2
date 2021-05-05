using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using HUD.Data.Models.UserModels;
using System.Security.Cryptography.X509Certificates;

namespace HUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration(builder =>
                //{
                //    var root = builder.Build();
                //    var vaultName = root["KeyVaultName"];
                //    builder.AddAzureKeyVault($"https://{vaultName}.vault.azure.net/",
                //        root["AzureADApplicationId"], GetCertificate(root["AzureADCertThumbprint"]),
                //        new PrefixKeyVaultSecretManager("HUD"));
                //})
                .ConfigureAppConfiguration((context, config) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }
                })
                //Host.CreateDefaultBuilder(args)
                //    .ConfigureAppConfiguration((context, config) =>
                //    {
                //if (context.HostingEnvironment.IsDevelopment())
                //{
                //    config.AddUserSecrets<Program>();
                //}
                //var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                //config.AddAzureKeyVault(
                //keyVaultEndpoint,
                //new DefaultAzureCredential());
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static X509Certificate2 GetCertificate(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (certificateCollection.Count == 0)
                {
                    throw new Exception("Certificate is not installed");
                }
                return certificateCollection[0];
            }
            finally
            {
                store.Close();
            }
        }

    }
}

// RSApiKey