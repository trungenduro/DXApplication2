using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using Microsoft.Maui.Controls;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;

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
        DHFOrder ActiveOrder;
        int ActiveHandle = -1;
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
          await  vm.Validate(e);
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

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            if(sender  is  DXBorder border)
            {
                
               if(border.BindingContext is DHFOrder order)
                {
                   // order.IsFavorite = !order.IsFavorite;
                    border.BackgroundColor = order.IsFavorite ? Colors.AliceBlue : Colors.Transparent;
                    if(border.Content is DXImage image)
                        image.TintColor = order.IsFavorite ? Colors.Red : Colors.Black;
                    DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
                    vm.AddToFavorites( order);
                   // await vm.UpdateOrderAsync();
                }
            }
        }

        private void EditClick(object sender, EventArgs e)
        {

        }

        private void DeleteClick(object sender, EventArgs e)
        {

        }

        private void DismissPopup(object sender, EventArgs e)
        {

        }
    }
}