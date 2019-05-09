using HomeManagement.App.ViewModels;
using System;
using Xamarin.Forms;

namespace HomeManagement.App.Views.Abstract
{
    public class BaseContentPage<TViewModel> : ContentPage
        where TViewModel : BaseViewModel, new()
    {
        protected BaseViewModel viewModel = new TViewModel();

        public BaseContentPage()
        {
            BindingContext = viewModel;

            viewModel.OnInitializationError += ViewModel_OnError; ;
        }

        private void ViewModel_OnError(object sender, EventArgs e)
        {
            DisplayAlert("Error", "Something went worng....", string.Empty);
        }
    }
}
