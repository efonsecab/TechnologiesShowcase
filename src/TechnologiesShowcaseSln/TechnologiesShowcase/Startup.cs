using Azure.AI.FormRecognizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PTI.Microservices.Library.Configuration;
using PTI.Microservices.Library.Interceptors;
using PTI.Microservices.Library.Services;
using PTI.Microservices.Library.Services.Specialized;
using System;
using System.Linq;

namespace TechnologiesShowcase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            var ptiMicroServicesPackageKey = Configuration.GetValue<string>("TestRapidAPIKey");
            GlobalPackageConfiguration.RapidApiKey = ptiMicroServicesPackageKey;
            GlobalPackageConfiguration.EnableHttpRequestInformationLog = false;
            var bingConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureBingSearchConfiguration").Get<PTI.Microservices.Library.Configuration.AzureBingSearchConfiguration>();
            services.AddSingleton(bingConfiguration);
            var azureVideoIndexerConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureVideoIndexerConfiguration").Get<PTI.Microservices.Library.Configuration.AzureVideoIndexerConfiguration>();
            services.AddSingleton(azureVideoIndexerConfiguration);
            var microsoftGraphConfiguration = Configuration.GetSection("MicrosoftGraphConfiguration")
                .Get<MicrosoftGraphConfiguration>();
            services.AddSingleton(microsoftGraphConfiguration);
            var azureCustomVisionConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureCustomVisionConfiguration").Get<PTI.Microservices.Library.Configuration.AzureCustomVisionConfiguration>();
            services.AddSingleton(azureCustomVisionConfiguration);
            var azureFaceConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureFaceConfiguration").Get<PTI.Microservices.Library.Configuration.AzureFaceConfiguration>();
            services.AddSingleton(azureFaceConfiguration);
            var azureComputerVisionConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureComputerVisionConfiguration").Get<PTI.Microservices.Library.Configuration.AzureComputerVisionConfiguration>();
            services.AddSingleton(azureComputerVisionConfiguration);
            var azureSpeechConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureSpeechConfiguration").Get<PTI.Microservices.Library.Configuration.AzureSpeechConfiguration>();
            services.AddSingleton(azureSpeechConfiguration);
            var twitterConfiguration =
                Configuration.GetSection("TwitterConfiguration").Get<TwitterConfiguration>();
            services.AddSingleton(twitterConfiguration);
            var azureContentModeratorConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureContentModeratorConfiguration").Get<PTI.Microservices.Library.Configuration.AzureContentModeratorConfiguration>();
            services.AddSingleton(azureContentModeratorConfiguration);
            var azureMapsConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureMapsConfiguration").Get<PTI.Microservices.Library.Configuration.AzureMapsConfiguration>();
            services.AddSingleton(azureMapsConfiguration);
            var azureFormsRecognizerConfiguration =
                Configuration.GetSection("AzureConfiguration:AzureFormsRecognizerConfiguration");
            services.AddSingleton(azureFormsRecognizerConfiguration);
            services.AddHttpClient();
            services.AddTransient<CustomHttpClientHandler>();
            services.AddTransient<CustomHttpClient>();
            services.AddTransient<AzureBingSearchService>();
            services.AddTransient<AzureVideoIndexerService>();
            services.AddTransient<ImagesService>();
            services.AddTransient<AzureCustomVisionService>();
            services.AddTransient<MicrosoftGraphService>();
            services.AddTransient<AzureFaceService>();
            services.AddTransient<AzureComputerVisionService>();
            services.AddTransient<AzureSpeechService>();
            services.AddTransient<TwitterService>();
            services.AddTransient<AzureContentModeratorService>();
            services.AddTransient<AzureMapsService>();
            services.AddTransient<AudibleWeatherService>();
            services.AddTransient<AudibleComputerVisionService>();
            services.AddTransient<AudibleTwitterService>();
            services.AddTransient<FormRecognizerClient>(sp=>new FormRecognizerClient(
                new Uri(azureFormsRecognizerConfiguration["Endpoint"]),
                new Azure.AzureKeyCredential(azureFormsRecognizerConfiguration["Key"])) 
            { 
            });
            //services.AddSingleton<ILogger<FilteringMessage>, MessagePersistenceLogger>();
            //services.AddSingleton<IImageStreamRetriever, ImageStreamRetriever>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            //services.AddHostedService<EmergencyAlertsJob>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
