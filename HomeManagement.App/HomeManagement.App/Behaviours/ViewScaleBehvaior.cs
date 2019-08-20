using System;
using System.Linq;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class ViewScaleBehvaior : Behavior<View>
    {
        protected override void OnAttachedTo(View bindable)
        {
            var tappedGesture = new TapGestureRecognizer();
            tappedGesture.Tapped += ViewTapped;
            bindable.GestureRecognizers.Add(tappedGesture);
            base.OnAttachedTo(bindable);
        }

        private async void ViewTapped(object sender, EventArgs e)
        {
            var view = sender as View;

            await view.ScaleTo(0.8, easing: Easing.SinIn);
            await view.ScaleTo(1, easing: Easing.SinOut);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            var gesture = bindable.GestureRecognizers.First() as TapGestureRecognizer;
            gesture.Tapped -= ViewTapped;
            bindable.GestureRecognizers.Clear();
            base.OnDetachingFrom(bindable);
        }
    }
}
