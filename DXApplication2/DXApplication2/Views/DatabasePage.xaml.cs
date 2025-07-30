using DXApplication2.ViewModels;
using LiningCheckRecord;

namespace DXApplication2.Views
{
    public partial class DatabasePage : ContentPage
    {
        public DatabasePage()
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
				Navigation.PushAsync(new SheetsPage(vm, file));
			}
			//collectionView.Commands.ShowDetailEditForm

		}

		private void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
		{
			DatabaseViewModel vm = (DatabaseViewModel)BindingContext;
		}
	}
}