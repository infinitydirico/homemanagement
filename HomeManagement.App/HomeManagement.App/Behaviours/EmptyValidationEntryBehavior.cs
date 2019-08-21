using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class EmptyValidationEntryBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.Unfocused += OnUnfocused;
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
                await ShakeIt(entry);
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
