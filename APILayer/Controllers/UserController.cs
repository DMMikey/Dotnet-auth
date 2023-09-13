using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BussinessLayer.Abstract;
using DTOLayer.DTOs.User;
using EntityLayer.Entites;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APILayer.Controllers
{
    [Route("api/auth")]
    public class UserController : Controller
    {

        private readonly IUserServices _userservices;
        private readonly IMapper _mapper;

        public UserController(IUserServices userservices, IMapper mapper)
        {
            _userservices = userservices;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            try
            {
                if (_userservices.TGetUserByID(user.Username) == null)
                {

                    _userservices.TRegister(user);
                    return Ok(user);
                }
                return BadRequest("Every Username Must Be Speacial");
            }
            catch
            {
                return BadRequest("Internal Server Error");
            }


        }

        [HttpGet("Get-Users")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _userservices.TListUsers();
                var DTOUsers = _mapper.Map<List<UsersGetDTO>>(users);
                return Ok(DTOUsers);
            }
            catch
            {
                return BadRequest("Internal server Error");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(string Username, string Password)
        {
            var User = _userservices.TLogin(Username, Password);

            return Ok(User);
        }

        [HttpGet("Get-User-ByUsername")]
        public IActionResult GetByUsername(string Username)
        {
            try
            {

                var user = _userservices.TGetUserByID(Username);
                var DTOUser = _mapper.Map<UsersGetDTO>(user);
                if (User == null)
                {
                    return BadRequest("User Can Not Be Found");
                }
                return Ok(DTOUser);
            }

            catch
            {
                return BadRequest("Internal Server Error");
            }

        }

        [HttpDelete("Delete-users")]
        public IActionResult DeleteUser(string Username)
        {
            try
            {
                if (Username == null)
                {
                    return BadRequest("Username Need");
                }
                var user = _userservices.TGetUserByID(Username);
                if (Username == null)
                {
                    return BadRequest("Username Need");
                }

                _userservices.TDeleteUser(user);
                return Ok(user.Username + "Deleted Successfully");
            }
            catch
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPut("Change-Password")]
        public IActionResult ChangePassword(string Username, string Password, string NewPassword)
        {
            var user = _userservices.TLogin(Username, Password);

            if (user != null)
            {
                // Kullanıcı adı ve mevcut şifre doğruysa şifreyi değiştir
                _userservices.TChangePassword(Username, Password, NewPassword);
                return Ok("Password changed successfully.");
            }
            else
            {
                return BadRequest("Invalid username or password.");
            }
        }
    }
}

