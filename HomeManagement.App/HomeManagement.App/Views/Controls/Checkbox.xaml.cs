﻿using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Checkbox : StackLayout
    {
        public Checkbox()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create("Checked", typeof(bool), typeof(Checkbox),
                                    defaultValue: false,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: CheckboxChanged);

        public bool Checked
        {
            get => (bool)GetValue(CheckedProperty);
            set => SetValue(CheckedProperty, value);
        }

        private void Check(object sender, EventArgs e)
        {
            checkboxChecked.IsVisible = true;
            checkboxUnchecked.IsVisible = false;
            Checked = true;
        }

        private void Uncheck(object sender, EventArgs e)
        {
            checkboxChecked.IsVisible = false;
            checkboxUnchecked.IsVisible = true;
            Checked = false;
        }

        private static void CheckboxChanged(BindableObject sender, object oldValue, object newValue)
        {
            var self = sender as Checkbox;
            var value = (bool)newValue;
            if (value)
            {
                self.Check(self, EventArgs.Empty);
            }
            else
            {
                self.Uncheck(self, EventArgs.Empty);
            }
        }
    }
}