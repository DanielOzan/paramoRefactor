using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sat.Recruitment.Test
{
    public static class TestFileHelper
    {
        public static bool RemoveTestFile(IConfiguration conf)
        {
            var file = Directory.GetCurrentDirectory() + conf.GetValue<string>("fileStoragePath");
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;


        }
    }
}
