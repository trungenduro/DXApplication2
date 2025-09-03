using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Spreadsheet;

namespace DXApplication2.ViewModels;

public partial class ExcelImportViewModel : ObservableObject {
    [ObservableProperty]
    bool useDefaultFile;

    [ObservableProperty]
    private IEnumerable<Employee>? employees;

    [RelayCommand]
    async Task Upload() {
        string? filePath = UseDefaultFile ? await GetDefaultFile() : await SelectFile();
        if (string.IsNullOrEmpty(filePath))
            return;

        await LoadDataFromExcel(filePath);
    }

    async Task<string?> SelectFile() {
        var options = new PickOptions { PickerTitle = "Select an Excel file" };
        var openResult = await FilePicker.Default.PickAsync(options);
        return openResult?.FullPath;
    }
    async Task<string> GetDefaultFile() {
        var fileName = "sample_customers_sheet.xlsx";
        var targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, fileName);
        if (File.Exists(targetFile))
            return targetFile;

        using Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync(fileName);
        using FileStream outputStream = File.OpenWrite(targetFile);
        fileStream.CopyTo(outputStream);
        outputStream.Flush();
        return targetFile;
    }
    async Task LoadDataFromExcel(string filePath) {
        using Workbook newCustomersWorkbook = new Workbook();
        bool excelOpenResult = await newCustomersWorkbook.LoadDocumentAsync(filePath);
        if (!excelOpenResult) {
            await ShowError("Couldn't open the file");
            return;
        }
        CellRange valuesRange = newCustomersWorkbook.Worksheets[0].GetDataRange();
        Worksheet firstWorkSheet = newCustomersWorkbook.Worksheets[0];
        int topRowIndex = valuesRange.TopRowIndex;
        int leftColumnIndex = valuesRange.LeftColumnIndex;
        if (!IsValidDataStructure(firstWorkSheet, topRowIndex, leftColumnIndex)) {
            await ShowError("Data structure in the selected file is invalid");
            return;
        }
        var newEmployees = new List<Employee>();
        for (int rowIndex = topRowIndex + 1; rowIndex < valuesRange.RowCount + topRowIndex; rowIndex++) {
            var employee = new Employee() {
                FirstName = firstWorkSheet.Rows[rowIndex][leftColumnIndex].Value.TextValue,
                LastName = firstWorkSheet.Rows[rowIndex][leftColumnIndex + 1].Value.TextValue,
                Company = firstWorkSheet.Rows[rowIndex][leftColumnIndex + 2].Value.TextValue
            };
            newEmployees.Add(employee);
        }

        Employees = newEmployees;
    }

    bool IsValidDataStructure(Worksheet workSheet, int topRowIndex, int leftColumnIndex) {
        return workSheet.Rows[topRowIndex][leftColumnIndex].Value.TextValue == "First Name" &&
            workSheet.Rows[topRowIndex][leftColumnIndex + 1].Value.TextValue == "Last Name" &&
            workSheet.Rows[topRowIndex][leftColumnIndex + 2].Value.TextValue == "Company";
    }
    Task ShowError(string message) {
        if (Application.Current?.MainPage == null)
            return Task.CompletedTask;

        return Shell.Current.DisplayAlert("Error", message, "OK");
    }
}

public class Employee {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
}