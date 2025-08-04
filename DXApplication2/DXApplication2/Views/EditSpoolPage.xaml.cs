using DXApplication2.ViewModels;

namespace DXApplication2.Views;

public partial class EditSpoolPage : ContentPage
{
	public EditSpoolPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;
	public EditSpoolPage(DatabaseViewModel model)
	{

		DatabaseViewModel = model;
		this.BindingContext = model;
		InitializeComponent();
	}

	private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
		var ct= this.BindingContext;
	}
}