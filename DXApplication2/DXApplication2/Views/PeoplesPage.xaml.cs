using DevExpress.Maui.DataGrid;
using DXApplication2.ViewModels;
using LiningCheckRecord;
using Microsoft.Maui.Controls;

namespace DXApplication2.Views
{
    public partial class PeoplesPage : ContentPage
    {
        public PeoplesPage()
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
			if (e.Item is CheckerTable peo)
			{
				vm.RemovePeople(peo);
			}
			//collectionView.Commands..dr

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

		private void ToolbarItem_Clicked_1(object sender, EventArgs e)
		{

		}

		private void save_Clicked(object sender, EventArgs e)
		{
			DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
			vm.SaveDatabase();
		}
	}
}