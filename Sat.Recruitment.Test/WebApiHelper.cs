using Castle.Core.Resource;
using Sat.Recruitment.Api.Dto;
using Sat.Recruitment.Api.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace Sat.Recruitment.Test
{
    public  class WebApiHelper

    {
        private string url = "";
        HttpClient client = new HttpClient();
        public WebApiHelper(string url)
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
