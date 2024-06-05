using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class DbConn:DbContext
    {
        public DbConn(DbContextOptions<DbConn> options)
          : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }
}
