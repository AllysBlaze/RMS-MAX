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
        //seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().HasData(new Article
            {
                Id = 1,
                Title = "Title1",
                Content = "Content1",
            },
            new Article
            {
                Id=2,
                Title = "Title2",
                Content = "Content2",
            }) ;

        }
    }
}
