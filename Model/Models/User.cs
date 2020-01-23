using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Model.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Email]
        public string Email { get; set; }
        
        public string Name { get; set; }
        [Required]
        public string  Password { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


    }
}
