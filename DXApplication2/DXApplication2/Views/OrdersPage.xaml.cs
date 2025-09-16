using Android.Bluetooth;
using DevExpress.Maui.Core;
using DevExpress.Maui.DataGrid;
using DevExpress.Maui.Editors;
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
			InitCache();
			this.checkbox.IsChecked = true;

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
                  //  border.BackgroundColor = order.IsFavorite ? Colors.AliceBlue : Colors.Transparent;
                  //  if(border.Content is DXImage image)
                   //     image.TintColor = order.IsFavorite ? Colors.Red : Colors.Black;
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
		private void inputChipGroup_Completed(object sender, DevExpress.Maui.Editors.CompletedEventArgs e)
		{
			var chipGroup = sender as InputChipGroup;
			if (chipGroup.EditorText == null) return;
			if (chipGroup.EditorText.Length <= 1)
			{
				e.ClearEditorText = false;
			}
			else
			{
				if (chipGroup.ItemsSource is List<object> list)
				{

					list.Add(chipGroup.EditorText);
				}
				if (this.BindingContext is DetailEditFormViewModel form)
				{

					if (form.DataControlContext is DetailEditFormViewModel form1)
					{
						//form1.DeleteCommand
						if (form1.DataControlContext is DatabaseViewModel vm) vm.AddPeople(chipGroup.EditorText);
					}
				}
				// AddPeople
				//	IList<CheckerTable> list = chipGroup.ItemsSource as BindingList<CheckerTable>;
				//	list.Add(new CheckerTable() { Name = chipGroup.EditorText });
			}
		}

		private void TextEdit_TextChanged(object sender, EventArgs e)
		{

		}

		private void TextEdit_Completed(object sender, EventArgs e)
		{
			if (sender is TextEdit edit)
			{
				this.collectionView.FilterString = $"Contains([ãqêÊñº],'{edit.Text}' )  OR  Contains([àƒåèñº],'{edit.Text}' )";
			}
		}

		private void inputchip_Completed(object sender, CompletedEventArgs e)
		{
			var chipGroup = sender as InputChipGroup;
			if (chipGroup.EditorText == null) return;
			if (chipGroup.EditorText.Length <= 0)
			{
				e.ClearEditorText = false;
			}
			else
			{				
				if (this.BindingContext is DatabaseViewModel vm)
				{
					vm.OrderFilter.Add(chipGroup.EditorText);
				}

				// AddPeople
				//	IList<CheckerTable> list = chipGroup.ItemsSource as BindingList<CheckerTable>;
				//	list.Add(new CheckerTable() { Name = chipGroup.EditorText });
			}
			ApplyFiler();
		}

		private void inputchip_ChildRemoved(object sender, ElementEventArgs e)
		{
			ApplyFiler();
		}

		private void CheckEdit_CheckedChanged(object sender, EventArgs e)
		{
			if (this.BindingContext is DatabaseViewModel vm)
				vm.AddFilter("ñ¢äÆê¨");
			ApplyFiler();
		}

		void ApplyFiler()
		{
			string filterstring = "";
			List<string> filters = new List<string>();
			
			if (this.BindingContext is DatabaseViewModel vm)
			{
				if (vm.OrderFilter == null) return;
				if (!vm.OrderFilter.Contains("ñ¢äÆê¨") && checkbox.IsChecked == true)
					checkbox.IsChecked = false;
				foreach (var item1 in vm.OrderFilter)
				{
					var item = item1.Trim();
					if (item=="ñ¢äÆê¨")
						filterstring += $"  [IsFinished]=false ";
					else
						filterstring += $"  ( Contains([ãqêÊñº],'{item}' )  OR  Contains([àƒåèñº],'{item}' ) OR  Contains([OrderNo],'{item}' ) )";

					filters.Add(filterstring);
				}
				try
				{

					this.collectionView.FilterString = string.Join(" AND ", filters);
				}
				catch (Exception)
				{
				}
			}
		}


		public void InitCache()
		{
			
			CopyFile("sample.png");
			CopyFile("type1.png");
			CopyFile("type2.png");
			CopyFile("type3.png");

		}
		public void CopyFile(string filename)
		{
			string f = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
			

				var file = File.Create(f);
				var task = FileSystem.Current.OpenAppPackageFileAsync(filename);
				task.Wait();
				var fileStream = task.Result;
				fileStream.CopyTo(file);
				fileStream.Close();
				file.Close();			
		}
	}
}