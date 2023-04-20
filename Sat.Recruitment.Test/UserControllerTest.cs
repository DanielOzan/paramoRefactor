using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using Xunit;
using Xunit.Extensions.Ordering;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Sat.Recruitment.Test
{

    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerTest
    {

        private readonly ILogger<UsersController> _logger;

        private readonly IUserService _service;
        private readonly IConfiguration _configuration;


        public UserControllerTest()
        {

            //  inyection of services for test
            //Set COnfiguration
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["fileStoragePath"] = "/Files/UsersTest.txt",
                    ["testingFlow"] = "true"
                })
                .Build();
            _configuration = configuration;
            //set services dependency and logger
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddTransient<IUserService, UserService>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<UsersController>();
            _logger = logger;
            _service = serviceProvider.GetService<IUserService>();

        }

        [Fact, Order(1)]
        public void userController_CreateUser_succeed_on_entry()
        {
            //Arrange
            var userController = new UsersController(_service, _logger);
            UserDto newUser = new UserDto
            {
                Name = "Test",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124",
                Password="admin123",
                
            };

            //Act
            var result = userController.CreateUser(newUser).Result;

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            UserResult userResult = (okObjectResult.Value as UserResult);
            Assert.True(userResult.IsSuccess);
            Assert.Equal("User Created", userResult.SuccessMsg);
        }
        //Correlative to the [Fact,Order(1)] userController_CreateUser_succeed_on_entry()
        [Fact, Order(2)]
        public void userController_CreateUser_fails_on_duplicate_entry()
        {
            //Arrange
            var userController = new UsersController(_service, _logger);
            UserDto newUser = new UserDto
            {
                Name = "Test",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124",
                Password = "admin123",
            };

            //Act
            var result = userController.CreateUser(newUser).Result;
            var BadReqResult = result as BadRequestObjectResult;


            //Assert
            Assert.NotNull(BadReqResult);
            UserResult userResult = (BadReqResult.Value as UserResult);
            Assert.False(userResult.IsSuccess);
            Assert.Contains("The user is duplicated", userResult.ErrorDescription);
        }


        [Fact, Order(3)]
        public void userController_CreateUser_Fails_on_wrong_input_entry()
        {
            //Arrange
            var userController = new UsersController(_service, _logger);
            UserDto newUser = new UserDto
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "ff",
                Password = "admin123",
            };
            //Act

            var result = userController.CreateUser(newUser).Result;
            var BadReqResult = result as BadRequestObjectResult;

            //Assert
            Assert.NotNull(BadReqResult);
            UserResult userResult = (BadReqResult.Value as UserResult);
            Assert.False(userResult.IsSuccess);
            Assert.Contains("The money is empty or incorrect", userResult.ErrorDescription);

        }
        [Fact, Order(4)]
        public void userController_CreateUser_Fails_on_nulls_input_entry()
        {
            //Arrange
            var userController = new UsersController(_service, _logger);
            UserDto newUser = new UserDto
            {
                Name = "",
                Email = "",
                Address = "Av. Juan G",
                Phone = "",
                UserType = "Normal",
                Money = "ff",
                Password = "admin123",
            };
            //Act
            var result = userController.CreateUser(newUser).Result;
            //Assert
            var BadReqResult = result as BadRequestObjectResult;
            Assert.NotNull(BadReqResult);
            UserResult userResult = (BadReqResult.Value as UserResult);

            Assert.False(userResult.IsSuccess);
            Assert.Contains("The money is empty or incorrect", userResult.ErrorDescription);
            Assert.Contains("The name is required", userResult.ErrorDescription);
            Assert.Contains("The phone is required", userResult.ErrorDescription);
            Assert.Contains("The email is empty or incorrect", userResult.ErrorDescription);

        }
        [Fact, Order(5)]
        public void userController_GetUsers_getSomeSucceed()
        {
            // Arrange
            var userController = new UsersController(_service, _logger);
            //Act
            var result = userController.GetUsers().Result;
            //Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            List<UserDto> userResult = (okObjectResult.Value as List<UserDto>);
            Assert.True(userResult.Count > 0);
        }


        [Fact, Order(6)]
        public void RemoveUserTestFile()
        {
            // Remove Test Storage file after testing
            //Asserts
            Assert.True(TestFileHelper.RemoveTestFile(_configuration));

        }

        [Fact, Order(7)]
        public void userController_GetUsers_getZeroSucceed()
        {
            // Arrange
            var userController = new UsersController(_service, _logger);
            //Act
            var result = userController.GetUsers().Result;
            //Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            List<UserDto> userResult = (okObjectResult.Value as List<UserDto>);
            Assert.True(userResult.Count == 0);
        }


    }
}
