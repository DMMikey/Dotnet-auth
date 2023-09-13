using System;
namespace BussinessLayer.Abstract
{
    public interface IGenericServices<T> where T : class
    {
        void TRegister(T t);

        T TLogin(string Username, string Password);

        List<T> TListUsers();

        T TGetUserByID(string Username);

        void TDeleteUser(T t);

        void TChangePassword(string Username, string Password, string newPassword);

        void TChangeUsername(string Username, string Password, string newUsername);
    }
}

