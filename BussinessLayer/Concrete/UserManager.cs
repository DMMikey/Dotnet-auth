using System;
using BussinessLayer.Abstract;
using DataAccessLayer.Abstact;
using DataAccessLayer.Concrete;
using EntityLayer.Entites;

namespace BussinessLayer.Concrete
{
    public class UserManager : IUserServices
    {

        private readonly IUserDAL _userDAL;

        public UserManager(IUserDAL userDAL)
        {
            _userDAL = userDAL;
        }

        public void TChangePassword(string Username, string Password, string newPassword)
        {
            var user = _userDAL.GetUserByID(Username);

            if (user != null && user.Password == Password)
            {
                _userDAL.ChangePassword(user, Username, newPassword);
            }
            else
            {
                // Kullanıcı doğrulama başarısız oldu, hata mesajı döndürebilirsiniz.
                throw new InvalidOperationException("Kullanıcı adı veya şifre yanlış.");
            }
        }

        public void TChangeUsername(string Username, string Password, string newUsername)
        {
            var user = _userDAL.GetUserByID(Username);

            if (user != null && user.Password == Password)
            {
                _userDAL.ChangeUsername(user, Username, newUsername);
            }
            else
            {
                // Kullanıcı doğrulama başarısız oldu, hata mesajı döndürebilirsiniz.
                throw new InvalidOperationException("Kullanıcı adı veya şifre yanlış.");
            }
        }

        public void TDeleteUser(User t)
        {
            var user = _userDAL.GetUserByID(t.Username);
            _userDAL.DeleteUser(user);
        }

        public User TGetUserByID(string Username)
        {
            return _userDAL.GetUserByID(Username);
        }

        public List<User> TListUsers()
        {
            return _userDAL.ListUsers();
        }

        public User TLogin(string Username, string Password)
        {
            var userFromDb = _userDAL.Login(Username);
            if (userFromDb.Password == Password)
            {
                return userFromDb;
            }
            return null;
        }

        public void TRegister(User t)
        {
            _userDAL.Register(t);
        }
    }
}

