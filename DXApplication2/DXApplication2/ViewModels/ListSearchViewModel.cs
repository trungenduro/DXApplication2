using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DXApplication2.ViewModels;

public partial class ListSearchViewModel : ObservableObject {

    [ObservableProperty]
    private IEnumerable<ContactItem> items;

    [ObservableProperty]
    private string? filterString;

    public ListSearchViewModel() {
        var names = new string[] { "Robert King", "Nancy Davolio", "Michael Suyama", "Steven Buchanan", "Margaret Peacock", "Andrew Fuller" };
        var phones = new string[] { "(71) 55-4848", "(206) 555-9857", "(71) 555-7773", "(71) 555-4848", "(206) 555-8122", "(206) 555-9482" };
        Items = Enumerable.Range(0, names.Length).Select(i => new ContactItem {
            Name = names[i],
            Description = phones[i]
        }).ToList();
    }

    [RelayCommand]
    private async Task HandleActionAsync(ContactItem item) {
        await Shell.Current.DisplayAlert(item.Name, "Action executed", "OK");
    }
    [RelayCommand]
    private void TextChanged(string text) {
        FilterString = $"Contains([Name], '{text}') or Contains([Description], '{text}')";
    }
}

public class ContactItem {
    public string? Name { get; set; }
    public string? Description { get; set; }
}