using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;

namespace Udemy.BankApp.Web.Data.UnitOfWork
{
    public interface IUow
    {
        //Repositorylerimiz hepsi aynı contextte çalışacak. (Unit of work örneği)
        IRepository<T> GetRepository<T>() where T : class, new();
        void SaveChanges();
    }
}