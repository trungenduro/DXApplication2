using AndroidX.Lifecycle;

using DevExpress.Maui.Charts;
using DevExpress.Maui.Core;

using DevExpress.Maui.Editors;

using DXApplication2.ViewModels;
using LiningCheckRecord;
using SkiaSharp;
using static Android.Icu.Text.IDNA;

namespace DXApplication2.Views;

public partial class CsvImportView : ContentPage
{
	public CsvImportView()
	{
		InitializeComponent();
	}

	DatabaseViewModel DatabaseViewModel;
	
	private void Save_Clicked(object sender, EventArgs e)
	{
        DatabaseViewModel.UpdateOrderAsync();
    }

	private void collectionView_CreateDetailFormViewModel(object sender, DevExpress.Maui.Core.CreateDetailFormViewModelEventArgs e)
	{

	}



	//add edit spool
    private async void collectionView_ValidateAndSave(object sender, DevExpress.Maui.Core.ValidateItemEventArgs e)
    {
		if( this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{

			

            if (e.Item is LiningSpool sp)
			{
				string mess = "";
				if (Sheet != null)
				{
					DatabaseViewModel.CheckSpool(Sheet, e);

				}
				
			}

            DatabaseViewModel.Validate(e);
            //form.SaveCommand.execute
            // await form.SaveAsync();
        }

    }
	DevExpress.Maui.Core.DetailEditFormViewModel editform;

	DHFOrder order;
	ExcelSheet Sheet;

	private void ContentPage_BindingContextChanged(object sender, EventArgs e)
	{
		if (this.BindingContext is DevExpress.Maui.Core.DetailEditFormViewModel form)
		{
			editform = form;
			if(form.Item is ExcelSheet sh)
				Sheet = sh;
			if (form.DataControlContext is DetailEditFormViewModel form1)
			{
				order = form1.Item as DHFOrder;				
				if (form1.DataControlContext is DatabaseViewModel viewmodel)
				{
                    DatabaseViewModel = viewmodel;

                }
			}
		}
	}

  


	LiningSpool ActiveSpool;
	int ActiveHandle = -1;



    private void PhotoEditClick(object sender, EventArgs e)
    {

    }

	private void TextEdit_Completed(object sender, EventArgs e)
	{
		

	}

	private void DXButton_Clicked(object sender, EventArgs e)
	{

	}

	// 擬似的な「戻る」や「キャンセル」ボタンをFilePicker表示前に用意し、FilePicker自体の表示中はユーザーが戻ることはできません。
	// FilePickerはOSのUIであり、.NET MAUIからは表示中のキャンセルや戻るボタンの制御はできません。
	// そのため、FilePicker表示後はユーザーがOSの戻るボタンやキャンセルボタンで閉じるのを待つしかありません。
	// ただし、PickAsyncの戻り値がnullの場合（ユーザーがキャンセルや戻るを選択した場合）は、既存のようにキャンセルメッセージを表示できます。
	// 追加の制御はできませんが、下記のようにコメントを明示しておくとよいでしょう。

	private async void SelectCSV_Clicked(object sender, EventArgs e)
	{
#if ANDROID
        try
        {
            var pickOptions = new Microsoft.Maui.Storage.PickOptions
            {
                PickerTitle = "CSVファイルを選択", 
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "text/csv", "application/vnd.ms-excel", ".csv" } }
                })
            };

            // FilePicker表示中はユーザーはアプリの戻る操作はできません（OSの仕様）
            var result = await FilePicker.Default.PickAsync(pickOptions);
            if (result == null)
            {
                await DisplayAlert("キャンセル", "ファイル選択がキャンセルされました。", "OK");
                return;
            }

            using var stream = await result.OpenReadAsync();
            using var reader = new StreamReader(stream);
            string csvContent = await reader.ReadToEndAsync();
            if (!string.IsNullOrWhiteSpace(csvContent))
            {
                var csvLines = csvContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                var csvData = new List<List<string>>();
                foreach (var line in csvLines)
                {
                    var fields = line.Split(',').Select(f => f.Trim()).ToList();
                    csvData.Add(fields);
                }

                if (csvData.Count == 0)
                {
                    await DisplayAlert("CSV読み込みエーラー", $"行数: {csvData.Count}", "OK");
                    return;
                }
                if (csvData.GroupBy(x => x[0]).Count() > 1)
                {
                    await DisplayAlert("CSV読み込みエーラー", $"行数: {csvData.Count}", "OK");
                    return;
                }
                if (this.BindingContext is DatabaseViewModel viewmodel)
                {
                    viewmodel.ImportCsv(csvData);
                }
                await DisplayAlert("CSV読み込み完了", $"行数: {csvData.Count}", "OK");
            }

            await DisplayAlert("選択完了", $"ファイル名: {result.FileName}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("エラー", $"ファイル選択中にエラーが発生しました: {ex.Message}", "OK");
        }
#else
		await DisplayAlert("未対応", "この機能はAndroidでのみ利用可能です。", "OK");
#endif
	}
	private DHFOrder ConvertCsvDataToDHFOrder(List<string> csvData)
	{
		// 仮定: 1行目はヘッダー、2行目以降がデータ
		// 必要に応じてカラム順を調整してください
		if (csvData == null || csvData.Count < 5)
			return null;

				var order = new DHFOrder { OrderNo = csvData[0], 客先名 = csvData[1] };	

		return order;
	}

	private void DXButton_Clicked_1(object sender, EventArgs e)
	{

	}

	private void csvSelect_Clicked(object sender, EventArgs e)
	{

	}
}


