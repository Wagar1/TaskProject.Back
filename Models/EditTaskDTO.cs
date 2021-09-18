using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApp.Models
{
    public class EditTaskDTO
    {
        [Required]
        public string Token { get; set; }
        public string Text { get; set; }
        public int? Status { get; set; }
    }
}
