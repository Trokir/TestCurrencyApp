using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestCurrency.Data.Interfaces;
using TestCurrency.Models;

namespace TestCurrency.Data.Repos
{
    public class UsersRepository:BaseRepository<User>,IUserRepository
    {
        public UsersRepository(DataContext context) : base(context) { }
    }

}
