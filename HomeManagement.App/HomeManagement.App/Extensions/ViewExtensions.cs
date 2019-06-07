using System;
using System.Linq;
using System.Reflection;
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
    }
}
