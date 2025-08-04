namespace DXApplication2.Views;

public partial class NewSheetView : ContentPage
{
	public NewSheetView()
	{
		InitializeComponent();
	}

    private void ContentPage_BindingContextChanged(object sender, EventArgs e)
    {
		var b = this.BindingContext;

    }
}