using ErgoNames.Api.Data;
using ErgoNames.Api.Models.Configuration;
using ErgoNames.Api.Security;
using ErgoNames.Api.Services;
using ErgoNames.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Serilog;

namespace ErgoNames.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();

            var nameApiConfiguration = new ErgoNameApiConfiguration();
            new ConfigureFromConfigurationOptions<ErgoNameApiConfiguration>(Configuration.GetSection("ErgoNamesConfiguration"))
                .Configure(nameApiConfiguration);

            services.AddSingleton(nameApiConfiguration);

            var storageConnectionString = new StorageConnectionString
            {
                ConnectionString = Configuration.GetConnectionString("AzureStorage")
            };

            services.AddSingleton(storageConnectionString);

            services.AddTransient<IErgoExplorerClient, ErgoExplorerClient>();
            services.AddTransient<ITableRepository, TableRepository>();
            services.AddTransient<ITokenValidator, TokenValidator>();
            services.AddTransient<ITokenResolver, TokenResolver>();
            

            services
                .AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", options => { });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication").RequireAuthenticatedUser().Build());
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime, IServiceScopeFactory scopeFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            lifetime.ApplicationStarted.Register(x => OnApplicationStarted(scopeFactory), null);
        }

        private void OnApplicationStarted(IServiceScopeFactory scopeFactory)
        {
            Log.Debug("Beginning OnApplicationStarted");

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                ITableRepository repository = scope.ServiceProvider.GetRequiredService<ITableRepository>();
                Log.Debug("Initializing tables");
                repository.InitializeTables().GetAwaiter().GetResult();
            }
        }
    }
}
