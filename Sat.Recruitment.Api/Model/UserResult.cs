using System;

namespace Sat.Recruitment.Api.Model
{
    [Serializable]
    public class UserResult
    {
       
        public string ErrorDescription { get; set; }
        public bool IsSuccess { get; set; }
        public string SuccessMsg { get; set; }

    }
}
