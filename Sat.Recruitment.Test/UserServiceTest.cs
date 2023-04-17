using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using System.Collections.Generic;
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
            IConfiguration _configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string>
           {
               ["fileStoragePath"] = "/Files/UsersTest.txt",
               ["testingFlow"] = "true"
           })
           .Build();
            //  inyection of services for test
            var services = new ServiceCollection();

            services.AddTransient<IUserRepository, UserRepository>();

            var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(_configuration)
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
            UserDto newUser = new UserDto
            {
                Name = "Pablo",
                Email = "isNotEmailFormat",
                Address = "Some Addresss 123",
                Phone = "3454-2334",
                UserType = "Normal",
                Money = "34"
            };

            var result = userService.CreateUser(newUser);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void userService_CreateUser_fails_on_parsingMoney()
        {
            var userService = new UserService(_repo, _logger);
            UserDto newUser = new UserDto
            {
                Name = "Pablo2",
                Email = "isNotEmailFormat",
                Address = "Some Addresss 1233",
                Phone = "3454-2334",
                UserType = "Normal",
                Money = "asdf"
            };

            var result = userService.CreateUser(newUser);
            Assert.False(result.IsSuccess);
        }
    }
}
