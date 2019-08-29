using HomeManagement.App.Extensions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class EmptyValidationBackgroundBehavior : Behavior<Entry>
    {
        Entry entry;
        bool animating = false;
        Color currentColor;

        protected override void OnAttachedTo(Entry bindable)
        {
            entry = bindable;
            currentColor = entry.BackgroundColor;
            bindable.Unfocused += Bindable_Unfocused;
            bindable.Focused += Bindable_Focused;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.Unfocused -= Bindable_Unfocused;
            bindable.Focused -= Bindable_Focused;
            base.OnDetachingFrom(bindable);
        }

        private async void Bindable_Focused(object sender, FocusEventArgs e)
        {
            animating = false;
            await entry.BackgroundColorTo(currentColor);
        }

        private void Bindable_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(entry.Text))
            {
                animating = true;

                AnimateBackground();
            }
        }

        private void AnimateBackground()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    if (!animating) break;

                    await entry.BackgroundColorTo(Color.Red);
                    await Task.Delay(750);
                    await entry.BackgroundColorTo(currentColor);
                }
            });
        }
    }
}
