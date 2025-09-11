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
           
        });
        IsInitialized = true;
    }

    bool CanInitialize() => !IsInitialized;
}
