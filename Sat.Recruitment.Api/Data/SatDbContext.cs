using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Api.Model;
using System.Xml;

namespace Sat.Recruitment.Api.Data
{
    public class SatDbContext : DbContext
    {
        public SatDbContext(DbContextOptions<SatDbContext> options)
        : base(options)
        {
        }

        public DbSet<UserModel> User { get; set; }
    }
        
}
