using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Models;
using File = Domain.Models.File;

namespace DataAccess.DataContext
{
    public class FileDbContext : IdentityDbContext<IdentityUser>
    {

        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {
        }

        public DbSet<File> Files { get; set; }
    }
}
