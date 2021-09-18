using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class Message
    {
        public Message(IEnumerable<Task> tasks, int total_Task_Count)
        {
            Tasks = tasks;
            Total_Task_Count = total_Task_Count;
        }

        public IEnumerable<Task> Tasks { get; set; }
        public int Total_Task_Count { get; set; }
    }
}
