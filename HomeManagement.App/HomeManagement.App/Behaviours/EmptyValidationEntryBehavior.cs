using HomeManagement.App.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class EmptyValidationEntryBehavior : Behavior<Entry>
    {
        Entry entry;
        Color currentColor;
        Color red = Color.FromHex("#ef5350");

        protected override void OnAttachedTo(Entry bindable)
        {
            entry = bindable;
            bindable.Unfocused += OnUnfocused;
            currentColor = bindable.BackgroundColor;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Unfocused -= OnUnfocused;
            base.OnDetachingFrom(bindable);
        }

        private async void OnUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;

            if (string.IsNullOrWhiteSpace(entry.Text))
            {
                await entry.BackgroundColorTo(red);
                await ShakeIt(entry);
                await entry.BackgroundColorTo(currentColor);
            }            
        }

        private async Task ShakeIt(Entry entry)
        {
            uint timeout = 50;
            await entry.TranslateTo(-15, 0, timeout);
            await entry.TranslateTo(15, 0, timeout);
            await entry.TranslateTo(-10, 0, timeout);
            await entry.TranslateTo(10, 0, timeout);
            await entry.TranslateTo(-5, 0, timeout);
            await entry.TranslateTo(5, 0, timeout);
            entry.TranslationX = 0;
        }
    }
}
