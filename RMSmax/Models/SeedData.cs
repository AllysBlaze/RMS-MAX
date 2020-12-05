using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMSmax.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace RMSmax.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            RMSContext context = app.ApplicationServices.GetRequiredService<RMSContext>();
            context.Database.Migrate();
            if(!context.Articles.Any())
            {
                context.Articles.AddRange(
                    new Article
                    {
                        Id = 1,
                        Title = "title1",
                        Content="content1",
                    },
                    new Article 
                    {
                        Id = 2,
                        Title = "title2",
                        Content = "content2",
                    },
                    new Article
                    {
                        Id = 3,
                        Title = "title3",
                        Content = "content3",
                    },
                    new Article
                    {
                        Id = 4,
                        Title = "title3",
                        Content = "content3",
                    },
                    new Article
                    {
                        Id = 5,
                        Title = "title3",
                        Content = "content3",
                    },
                    new Article
                    {
                        Id = 6,
                        Title = "title4",
                        Content = "content4",
                    }
                    );

                

            }
            context.SaveChanges();
        }
    }
}
