using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        public IActionResult Create([FromForm]TaskDTO taskDto)
        {
            Models.Task newTask = new Models.Task() {  UserName = taskDto.UserName,  Email = taskDto.Email, Text = taskDto.Text, Status = 0 };
            taskRepository.CreateTask(newTask);
            var response = new TaskViewModel("ok", newTask);
            return new ObjectResult(response){ StatusCode = StatusCodes.Status201Created };
        }

        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, [FromForm]EditTaskDTO editTaskDTO)
        {
            bool isInvalid = Auth._isEmptyOrInvalid(editTaskDTO.Token);        

            if (!isInvalid)
            {
                var task = new Models.Task() { ID = id, Status = editTaskDTO.Status.Value, Text = editTaskDTO.Text };
                taskRepository.EditTask(task);
                return Ok(new TaskViewModel("ok"));
            }
            else
            {
                var token = new Dictionary<string, string>();
                token.Add("token", "Токен истёк");
                return BadRequest(new TaskViewModel("error", token));
            }
        }
    }
}
