using Microsoft.AspNetCore.Authorization;
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
    public class MembersController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;

        public MembersController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }

        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("login")]
        public IActionResult Authentication([FromQuery] UserCredential userCredential)
        {
            var token = jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return BadRequest(new TaskViewModel("error", new { username = "Поле является обязательным для заполнения", password = "Неверный логин или пароль" }));
            return Ok(new TaskViewModel("ok", token));
        }


    }
}
