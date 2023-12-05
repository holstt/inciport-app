using InciportWebService.Application;
using InciportWebService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace InciportWebService.Api {

  public class Startup {

    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      Console.WriteLine("Configuring services...");
      // Add DAO to services container
      services.AddCustomDbContextInterface(Configuration.GetConnectionString("Default"));
      services.AddCustomDbContext(Configuration.GetConnectionString("Default")); // To make Identity (authencitation) happy (expects the concrete implementation!)

      services.AddControllers(options => options.Filters.Add<ApplicationExceptionFilter>());

      // Add CORS. Allow any
      services.AddCors(options => {
        options.AddDefaultPolicy(
            builder => {
              builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowedToAllowWildcardSubdomains();
            });
      });

      services.AddApplicationAuthorization(Configuration);

      services.AddApplicationSwaggerConfig();

      // Register application services.
      services.AddScoped<AuthencicationService>();
      services.AddScoped<ICategoriesService, CategoriesService>();
      services.AddScoped<IImageService, ImagesService>();
      services.AddScoped<IIncidentReportService, IncidentReportService>();
      services.AddScoped<IWorkerTeamsService, WorkerTeamsService>();
      services.AddScoped<IUserService, UserService>();
      services.AddScoped<IdentityService>();
      services.AddScoped<ApplicationAuthorizationService>();
      services.AddScoped<IImageService, ImagesService>();
      services.AddScoped<IImageSaver, ImageSaver>();
      services.AddScoped<IFileService, FileService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services) {
      Console.WriteLine("Configuring app...");

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      if (ShouldCreateDefaultDb()) {
        CreateDefaultDb(services);
      }
      else if (ShouldCreateTestDb()) {
        CreateTestDb(services);
      }

      if (ShouldPublishSwaggerUI()) {
        app.UseCustomSwaggerUI(services);
      }

      app.UseCors();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }

    private bool ShouldPublishSwaggerUI() => Configuration.GetValue<bool>("ShouldPublishSwaggerUI");

    private bool ShouldCreateTestDb() => Configuration.GetValue<bool>("DbInitialization:ShouldCreateTestDb");

    private bool ShouldCreateDefaultDb() => Configuration.GetValue<bool>("DbInitialization:ShouldCreateDefaultDb");

    // Creates a database with test data for use in development/staging.
    private void CreateTestDb(IServiceProvider services) {
      ApplicationDbContext dbContext = services.GetRequiredService<ApplicationDbContext>();
      DbTestDataInitializer initializer = new DbTestDataInitializer(dbContext, services, Configuration);
      initializer.InitializeAsync().GetAwaiter().GetResult();
    }

    private void CreateDefaultDb(IServiceProvider services) {
      ApplicationDbContext dbContext = services.GetRequiredService<ApplicationDbContext>();
      DbDefaultInitializer initializer = new DbDefaultInitializer(dbContext, services, Configuration);
      initializer.InitializeAsync().GetAwaiter().GetResult();
    }
  }
}