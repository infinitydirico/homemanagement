﻿using HomeManagement.App.Data.Entities;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomeManagement.App.ViewModels
{
    public class AddTransactionViewModel : BaseTransactionEditionViewModel
    {
        public AddTransactionViewModel()
        {
        }

        public AddTransactionViewModel(Account account) : base(account)
        {
            AddTransactionCommand = new Command(AddTransaction);
            SelectedTransactionType = ChargeType.Expense;
        }

        public ICommand AddTransactionCommand { get; }

        public event EventHandler OnAdded;

        public virtual void AddTransaction()
        {
            if (HasInvalidValues()) return;

            transactionManager.AddChargeAsync(Transaction);

            OnAdded.Invoke(this, EventArgs.Empty);
        }
    }
}
