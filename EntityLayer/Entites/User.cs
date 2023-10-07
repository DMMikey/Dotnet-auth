using System;
using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Entites
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public long PhoneNumber { get; set; }

        public string ImgURL { get; set; }

        public string Role { get; set; }
    }
}

