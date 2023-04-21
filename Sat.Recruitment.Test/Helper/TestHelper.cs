using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Data;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sat.Recruitment.Test.Helper
{
    public static class TestHelper
    {
        public static ServiceProvider GetTestDependencyServices()
        {

            //  inyection of services for test
            var configuration = new ConfigurationBuilder()
          .AddJsonFile("appsettings.test.json")
           .AddEnvironmentVariables()
           .Build();
           
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddTransient<IUserService, UserService>()
            .AddTransient<IUserRepository, UserRepository_SqLite>()
            .AddDbContext<SatDbContext>(options =>
            options.UseSqlite(configuration["ConnectionStrings:sqlite"]))
            .AddLogging()
            .BuildServiceProvider();


            return serviceProvider;

        }
        public static bool removeDbTestData(SatDbContext db)
        {

            var userToRemove = db.User.ToList();
            db.User.RemoveRange(userToRemove);
            db.SaveChanges();
            return true;

        }
    }

}
