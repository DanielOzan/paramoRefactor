using System;

namespace Sat.Recruitment.Api.Model
{
    [Serializable]
    public class UserLogin
    {

        public string account { get; set; }
        public string password { get; set; }
    }
}
