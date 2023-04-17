using System;
using System.Dynamic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using Sat.Recruitment.Api;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Model;
using Sat.Recruitment.Api.Repository;
using Sat.Recruitment.Api.Services;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerTest
    {

        private readonly ILogger<UsersController> _logger;


        private readonly IUserService _service;
  

        public UserControllerTest()
        {
            //  inyection of services for test
            var services = new ServiceCollection();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();

            var serviceProvider = new ServiceCollection()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddLogging()
            .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<UsersController>();
            _logger = logger;
            _service = serviceProvider.GetService<IUserService>();
            

        }




        [Fact]
        public void userController_CreateUser_succeed_on_entry()
        {
            //Arrange
            var userController = new UsersController(_service,_logger);

            //Act
            var result = userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;

           
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            UserResult userResult = (okObjectResult.Value as UserResult);

            //Assert
            Assert.True(userResult.IsSuccess);
            Assert.Equal("User Created", userResult.SuccesMsg);
        }


        [Fact]
        public void userController_CreateUser_Fails_on_wrong_input_entry()
        {
            //Arrange
            var userController = new UsersController(_service, _logger);
            //Act

            var result = userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "ff").Result;
            var BadReqResult = result as BadRequestObjectResult;
            Assert.NotNull(BadReqResult);
            UserResult userResult = (BadReqResult.Value as UserResult);

            //Assert
            Assert.False(userResult.IsSuccess);
            Assert.Contains("The money is empty or incorrect", userResult.ErrorDescription);

        }
        [Fact]
        public void userController_CreateUser_Fails_on_nulls_input_entry()
        {
            //Arrange
            var userController = new UsersController(_service, _logger);
            //Act
            var result = userController.CreateUser("", "", "Av. Juan G", "", "Normal", "ff").Result;
           
            var BadReqResult = result as BadRequestObjectResult;
            Assert.NotNull(BadReqResult);
            UserResult userResult = (BadReqResult.Value as UserResult);

            Assert.False(userResult.IsSuccess);
            Assert.Contains("The money is empty or incorrect", userResult.ErrorDescription);
            Assert.Contains("The name is required", userResult.ErrorDescription);
            Assert.Contains("The phone is required", userResult.ErrorDescription);
            Assert.Contains("The email is empty or incorrect", userResult.ErrorDescription);

        }
   
    }
}
