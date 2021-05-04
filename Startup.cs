using HUD.Data.Models;
//using HUD.Data.Models.RepairShopr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using HUD.Data.Models.RepairShopr;
using Microsoft.AspNetCore.Components.Authorization;
using HUD.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using HUD.Data;
using Microsoft.EntityFrameworkCore;
using HUD.Data.Models.UserModels; 
using Microsoft.Data.SqlClient;

namespace HUD
{
    public class Startup
    {

        public string ApiKey { get; set; }
        private string _connection = null;
        public string connStr;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //var builder = new SqlConnectionStringBuilder(
            //Configuration.GetConnectionString("DefaultConnection"));
            //builder.Password = Configuration["DbPassword"];
            //_connection = builder.ConnectionString;
            //this.connStr = _connection;

            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); // Configuration.GetConnectionString("DefaultConnection")

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            //services.AddSingleton<ApiConnection>();
            //services.AddBlazorStrap();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSingleton<IConfiguration>(Configuration);

            //services.AddTransient<ApplicationDbContext>();
            services.AddDbContext<ApplicationDbContext>();
            //services.AddScoped(p =>
            //    p.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
            //    .CreateDbContext());

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MyUserClaimsPrincipalFactory>();

            services.AddHttpContextAccessor();

            services.AddOptions();
            services.AddAuthorizationCore();

            services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });

            //services.AddSingleton<ApiConnection>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HUDAPI");
            //    c.RoutePrefix = "/api";
            //});

            //SecretClientOptions options = new SecretClientOptions()
            //{
            //    Retry =
            //    {
            //        Delay= TimeSpan.FromSeconds(2),
            //        MaxDelay = TimeSpan.FromSeconds(16),
            //        MaxRetries = 5,
            //        Mode = RetryMode.Exponential
            //     }
            //};
            //var client = new SecretClient(new Uri("https://HUDvault.vault.azure.net/"), new DefaultAzureCredential(), options);

            //KeyVaultSecret secret = client.GetSecret("HUD-ConnectionStrings--DbPassword");

            //string secretValue = secret.Value;

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapHub<UpdateHub>("/hub/updatehub");
                //endpoints.MapHub<UpdateHub>(UpdateHub.HubUrl);
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
