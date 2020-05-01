using HomeManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.API.Data
{
    public static class CurrencySeed
    {
        public static void SeedCurrencies(WebAppDbContext dbContext)
        {
            if (!dbContext.Currencies.Any())
            {
                dbContext.AddRange(new List<Currency>
                {
                    new Currency
                    {
                        Name = "USD",
                        Value = 1,
                        ChangeStamp = DateTime.Now                        
                    },
                    new Currency
                    {
                        Name = "ARS",
                        Value = 0,
                        ChangeStamp = DateTime.Now
                    },
                    new Currency
                    {
                        Name = "EUR",
                        Value = 0,
                        ChangeStamp = DateTime.Now
                    }
                });

                dbContext.SaveChanges();
            }
        }
    }
}
