using HomeManagement.App.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace HomeManagement.App.Common
{
    public static class Extensions
    {
        public static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
              (this Dictionary<TKey, TValue> original)
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
                                                                    original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value);
            }
            return ret;
        }

        public static string ToFullDate(this DateTime dateTime) => dateTime.Day < 10 ? dateTime.ToString("d MMMM yyyy") : dateTime.ToString("dd MMMM yyyy");

        public static Xamarin.Forms.Page GetParent(this VisualElement element)
        {
            Element parent = element?.Parent;
            while (parent?.Parent != null)
            {
                parent = parent?.Parent;
            }
            return parent as Xamarin.Forms.Page;
        }

        public static ToolbarItem CreateLanguateToolbarItem(this BaseViewModel baseViewModel)
        {
            return new ToolbarItem
            {
                Icon = baseViewModel.LanguageFlag,

                Text = baseViewModel.LanguageText,

                Command = baseViewModel.ChangeLanguageCommand
            };
        }
    }
}
