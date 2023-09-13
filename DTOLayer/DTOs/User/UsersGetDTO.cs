using System;
using System.ComponentModel.DataAnnotations;

namespace DTOLayer.DTOs.User
{
    public class UsersGetDTO
    {
        [Key]
        public string Username { get; set; }

        public string Email { get; set; }

        public long PhoneNumber { get; set; }
    }
}

