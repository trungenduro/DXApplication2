using DXApplication2.ViewModels;

namespace DXApplication2.Views;

public partial class NewSpoolPage : ContentPage
{
	public NewSpoolPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;
	public NewSpoolPage(DatabaseViewModel model)
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