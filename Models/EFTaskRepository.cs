using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class EFTaskRepository : ITaskRepository
    {
        private TaskDbContext context;

        public EFTaskRepository(TaskDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Task> Tasks => context.Tasks;

        public void CreateTask(Task task)
        {
            context.Add(task);
            context.SaveChanges();
        }

        public void DeleteTask(Task task)
        {
            context.Remove(task);
            context.SaveChanges();
        }

        public void SaveTask(Task task)
        {
            context.SaveChanges();
        }
    }
}
