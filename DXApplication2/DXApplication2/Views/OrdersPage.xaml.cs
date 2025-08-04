using DevExpress.Maui.DataGrid;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using Microsoft.Maui.Controls;

namespace DXApplication2.Views
{
    public partial class OrdersPage : ContentPage
    {
        public OrdersPage()
        {
            InitializeComponent();
        }

		private void collectionView_DoubleTap(object sender, DevExpress.Maui.DataGrid.DataGridGestureEventArgs e)
		{

			Navigation.PushAsync(new SheetsPage());

		}

		private void Swipe_Open(object sender, DevExpress.Maui.DataGrid.SwipeItemTapEventArgs e)
		{
			DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
			if (e.Item is DHFOrder file)
			{
                vm.CurrentOrder = file;
			//	DataGridView collectionView = new DataGridView();
                collectionView.ShowDetailEditForm(e.RowHandle);
              //  Navigation.PushAsync(new SheetsPage(vm, file));
			}
			//collectionView.Commands.ShowDetailEditForm

		}

		private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
		{
			DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
            e.AutoUpdateItemsSource = true; // Ensure the items source is updated automatically
											//   await vm.Validate(e);

		}

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
          //  grid.ShowDetailEditForm(e.RowHandle);
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
            vm.RefreshCommand.Execute(null);
        }
    }
}