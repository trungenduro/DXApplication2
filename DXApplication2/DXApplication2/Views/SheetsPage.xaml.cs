using DXApplication2.ViewModels;
using LiningCheckRecord;

namespace DXApplication2.Views;

public partial class SheetsPage : ContentPage
{
	public SheetsPage()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;

	DHFOrder? dhfOrder;

	public SheetsPage(DatabaseViewModel viewModel, DHFOrder order)
	{
		InitializeComponent();
		DatabaseViewModel = viewModel;
		BindingContext = viewModel;
		dhfOrder = order;

	}



}