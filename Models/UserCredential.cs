using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class UserCredential
    {
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Неверный логин или пароль")]
        public string Password { get; set; }
    }
}
