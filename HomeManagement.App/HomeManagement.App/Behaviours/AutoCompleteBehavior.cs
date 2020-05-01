using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class AutoCompleteBehavior : Behavior<Entry>
    {
        List<Label> options = new List<Label>();
        Entry entry;

        public static readonly BindableProperty SuggestionsProperty =
            BindableProperty.Create("Suggestions",
                                    typeof(List<string>),
                                    typeof(AutoCompleteBehavior),
                                    new List<string>(),
                                    propertyChanged: OnSuggestionChanged);

        public List<string> Suggestions
        {
            get { return (List<string>)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        private static void OnSuggestionChanged(BindableObject bindable, object newValue, object oldValue)
        {
            var autoComplete = (AutoCompleteBehavior)bindable;
            autoComplete.options.AddRange(autoComplete.InitializeLabels(autoComplete.Suggestions).ToList());
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            entry = bindable;
            entry.Focused += OnEntryFocused;
            entry.Unfocused += OnEntryUnfocused;
            entry.TextChanged += OnTextChanged;
            base.OnAttachedTo(bindable);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ClearSuggestions();
            AddSuggestions();
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            AddSuggestions();
        }

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            ClearSuggestions();
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            entry.Focused -= OnEntryFocused;
            entry.Unfocused -= OnEntryUnfocused;
            entry.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private async void AddSuggestions()
        {
            var parent = entry.Parent as StackLayout;

            foreach (var label in string.IsNullOrEmpty(entry.Text) ?
                                    options :
                                    options.Where(x => x.Text.ToLower().Contains(entry.Text.ToLower())))
            {
                parent.Children.Add(label);
                await label.FadeTo(1, length: 100 ,easing: Easing.SpringIn);
            }
        }

        private void ClearSuggestions()
        {
            var parent = entry.Parent as StackLayout;
            foreach (var child in options)
            {
                parent.Children.Remove(child);
            }            
        }

        private void Gesture_Tapped(object sender, System.EventArgs e)
        {
            var label = sender as Label;
            entry.Text = label.Text;
            ((IEntryController)entry).SendCompleted();
        }

        private IEnumerable<Label> InitializeLabels(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                var label = new Label
                {
                    Text = value,
                    Margin = new Thickness(10, 0, 0, 0),
                    TextColor = Color.Gold,
                    Opacity = 0
                };
                var gesture = new TapGestureRecognizer();
                gesture.Tapped += Gesture_Tapped;
                label.GestureRecognizers.Add(gesture);

                yield return label;
            }
        }
    }
}
