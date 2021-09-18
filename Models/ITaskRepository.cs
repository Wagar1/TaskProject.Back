using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public interface ITaskRepository
    {
        IQueryable<Task> Tasks { get; }
        void CreateTask(Task task);
        void EditTask(Task task);
        void DeleteTask(Task task);
    }
}
