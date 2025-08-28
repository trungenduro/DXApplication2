namespace DXApplication2.Views
{
    public partial class ItemEditing3Page : ContentPage
    {
        public ItemEditing3Page()
        {
            InitializeComponent();
        }

		private void DXButton_Clicked(object sender, EventArgs e)
		{
				Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "*.db")
					.ToList()
				.ForEach(file => File.Delete(file));

			using var entitiesContext = new LiningCheckRecord.LiningCheckContext();
			SQLitePCL.Batteries_V2.Init();
			entitiesContext.Database.EnsureCreated();
		}
    }
}