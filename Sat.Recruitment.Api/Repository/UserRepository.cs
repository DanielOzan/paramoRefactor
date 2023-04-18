﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sat.Recruitment.Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sat.Recruitment.Api.Repository
{

    public class UserRepository:IUserRepository
    {
        readonly string _path;
        readonly ILogger<UserRepository> _logger;
        readonly IConfiguration _configuration;
        public UserRepository(ILogger<UserRepository> logger,IConfiguration conf)
        {

            _logger = logger;
            _configuration = conf;
            string storageFilePath = GetPath(conf);
            _path = storageFilePath;
        }

        public List<UserModel> GetUsers()
        {
            List<UserModel> users= new List<UserModel>();
            JArray jarray= new JArray();
            if (File.Exists(_path))
            {
                using StreamReader reader = new StreamReader(_path);
                var json = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(json))
                {
                    jarray = JArray.Parse(json);
                    _logger.LogInformation("User File is empty");
                }
                if (jarray.Count() > 0)
                    users = JsonConvert.DeserializeObject<List<UserModel>>(json);
            }
            else
            { _logger.LogInformation("User File doesn't exists"); }
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

        private bool isDuplicate(List<UserModel>usersList, UserModel newEntry)
        {
            var dupFound = usersList.Where(x => (x.Phone == newEntry.Phone || x.Email == newEntry.Email) || (x.Name == newEntry.Name && x.Address == newEntry.Address));
            if (dupFound.Count()>0)
             return true;
            else
                return false;
        }
        //private string GetPath(IConfiguration conf)
        //{

        //    if (conf.GetValue<string>("testingFlow") == "true")
        //    {

        //        return Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName) + conf.GetValue<string>("fileStoragePath");
        //    }
        //    else
        //    {
        //        return Directory.GetCurrentDirectory() + conf.GetValue<string>("fileStoragePath");
        //    }
        //}
        private string GetPath(IConfiguration conf)
        {
       
            {
                return Directory.GetCurrentDirectory() + conf.GetValue<string>("fileStoragePath");
            }
        }


    }
}
