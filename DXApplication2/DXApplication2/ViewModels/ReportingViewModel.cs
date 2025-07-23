using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Maui.Pdf;
using DXApplication2.ReportLibrary;

namespace DXApplication2.ViewModels;

public partial class ReportingViewModel : ObservableObject {

    [ObservableProperty]
    PdfDocumentSource? documentSource;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(InitializeCommand))]
    bool isInitialized;

    [RelayCommand(CanExecute = nameof(CanInitialize))]
    async Task InitializeAsync() {
        await Task.Run(() => {
            var report = new XtraReportInstance() { Name = "Sample" };
            report.CreateDocument();
            string resultFile = Path.Combine(FileSystem.Current.AppDataDirectory, report.Name + ".pdf");
            report.ExportToPdf(resultFile);
            DocumentSource = PdfDocumentSource.FromFile(resultFile);
        });
        IsInitialized = true;
    }

    bool CanInitialize() => !IsInitialized;
}
