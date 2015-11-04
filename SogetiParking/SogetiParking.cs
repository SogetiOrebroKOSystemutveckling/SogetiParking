using Xamarin.Forms;

namespace SogetiParking
{
	public class App : Application
	{
		public App ()
		{
			var mainPage = new MainPage();

			mainPage.BindingContext = new MainPageViewModel();

			MainPage = mainPage;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

