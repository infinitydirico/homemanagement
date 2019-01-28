using System;
using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Models
{
    public class AccountModel
    {
        private double balance;

        public AccountModel() { }

        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public double Balance
        {
            get => balance;
            set
            {
                balance = Math.Round(value, 2);
            }
        }

        public bool ExcludeFromStatistics { get; set; }

        public AccountType AccountType { get; set; }

        public int CurrencyId { get; set; }

        public CurrencyModel Currency { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public enum AccountType
    {
        Cash,
        Bank,
        CreditCard
    }
}
