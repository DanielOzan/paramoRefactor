using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Data;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using Sat.Recruitment.Test.Helper;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserServiceTest
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly SatDbContext _context;

        public UserServiceTest()
        {

            //  inyection of services for test
            var serviceProvider = TestHelper.GetTestDependencyServices();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<UserService>();
            _logger = logger;
            _repo = serviceProvider.GetService<IUserRepository>();
            _configuration= serviceProvider.GetService<IConfiguration>();
            _context = serviceProvider.GetService<SatDbContext>();
            _logger.LogInformation("Test User Service initializing...");

        }

        [Fact, Order(1)]
        public void userService_NormalizeEmail_succeed()
        {
            //Arrange
            var userService = new UserService(_repo, _logger);
            //Act
            var result = userService.NormalizeEmail("m+   .ike...+@gmail.com");
            Assert.Equal("mike@gmail.com", result);

            result = userService.NormalizeEmail("Ricardo@gmail.com");
            Assert.Equal("Ricardo@gmail.com", result);

            result = userService.NormalizeEmail("Ricardo@gmail.com");
            Assert.Equal("Ricardo@gmail.com", result);

            result = userService.NormalizeEmail("@gmail.com");
            Assert.Equal("@gmail.com", result);
        }
        [Fact, Order(2)]
        public void userService_CreateUser_fails_on_noEmailFormat()
        {
            //Arrange
            var userService = new UserService(_repo, _logger);
            UserDto newUser = new UserDto
            {
                Account="pab",
                Name = "Pablo",
                Email = "isNotEmailFormat",
                Address = "Some Addresss 123",
                Phone = "3454-2334",
                UserType = "Normal",
                Money = "34",
                Password = "admin123",
                Role = "Default"
            };
            //Act
            var result = userService.CreateUser(newUser).Result;
            //Assert
            Assert.False(result.IsSuccess);
        }
        [Fact, Order(3)]
        public void userService_CreateUser_Succeed_on_decimalValueParse()
        {
            //Arrange
            var userService = new UserService(_repo, _logger);
            UserDto newUser = new UserDto
            {
                Account="pab3",
                Name = "Pablo",
                Email = "pabloEmail@yahoo.com",
                Address = "Some Addresss 1233",
                Phone = "3454-2334",
                UserType = "Normal",
                Money = "30.63",
                Password = "admin123",
                Role = "Default"
            };
            //Act
            var result = userService.CreateUser(newUser).Result;
            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact, Order(4)]
        public void userService_CreateUser_fails_on_parsingMoney()
        {
            //Arrange
            var userService = new UserService(_repo, _logger);
            UserDto newUser = new UserDto
            {
                Account = "pab33",
                Name = "Pablo2",
                Email = "isNotEmailFormat",
                Address = "Some Addresss 1233",
                Phone = "3454-2334",
                UserType = "Normal",
                Money = "asdf",
                Password = "admin123",
                Role = "Default"
            };
            //Act
            var result = userService.CreateUser(newUser).Result;
            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact, Order(6)]
        public void Remove_all_Users_fromTestDb_succed() //JOB 
        {
            // Remove Test Storage file after testing
            //Asserts
            Assert.True(TestHelper.removeDbTestData(_context));

        }
    }
}
