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

        public void EditTask(Task task)
        {
            var editTask = context.Tasks.First(x => x.ID == task.ID);
            if (!string.IsNullOrEmpty(task.Text))
            {
                editTask.Text = task.Text;
            }
            if(task.Status != 0)
            {
                editTask.Status = task.Status;
            }
            context.SaveChanges();
        }
    }
}
