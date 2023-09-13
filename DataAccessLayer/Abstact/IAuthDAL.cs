using System;
namespace DataAccessLayer.Abstact
{
    public interface IAuthDAL<T> where T : class
    {
        void Register(T t);

        T Login(string Username);

        List<T> ListUsers();

        T GetUserByID(string Username);

        void DeleteUser(T t);

        void ChangePassword(T t, string username, string newPassword);

        void ChangeUsername(T t, string Username, string newUsername);
    }
}

