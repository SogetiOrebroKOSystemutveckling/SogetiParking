using System;
using Xamarin.Forms;

namespace SogetiParking
{
	public class MainPageViewModel
	{
		public MainPageViewModel ()
		{
			ColdGarageLeft = "Ledig";
			ColdGarageCenter = "Upptagen";
			ColdGarageRight = "Ledig";
			WarmGarageLeft = "Upptagen";
			WarmGarageRight = "Ledig";
		}

		public string ColdGarageLeft { get; set; }
		public string ColdGarageCenter { get; set; }
		public string ColdGarageRight { get; set; }
		public string WarmGarageLeft { get; set; }
		public string WarmGarageRight { get; set; }
	}
}