using DXApplication2.Views;

namespace DXApplication2;

public partial class App : Application {
    public App() {


		//var filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "Photo");
		//var files = Directory.GetFiles(Path.Combine(FileSystem.Current.AppDataDirectory, "Photo"), "*.*").ToList();
		//		.ToList()
		//	.ForEach(file => File.Delete(file));


		using var entitiesContext = new LiningCheckRecord.LiningCheckContext();
        SQLitePCL.Batteries_V2.Init();
        entitiesContext.Database.EnsureCreated();
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState) {
		return new Window(new AppShell());
	}
}