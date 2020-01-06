using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTO
{
    public class RegisterDto
    {
        [Required]
        [Email]

        public string Email { get; set; }


        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
