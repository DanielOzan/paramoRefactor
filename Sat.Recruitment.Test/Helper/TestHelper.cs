using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.Test.Helper
{
    public static class TestHelper
    {
        public static ServiceProvider GetTestDependencyServices()
        {

            //  inyection of services for test
            IConfiguration configuration = new ConfigurationBuilder()
                            .AddInMemoryCollection(new Dictionary<string, string>
                                    {
                                       ["fileStoragePath"] = "/Files/UsersTest.txt",
                                       ["testingFlow"] = "true"
                                    }).Build();
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddTransient<IUserService, UserService>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddLogging()
            .BuildServiceProvider();


            return serviceProvider;

        }
    }
}
