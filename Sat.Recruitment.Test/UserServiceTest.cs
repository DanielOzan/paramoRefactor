using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserServiceTest
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repo;
     
    

        public UserServiceTest()
        {

        //  inyection of services for test
        var services = new ServiceCollection();

       
        services.AddTransient<IUserRepository, UserRepository>();

        var serviceProvider = new ServiceCollection()
        .AddTransient<IUserService, UserService>()
        .AddTransient<IUserRepository, UserRepository>()
        .AddLogging()
        .BuildServiceProvider();

        var factory = serviceProvider.GetService<ILoggerFactory>();

        var logger = factory.CreateLogger<UserService>();
        _logger = logger;
        _repo = serviceProvider.GetService<IUserRepository>();

        }

        [Fact]
        public void userService_NormalizeEmail_succeed()
        {
            var userService = new UserService(_repo, _logger);

            var result = userService.NormalizeEmail("m+   .ike...+@gmail.com");
            Assert.Equal("mike@gmail.com", result);

            result = userService.NormalizeEmail("Ricardo@gmail.com");
            Assert.Equal("Ricardo@gmail.com", result);

            result = userService.NormalizeEmail("Ricardo@gmail.com");
            Assert.Equal("Ricardo@gmail.com", result);

            result = userService.NormalizeEmail("@gmail.com");
            Assert.Equal("@gmail.com", result);
        }
        [Fact]
        public void userService_CreateUser_fails_on_noEmailFormat()
        {
            var userService = new UserService(_repo, _logger);

            var result = userService.CreateUser("Pablo","isNotEmailFormat","Some Addresss 123","3454-2334","Normal","34");
            Assert.False( result.IsSuccess);
        }

        [Fact]
        public void userService_CreateUser_fails_on_parsingMoney()
        {
            var userService = new UserService(_repo, _logger);

            var result = userService.CreateUser("Pablo", "isNotEmailFormat", "Some Addresss 123", "3454-2334", "Normal", "asdf");
            Assert.False(result.IsSuccess);
        }
    }
}
