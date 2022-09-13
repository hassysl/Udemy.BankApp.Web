﻿using System.Collections.Generic;
using System.Linq;
using Udemy.BankApp.Web.Data.Context;
using Udemy.BankApp.Web.Data.Entities;
using Udemy.BankApp.Web.Data.Interfaces;

namespace Udemy.BankApp.Web.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankContext _context;

        public AccountRepository(BankContext context)
        {
            _context = context;
        }

        public void Create(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public void Remove(Account acc)
        {
            _context.Set<Account>().Remove(acc);
            _context.SaveChanges();
        }
        public List<Account> GetAll()
        {
            return _context.Set<Account>().ToList();
        }

    }
}
