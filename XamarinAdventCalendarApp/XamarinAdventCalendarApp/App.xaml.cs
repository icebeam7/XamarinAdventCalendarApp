using Xamarin.Forms;

namespace XamarinAdventCalendarApp
{
    public partial class App : Application
    {
        public App() 
        {
            InitializeComponent();

            MainPage = new Views.PublicationView();
        }
    }
}
