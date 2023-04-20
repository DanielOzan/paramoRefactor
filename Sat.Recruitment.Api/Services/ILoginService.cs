using Sat.Recruitment.Api.Model;

namespace Sat.Recruitment.Api.Services
{
    public interface ILoginService
    {
         void CreateDefaultAdmin();
        string Authenticate(UserLogin user);


    }
}