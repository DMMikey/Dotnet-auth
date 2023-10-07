using System;
namespace DataAccessLayer.Abstact
{
    public interface IGenericDAL<T> where T : class
    {

        void Create(T t);

        void Delete(T t);

        void Update(T t);

        List<T> List();

        T GetByID(int ID);

    }
}

