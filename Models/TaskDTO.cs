using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class TaskDTO
    {
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Неверный email")]
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле является обязательным для заполнения")]
        public string Text { get; set; }
    }
}
