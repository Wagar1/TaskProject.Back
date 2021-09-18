using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models.ViewModels
{
    public class TaskViewModel
    {
        public TaskViewModel(string status, dynamic message)
        {
            Status = status;
            Message = message;
        }

        public string Status { get; set; }
        public dynamic Message { get; set; }
    }
}
