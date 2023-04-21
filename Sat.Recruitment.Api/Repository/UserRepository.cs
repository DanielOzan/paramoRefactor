using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sat.Recruitment.Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

        public async Task<List<UserModel>> GetUsersAsync()
        {
            List<UserModel> users= new List<UserModel>();
            JArray jarray= new JArray();
            if (File.Exists(_path))
            {
                using (var reader = new StreamReader(_path))
                {
                    var json = await reader.ReadToEndAsync().ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(json))
                    {
                        jarray = JArray.Parse(json);
                        _logger.LogWarning("User File is empty");
                    }
                    if (jarray.Count() > 0)
                        users = JsonConvert.DeserializeObject<List<UserModel>>(json);
                }
        }
            else
            { _logger.LogWarning("User File doesn't exists, it will be created"); }
            return users;
        }
        public async Task<UserResult> AddUserAsync(UserModel newUser)
        {
            
            List<UserModel> usersList = await GetUsersAsync().ConfigureAwait(false);
            try
            {
                if (isDuplicate(usersList, newUser))
                {
                    _logger.LogError("User already exists, duplicated user found");
                    return new UserResult { IsSuccess = false, ErrorDescription = "The user is duplicated" };

                }

                usersList.Add(newUser);
                string json = JsonConvert.SerializeObject(usersList.ToArray());

               await  System.IO.File.WriteAllTextAsync(_path, json).ConfigureAwait(false);

                return new UserResult { IsSuccess = true, ErrorDescription = null, SuccessMsg = "User Created" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new UserResult { IsSuccess = false, ErrorDescription = "Add User Fail" };
            }
        }

        private bool isDuplicate(List<UserModel>usersList, UserModel newEntry)
        {
            var dupFound = usersList.Where(x => (string.Compare(x.Phone,newEntry.Phone,false)==0 || string.Compare(x.Email, newEntry.Email, StringComparison.Ordinal) == 0) || (string.Compare(x.Name, newEntry.Name, StringComparison.Ordinal) == 0 && string.Compare(x.Address, newEntry.Address, StringComparison.Ordinal) == 0));
            if (dupFound.Count()>0)
             return true;
            else
                return false;
        }
        private string GetPath(IConfiguration conf)
        {
       
                return $"{Directory.GetCurrentDirectory()}{conf.GetValue<string>("fileStoragePath")}";
        }

    }
}
