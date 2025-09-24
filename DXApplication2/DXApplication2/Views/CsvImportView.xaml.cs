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

	// �[���I�ȁu�߂�v��u�L�����Z���v�{�^����FilePicker�\���O�ɗp�ӂ��AFilePicker���̂̕\�����̓��[�U�[���߂邱�Ƃ͂ł��܂���B
	// FilePicker��OS��UI�ł���A.NET MAUI����͕\�����̃L�����Z����߂�{�^���̐���͂ł��܂���B
	// ���̂��߁AFilePicker�\����̓��[�U�[��OS�̖߂�{�^����L�����Z���{�^���ŕ���̂�҂�������܂���B
	// �������APickAsync�̖߂�l��null�̏ꍇ�i���[�U�[���L�����Z����߂��I�������ꍇ�j�́A�����̂悤�ɃL�����Z�����b�Z�[�W��\���ł��܂��B
	// �ǉ��̐���͂ł��܂��񂪁A���L�̂悤�ɃR�����g�𖾎����Ă����Ƃ悢�ł��傤�B

	private async void SelectCSV_Clicked(object sender, EventArgs e)
	{
#if ANDROID
        try
        {
            var pickOptions = new Microsoft.Maui.Storage.PickOptions
            {
                PickerTitle = "CSV�t�@�C����I��", 
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "text/csv", "application/vnd.ms-excel", ".csv" } }
                })
            };

            // FilePicker�\�����̓��[�U�[�̓A�v���̖߂鑀��͂ł��܂���iOS�̎d�l�j
            var result = await FilePicker.Default.PickAsync(pickOptions);
            if (result == null)
            {
                await DisplayAlert("�L�����Z��", "�t�@�C���I�����L�����Z������܂����B", "OK");
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
                    await DisplayAlert("CSV�ǂݍ��݃G�[���[", $"�s��: {csvData.Count}", "OK");
                    return;
                }
                if (csvData.GroupBy(x => x[0]).Count() > 1)
                {
                    await DisplayAlert("CSV�ǂݍ��݃G�[���[", $"�s��: {csvData.Count}", "OK");
                    return;
                }
                if (this.BindingContext is DatabaseViewModel viewmodel)
                {
                    viewmodel.ImportCsv(csvData);
                }
                await DisplayAlert("CSV�ǂݍ��݊���", $"�s��: {csvData.Count}", "OK");
            }

            await DisplayAlert("�I������", $"�t�@�C����: {result.FileName}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("�G���[", $"�t�@�C���I�𒆂ɃG���[���������܂���: {ex.Message}", "OK");
        }
#else
		await DisplayAlert("���Ή�", "���̋@�\��Android�ł̂ݗ��p�\�ł��B", "OK");
#endif
	}
	private DHFOrder ConvertCsvDataToDHFOrder(List<string> csvData)
	{
		// ����: 1�s�ڂ̓w�b�_�[�A2�s�ڈȍ~���f�[�^
		// �K�v�ɉ����ăJ�������𒲐����Ă�������
		if (csvData == null || csvData.Count < 5)
			return null;

				var order = new DHFOrder { OrderNo = csvData[0], �q�於 = csvData[1] };	

		return order;
	}

	private void DXButton_Clicked_1(object sender, EventArgs e)
	{

	}

	private void csvSelect_Clicked(object sender, EventArgs e)
	{

	}
}


