using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMSmax.Models;

namespace RMSmax.Data
{
    public class RMSContext : DbContext
    {
        public RMSContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
        
       
    }
}
