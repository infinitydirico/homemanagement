﻿namespace HomeManagement.Models
{
    public class AccountModel
    {
        public AccountModel() { }

        public AccountModel(int id, string name, int balance, bool excludeFormStatistics, bool isCash, int userId)
        {
            Id = id;
            Name = name;
            Balance = balance;
            ExcludeFromStatistics = excludeFormStatistics;
            IsCash = isCash;
            UserId = userId;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        public bool ExcludeFromStatistics { get; set; }

        public bool IsCash { get; set; }

        public int UserId { get; set; }
    }
}