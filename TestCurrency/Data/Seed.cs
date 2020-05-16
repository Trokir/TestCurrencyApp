using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TestCurrency.Core;
using TestCurrency.Models;

namespace TestCurrency.Data
{
    public class Seed
    {
        public static void SeedUsers(DataContext context)
        {
            if (!context.Users.Any())
            {

                var users = new List<User>();
                var fUser = new User {Name = "John"};
                fUser.Currencies = new List<Currency>
                {
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Eur,
                        Count = 10203,
                        User = fUser,
                        UserId = fUser.Id
                    },
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Rub,
                        Count = 2222,
                        User = fUser,
                        UserId = fUser.Id
                    },
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Usd,
                        Count = 222432,
                        User = fUser,
                        UserId = fUser.Id
                    }
                };
                users.Add(fUser);

                var sUser = new User {Name = "Piter"};
                sUser.Currencies = new List<Currency>
                {
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Eur,
                        Count = 1053,
                        User = sUser,
                        UserId = sUser.Id
                    },
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Rub,
                        Count = 2322,
                        User = sUser,
                        UserId = sUser.Id
                    },
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Usd,
                        Count = 25532,
                        User = sUser,
                        UserId = sUser.Id
                    }
                };
                users.Add(sUser);
                var tUser = new User {Name = "Sara"};
                tUser.Currencies = new List<Currency>
                {
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Eur,
                        Count = 1053,
                        User = tUser,
                        UserId = tUser.Id
                    },
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Rub,
                        Count = 2322,
                        User = tUser,
                        UserId = tUser.Id
                    },
                    new Currency
                    {
                        TypeOfCurrency = CurrencyType.Usd,
                        Count = 25532,
                        User = tUser,
                        UserId = tUser.Id
                    }
                };
                users.Add(tUser);

               
                foreach (var user in users)
                {
                    user.Name = user.Name.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }
    }
}
