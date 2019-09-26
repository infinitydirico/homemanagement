using HomeManagement.App.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeManagement.App.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RadioGroup : StackLayout
    {
        private const string radioButtonChecked = "radio_button_checked_24dp.png";
        private const string radioButtonUnchecked = "radio_button_unchecked_24dp.png";

        public RadioGroup()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty OptionsProperty =
            BindableProperty.Create("Options", typeof(IEnumerable<TransactionType>), typeof(RadioGroup),
                                    defaultValue: null,
                                    propertyChanged: LoadOptions);

        public IEnumerable<TransactionType> Options
        {
            get => (IEnumerable<TransactionType>)GetValue(OptionsProperty);
            set
            {
                SetValue(OptionsProperty, value);
            }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create("SelectedItem", typeof(TransactionType), typeof(RadioGroup),
                                    defaultValue: null,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: SelectedItemChanged);

        public TransactionType SelectedItem
        {
            get => (TransactionType)GetValue(SelectedItemProperty);
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        private static void LoadOptions(BindableObject bindale, object oldValue, object newValue)
        {
            var self = bindale as RadioGroup;
            var options = self.Options;
            if(options != null && options.Count() > 0)
            {
                foreach (var option in options)
                {
                    var button = new Button
                    {
                        Image = self.SelectedItem.Equals(option) ? radioButtonChecked : radioButtonUnchecked,
                        BackgroundColor = Color.Transparent,
                        Text = option.ToString()
                    };

                    button.Clicked += self.OptionChanged;
                    
                    self.Children.Add(button);
                }
            }
        }

        private void OptionChanged(object sender, EventArgs e)
        {
            foreach (Button button in Children)
            {
                if (button.Equals(sender))
                {
                    button.Image = radioButtonChecked;
                    var selectedItem = Options.First(x => x.ToString().Equals(button.Text));
                    SelectedItem = selectedItem;
                }
                else
                {
                    button.Image = radioButtonUnchecked;
                }
            }
        }

        private static void SelectedItemChanged(BindableObject bindale, object oldValue, object newValue)
        {
            var self = bindale as RadioGroup;

            if (self.Children.Count.Equals(0)) return;

            var optionToSetSelected = self.Children.First(x => (x as Button).Text.Equals(newValue.ToString())) as Button;
            self.OptionChanged(optionToSetSelected, EventArgs.Empty);
        }
    }
}