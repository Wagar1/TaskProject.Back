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

        [HttpGet]
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

        [HttpPost("create")]
        public IActionResult Create([FromForm]string username, [FromForm]string email, [FromForm]string text)
        {
            Models.Task newTask = new Models.Task() { ID = Guid.NewGuid(), UserName = username,  Email = email, Text = text, Status = 0 };
            taskRepository.CreateTask(newTask);
            var response = new TaskViewModel("ok", newTask);
            return new ObjectResult(response){ StatusCode = StatusCodes.Status201Created };
        }
    }
}
