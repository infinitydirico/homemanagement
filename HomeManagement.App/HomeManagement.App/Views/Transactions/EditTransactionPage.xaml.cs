﻿using Autofac;
using HomeManagement.App.Data.Entities;
using HomeManagement.App.Managers;
using HomeManagement.App.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditTransactionPage : ContentPage
	{
        private Account account;
        ILocalizationManager localizationManager = App._container.Resolve<ILocalizationManager>();

        public EditTransactionPage()
        {
            InitializeComponent();

            Title = localizationManager.Translate("EditTransaction");
        }

        public EditTransactionPage(Account account, Transaction Transaction) : this()
        {
            this.account = account;
            var viewModel = new EditTransactionViewModel(account, Transaction);
            BindingContext = viewModel;

            viewModel.OnTransactionUpdated += ViewModel_OnTransactionUpdated;
            viewModel.OnError += ViewModel_OnError;
            viewModel.OnCancel += ViewModel_OnCancel;
        }

        private void ViewModel_OnCancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ViewModel_OnError(object sender, EventArgs e)
        {
            DisplayAlert("Error", "Algunos de los datos ingresados no son validos", string.Empty);
        }

        private void ViewModel_OnTransactionUpdated(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}