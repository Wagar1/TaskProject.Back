using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class TaskDbContext :DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options)
           : base(options) { }

        public DbSet<Task> Tasks { get; set; }
    }
}
