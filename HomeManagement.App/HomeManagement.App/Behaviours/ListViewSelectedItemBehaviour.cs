using HomeManagement.App.Extensions;
using Xamarin.Forms;

namespace HomeManagement.App.Behaviours
{
    public class ListViewSelectedItemBehaviour : Behavior<ListView>
    {
        private ListView listView;

        protected override void OnAttachedTo(ListView bindable)
        {
            listView = bindable;
            bindable.ItemTapped += Bindable_ItemTapped;
            bindable.ItemSelected += Bindable_ItemSelected;

            base.OnAttachedTo(bindable);
        }

        private void Bindable_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem == null)
            {
                foreach (ViewCell cell in listView.GetCells() as ITemplatedItemsList<Cell>)
                {
                    cell.View.BackgroundColor = Color.FromHex("#303030");
                }
            }
        }

        private void Bindable_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            foreach (ViewCell cell in listView.GetCells() as ITemplatedItemsList<Cell>)
            {
                if (cell.BindingContext != null && cell.BindingContext.Equals(e.Item))
                {
                    cell.View.BackgroundColor = Color.FromHex("#212121");
                }
                else
                {
                    cell.View.BackgroundColor = Color.FromHex("#303030");
                }
            }
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            bindable.ItemTapped -= Bindable_ItemTapped;
            bindable.ItemSelected -= Bindable_ItemSelected;
            base.OnDetachingFrom(bindable);
        }
    }
}
