using Sat.Recruitment.Api.Dto;
using Xunit;

namespace Sat.Recruitment.Test
{
    public class UserIntegrationTest
    {

        //This tests WILL WORK ONLY IF THE MAIN PROYECT IS RUNNING http://localhost:5000/create-user
        //[Fact]
        public void AddUserSuceedOnHttpClient()
        {

            //Arrange
            UserDto newUser = new UserDto
            {
                Name = "Test",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            };

            var url = "http://localhost:5000/create-user";
            TestWebApiHelper helper = new TestWebApiHelper(url);
            //Act
            var response = helper.Post(newUser);

            //Assert
            Assert.Contains("OK", response.Content.ReadAsStringAsync().Result);

        }

        //[Fact]
        public void AddUserFailOnHttpClient()
        {
            //Arrange
            UserDto newUser = new UserDto
            {
                Name = null,
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = "Normal",
                Money = "124"
            };

            var url = "http://localhost:5000/create-user";
            TestWebApiHelper helper = new TestWebApiHelper(url);
            //Act
            var response = helper.Post(newUser);

            //Assert
            var resultObj = response.Content.ReadAsStringAsync().Result;
            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("Name field is required", resultObj);

        }
    }
}
