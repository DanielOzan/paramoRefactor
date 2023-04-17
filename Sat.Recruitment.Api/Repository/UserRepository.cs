using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sat.Recruitment.Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Xml.Linq;

namespace Sat.Recruitment.Api.Repository
{
    
    public class UserRepository:IUserRepository
    {
        readonly string _path;
        readonly ILogger<UserRepository> _logger;
        public UserRepository(ILogger<UserRepository> logger)
        {
            _path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            _logger = logger;
        }

        public List<UserModel> GetUsers()
        {
            List<UserModel> users= new List<UserModel>();
            JArray jarray= new JArray();
            using StreamReader reader = new StreamReader(_path);
            var json = reader.ReadToEnd();
            if (!string.IsNullOrEmpty(json))
            {
                jarray = JArray.Parse(json);
                _logger.LogInformation("User File is empty");
            }
            if (jarray.Count() > 0)
               users = JsonConvert.DeserializeObject<List<UserModel>>(json);

            return users;
        }
        public UserResult AddUser(UserModel newUser)
        {
            
            List<UserModel> usersList =  GetUsers();
            try
            {
                if (isDuplicate(usersList, newUser))
                {
                    _logger.LogError("User already exists, duplicated user found");
                    return new UserResult { IsSuccess = false, ErrorDescription = "The user is duplicated" };

                }

                usersList.Add(newUser);
                string json = JsonConvert.SerializeObject(usersList.ToArray());

                System.IO.File.WriteAllText(_path, json);

                return new UserResult { IsSuccess = true, ErrorDescription = null, SuccesMsg = "User Created" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResult { IsSuccess = false, ErrorDescription = "Add User Fail" };
            }
        }

        public bool isDuplicate(List<UserModel>usersList, UserModel newEntry)
        {
            var dupFound = usersList.Where(x => (x.Phone == newEntry.Phone || x.Email == newEntry.Email) || (x.Name == newEntry.Name && x.Address == newEntry.Address));
            if (dupFound.Count()>0)
             return true;
            else
                return false;
        }

    }
}
