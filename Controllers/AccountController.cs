using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
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
    public class AccountController : Controller
    {
        //private readonly IApplicationUserRepository _applicationUserRepository;
        //private readonly IUserMapper _userMapper;
        //private readonly IAccountRepository _accountRepository;
        //private readonly IAccountMapper _accountMapper;

        //public AccountController(IUserMapper userMapper, IApplicationUserRepository applicationUserRepository, IAccountRepository accountRepository, IAccountMapper accountMapper)
        //{
        //    _userMapper = userMapper;
        //    _applicationUserRepository = applicationUserRepository;
        //    _accountRepository = accountRepository;
        //    _accountMapper = accountMapper;
        //}




        //private readonly IRepository<Account> _accountRepository;
        //private readonly IRepository<ApplicationUser> _userRepository;

        ////Dependency Injection
        //public AccountController(IRepository<Account> accountRepository, IRepository<ApplicationUser> userRepository)
        //{
        //    _accountRepository = accountRepository;
        //    _userRepository = userRepository;
        //}



        private readonly IUow _uow;

        public AccountController(IUow uow)
        {
            _uow = uow;
        }

        //Create Get 
        public IActionResult Create(int id)
        {
            var userInfo = _uow.GetRepository<ApplicationUser>().GetById(id); //repository
            return View(new UserListModel
            {
                Id = userInfo.Id,
                Name = userInfo.Name,
                Surname = userInfo.Surname
            });
        }

        [HttpPost]
        public IActionResult Create(AccountCreateModel model)
        {
            _uow.GetRepository<Account>().Create(new Account
            {
                AccountNumber = model.AccountNumber,
                Balance = model.Balance,
                ApplicationUserId = model.ApplicationUserId
            });
            _uow.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        //Tüm kullanıcıların full name ve hesap bilgilerini getiren GetUserById Get 
        [HttpGet]
        public IActionResult GetByUserId(int userId)
        {
            //GetQueryable ile query'e linq sorgusu yazılabileceğini anlattık.
            var query = _uow.GetRepository<Account>().GetQueryable();
            //Parametre ile uyuşacak olan tüm hesapları accounts içine liste tipinde gönderdik
            var accounts = query.Where(x => x.ApplicationUserId == userId).ToList();

            var user = _uow.GetRepository<ApplicationUser>().GetById(userId);

            //Accountları tutabileceğimiz bir liste oluşturduk
            var list = new List<AccountListModel>();

            //Full name normalde bizim propertymiz değil. Bu sebeple user içinden hem name'i hem de surname'i viewbage gönderdik ki view'da kullanabilelim.
            ViewBag.FullName = user.Name + " " + user.Surname;

            //Accountaların her birini tüm proplarıyla beraber list adlı değişkene gönderdik ve view'da bu listeyi döndük
            foreach (var item in accounts)
            {
                list.Add(new()
                {
                    AccountNumber = item.AccountNumber,
                    ApplicationUserId = item.ApplicationUserId,
                    Balance = item.Balance,
                    Id = item.Id
                });
            }
            
            return View(list);
        }



        //Para gönderme ekranı Sendmoney Get
        [HttpGet]
        public IActionResult SendMoney(int accountId)
        {
            //Bir hesaptan başka hesaba para gönderirken karşımıza kendi hesabımız hariç diğer bütün hesapları listelemek için aşağıdaki Linq sorgusunu yazdık ve bir list adında değişkene attık. Daha sonra döndürdük.
            var query = _uow.GetRepository<Account>().GetQueryable();

            var accounts = query.Where(x => x.Id != accountId).ToList();
            var list = new List<AccountListModel>();

            ViewBag.SenderId = accountId; //Post'a id göndermek için

            foreach (var account in accounts)
            {
                list.Add(new()
                {
                    AccountNumber = account.AccountNumber,
                    ApplicationUserId= account.ApplicationUserId,
                    Balance= account.Balance,
                    Id= account.Id
                });
            }

            var list2 = new SelectList(list);
            //var items = list2.Items;

            return View(new SelectList(list,"Id","AccountNumber"));
        }

        //SendMoney Post
        [HttpPost]
        public IActionResult SendMoney(SendMoneyModel model)
        {
            var senderAccount = _uow.GetRepository<Account>().GetById(model.SenderId);

            senderAccount.Balance -= model.Amount;

            _uow.GetRepository<Account>().Update(senderAccount);



            var account = _uow.GetRepository<Account>().GetById(model.AccountId);   
            account.Balance += model.Amount;
            _uow.GetRepository<Account>().Update(account);

            _uow.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
