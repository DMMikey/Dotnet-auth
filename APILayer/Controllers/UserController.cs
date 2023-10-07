using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using BussinessLayer.Abstract;
using DTOLayer.DTOs.User;
using EntityLayer.Entites;
using JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using APILayer.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APILayer.Controllers
{
    [Route("api/auth")]
    public class UserController : Controller
    {

        private readonly IUserServices _userservices;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserController(IUserServices userservices, IMapper mapper, IConfiguration config)
        {
            _userservices = userservices;
            _mapper = mapper;
            _config = config;
        }
        
        [HttpPost("Register")]
        public IActionResult Register([FromBody] User user)
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
                return StatusCode(500);
            }


        }
        [HttpGet("getusers")]
        [Authorize()]
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
        public IActionResult Login([FromBody] LoginRequestObject loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest();
            }
            var User = _userservices.TLogin(loginModel.Username, loginModel.Password);
            if (User == null)
            {

                return BadRequest(error: "Invalid username or password.");
            }
            var token = GenerateJwtToken(loginModel.Username);

            return Ok(new { token });

        }

        [HttpGet("getByUsername")]
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
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult DeleteUser(string token)
        {
            try
            {
                var jwtdecoder = new JwtDecoder(_config.GetSection("Jwt:Secret").Value);
                var claimsPrincipal = jwtdecoder.DecodeJwtToken(token);
                var Username = claimsPrincipal.FindFirst(ClaimTypes.Name);
                if (Username == null)
                {
                    return BadRequest("Username Need");
                }
                var user = _userservices.TGetUserByID(Username.Value);
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
        [HttpGet]
        [Authorize]
        public IActionResult ValidUser([FromHeader] string token)
        {
            try
            {
                if (token == null)
                {
                    return StatusCode(404);
                }
                var jwtdecoder = new JwtDecoder(_config.GetSection("Jwt:Secret").Value);
                var claimsPrincipal = jwtdecoder.DecodeJwtToken(token);
                var usernameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
                if (usernameClaim == null)
                {
                    return StatusCode(403);
                }
                var Validuser = _userservices.TGetUserByID(usernameClaim.Value);

                return Ok(Validuser);

            }
            catch
            {
                return StatusCode(500);
            }
        }




        private class JwtDecoder
        {
            private readonly string _secretKey;

            public JwtDecoder(string secretKey)
            {
                _secretKey = secretKey;
            }

            public ClaimsPrincipal DecodeJwtToken(string jwtToken)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // Diğer gerekli validasyon ayarlarını ekleyebilirsiniz
                };

                try
                {
                    var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);
                    return claimsPrincipal;
                }
                catch (Exception ex)
                {
                    throw new Exception("Token decode hatası: " + ex.Message);
                }
            }
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("Jwt:Secret").Value); // Gizli anahtarı bu şekilde alın
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}

