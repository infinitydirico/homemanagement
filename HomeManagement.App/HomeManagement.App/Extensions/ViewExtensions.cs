using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HomeManagement.App.Extensions
{
    public static class ViewExtensions
    {
        public static ITemplatedItemsList<Cell> GetCells(this ListView listView)
        {
            var templatedItems = (listView as ItemsView<Cell>)
                .GetType()
                .GetRuntimeProperties()
                .FirstOrDefault(info => info.Name == "TemplatedItems");

            if (templatedItems == null) throw new NullReferenceException($"The parameter {listView} does not have any TemplatedItems");

            return (ITemplatedItemsList<Cell>)templatedItems.GetValue(listView);
        }

        public static Task<bool> BackgroundColorTo(this VisualElement self, Color toColor, uint length = 250, Easing easing = null)
        {
            Color fromColor = self.BackgroundColor;
            Func<double, Color> transform = (t) =>
                Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                               fromColor.G + t * (toColor.G - fromColor.G),
                               fromColor.B + t * (toColor.B - fromColor.B),
                               fromColor.A + t * (toColor.A - fromColor.A));

            Action<Color> callback = (c) => self.BackgroundColor = c;
            return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
        }

        public static Task<bool> BorderColorTo(this Frame self, Color toColor, uint length = 250, Easing easing = null)
        {
            Color fromColor = self.BackgroundColor;
            Func<double, Color> transform = (t) =>
                Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                               fromColor.G + t * (toColor.G - fromColor.G),
                               fromColor.B + t * (toColor.B - fromColor.B),
                               fromColor.A + t * (toColor.A - fromColor.A));

            Action<Color> callback = (c) => self.BorderColor = c;
            return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
        }

        public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
        {
            Func<double, Color> transform = (t) =>
                Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
                               fromColor.G + t * (toColor.G - fromColor.G),
                               fromColor.B + t * (toColor.B - fromColor.B),
                               fromColor.A + t * (toColor.A - fromColor.A));
            return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
        }

        public static void CancelAnimation(this VisualElement self)
        {
            self.AbortAnimation("ColorTo");
        }

        static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
        {
            easing = easing ?? Easing.Linear;
            var taskCompletionSource = new TaskCompletionSource<bool>();

            element.Animate<Color>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
            return taskCompletionSource.Task;
        }

        public static TViewModel GetViewModel<TViewModel>(this Element element)
        {
            TViewModel viewModel;
            Element searchedView = element;

            while (searchedView.Parent != null)
            {
                searchedView = searchedView.Parent;
                if(searchedView.BindingContext is TViewModel)
                {
                    viewModel = (TViewModel)searchedView.BindingContext;
                    return viewModel;
                }
            }
            throw new Exception($"No viewmodel found for {typeof(TViewModel).Name}");
        }
    }
}
