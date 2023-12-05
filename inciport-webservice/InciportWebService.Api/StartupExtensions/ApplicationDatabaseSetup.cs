using InciportWebService.Application;
using InciportWebService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InciportWebService.Api {

  public static class ApplicationDatabaseSetup {

    public static void AddCustomDbContext(this IServiceCollection services, string connectionString) {
      services.AddDbContext<ApplicationDbContext>(options => {
        options.UseLazyLoadingProxies().UseNpgsql(connectionString);
        //options.LogTo(logMessage => Debug.WriteLine(logMessage)); // Log queries to console
      });
    }

    public static void AddCustomDbContextInterface(this IServiceCollection services, string connectionString) {
      services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => {
        options.UseLazyLoadingProxies().UseNpgsql(connectionString);
        //options.LogTo(logMessage => Debug.WriteLine(logMessage)); // Log queries to console
      });
    }
  }
}