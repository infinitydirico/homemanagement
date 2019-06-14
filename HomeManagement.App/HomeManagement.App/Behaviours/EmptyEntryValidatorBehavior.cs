using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class EmptyEntryValidatorBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += EntryTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= EntryTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                ((Entry)sender).BackgroundColor = Color.FromHex("#282828");
                ((Entry)sender).PlaceholderColor = Color.Red;
            }
            else
            {
                ((Entry)sender).BackgroundColor = Color.Transparent;
                ((Entry)sender).PlaceholderColor = Color.Transparent;
            }
        }
    }
}
