using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Models;
using TaskApp.Models.ViewModels;

namespace TaskApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private ITaskRepository taskRepository;

        public TasksController(ITaskRepository repo)
        {
            taskRepository = repo;
        }

        public IActionResult Get([FromQuery] string developer)
        {
            if (string.IsNullOrEmpty(developer))
            {
                var response = new TaskViewModel("error", "Не передано имя разработчика");
                return Ok(response);
            }

            var tasks = new TaskViewModel("ok", new Message(taskRepository.Tasks, taskRepository.Tasks.Count()));
            return Ok(tasks);
        }
    }
}
