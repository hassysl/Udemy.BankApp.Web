using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Entities;
using Udemy.BankApp.Web.Data.Interfaces;
using Udemy.BankApp.Web.Data.Repositories;
using Udemy.BankApp.Web.Data.UnitOfWork;
using Udemy.BankApp.Web.Mapping;
using Udemy.BankApp.Web.Models;

namespace Udemy.BankApp.Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IUserMapper _usermapper;
        private readonly IUow _uow;

        public HomeController(IUserMapper usermapper, IUow uow)
        {
            _usermapper = usermapper;
            _uow = uow;
        }

        public IActionResult Index()
        {
            return View(_usermapper.MapToListOfUserList(_uow.GetRepository<ApplicationUser>().GetAll())); 
        }
    }
}
