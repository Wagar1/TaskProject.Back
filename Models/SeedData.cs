using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TaskDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<TaskDbContext>>()))
            {
                // Look for any board games.
                if (context.Tasks.Any())
                {
                    return;   // Data was already seeded
                }

                context.Tasks.AddRange(
                     new Task
                     {
                         UserName = "Test User",
                         Email = "test_user_1@example.com",
                         Text = "Hello, world!",
                         Status = 0
                     }
                );

                context.SaveChanges();
            }
        }
    }
}

