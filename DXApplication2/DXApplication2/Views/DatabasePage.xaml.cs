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
			if (e.Item is DHFOrder file)
				//	Navigation.PushAsync(new ViewPDFSyncfusion(file));
				Navigation.PushAsync(new SheetsPage());
		}
	}
}