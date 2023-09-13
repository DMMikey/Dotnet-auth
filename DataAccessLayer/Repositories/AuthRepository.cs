using System;
using DataAccessLayer.Abstact;
using DataAccessLayer.Concrete;

namespace DataAccessLayer.Repositories
{
    public class AuthRepository<T> : IAuthDAL<T> where T : class
    {

        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ChangePassword(T t, string username, string newPassword)
        {
            var user = _context.Set<T>().Find(username);

            if (user != null)
            {
                // Password özelliğinin varlığını kontrol edelim
                var passwordProperty = typeof(T).GetProperty("Password");
                if (passwordProperty != null && passwordProperty.PropertyType == typeof(string))
                {
                    // Kullanıcının yeni şifresini atanması
                    passwordProperty.SetValue(user, newPassword);

                    // Değişikliklerin veritabanına kaydedilmesi
                    _context.SaveChanges();
                }
                else
                {
                    // T sınıfının Password özelliği yok veya tipi uygun değilse hata ver
                    throw new InvalidOperationException("T sınıfı geçerli bir Password özelliği içermiyor.");
                }
            }
        }

        public void ChangeUsername(T t, string Username, string newUsername)
        {
            var user = _context.Set<T>().Find(Username);

            if (user != null)
            {
                // Password özelliğinin varlığını kontrol edelim
                var usernameProp = typeof(T).GetProperty("Username");
                if (usernameProp != null && usernameProp.PropertyType == typeof(string))
                {
                    // Kullanıcının yeni şifresini atanması
                    usernameProp.SetValue(user, newUsername);

                    // Değişikliklerin veritabanına kaydedilmesi
                    _context.SaveChanges();
                }
                else
                {
                    // T sınıfının Password özelliği yok veya tipi uygun değilse hata ver
                    throw new InvalidOperationException("T sınıfı geçerli bir Password özelliği içermiyor.");
                }
            }
        }

        public void DeleteUser(T t)
        {
            _context.Remove(t);
            _context.SaveChanges();
        }

        public T GetUserByID(string Username)
        {
            return _context.Set<T>().Find(Username);
        }

        public List<T> ListUsers()
        {
            return _context.Set<T>().ToList();
        }

        public T Login(string Username)
        {
            return _context.Set<T>().Find(Username);
        }

        public void Register(T t)
        {
            _context.Add(t);
            _context.SaveChanges();
        }
    }
}

