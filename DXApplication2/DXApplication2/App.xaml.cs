using DXApplication2.Views;

namespace DXApplication2;

public partial class App : Application {
    public App() {

	//	Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "*.db")
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