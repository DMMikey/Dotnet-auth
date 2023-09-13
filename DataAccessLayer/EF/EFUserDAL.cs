using System;
using DataAccessLayer.Abstact;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Entites;

namespace DataAccessLayer.EF
{
    public class EFUserDAL : AuthRepository<User>, IUserDAL
    {
        public EFUserDAL(ApplicationDbContext context) : base(context)
        {
        }
    }
}

