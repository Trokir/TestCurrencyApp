using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCurrency.Models;

namespace TestCurrency.Data.Interfaces
{
   public interface IUserRepository:IAsyncRepository<User>
    {
       
    }
}
