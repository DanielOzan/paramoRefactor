using Sat.Recruitment.Api.Dto;
using System.Net.Http;
using System.Text;

namespace Sat.Recruitment.Test
{
    public  class TestWebApiHelper

    {
        private string url = "";
        HttpClient client = new HttpClient();
        public TestWebApiHelper(string url)
        {
            this.url = url;
        }

        public HttpResponseMessage Post(UserDto obj)
        {

            var  json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var content= new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(this.url, content).Result;
           
            return response;
        }


    }
}
