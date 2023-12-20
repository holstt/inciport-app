using InciportWebService.Application;
using InciportWebService.Domain;
using InciportWebService.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public class Program {
    private const string ENV_VARIABLE_HOST = "HOST";
    private const string ENV_VARIABLE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

    public static async Task Main(string[] args) {
      IHost host = CreateHostBuilder(args).Build(); // ConfigureServices
      host.Run(); // Configure
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configbuilder) =>
                                        configbuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(ENV_VARIABLE_ENVIRONMENT)}.json", optional: false, reloadOnChange: true)
                                                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(ENV_VARIABLE_HOST)}.json", optional: false, reloadOnChange: true))
            .ConfigureWebHostDefaults(webBuilder => {
              webBuilder.UseStartup<Startup>();
            })
            .ConfigureServices(services => {
              // Add archive service as a background service which will be started together with the host.
              services.AddHostedService<ArchiveService>();
            });
  }
}