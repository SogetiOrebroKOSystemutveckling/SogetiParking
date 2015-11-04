using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Org.Json;
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

		    Garages = GetGarages();
		}

	    public List<Garage> Garages { get; set; } 

	    public List<Garage> GetGarages()
	    {
	        var request = WebRequest.Create("http://localhost:8080/api/Sensor/GetSensors");
	        request.ContentType = "application/json";
	        request.Method = "GET";

	        using (var response = request.GetResponse())
	        using (var responseStream = response.GetResponseStream())
	        {
	            var reader = new StreamReader(responseStream);
	            var content = reader.ReadToEnd();

	            return JsonConvert.DeserializeObject<List<Garage>>(content);
	        }
	    }

	    public string ColdGarageLeft { get; set; }
		public string ColdGarageCenter { get; set; }
		public string ColdGarageRight { get; set; }
		public string WarmGarageLeft { get; set; }
		public string WarmGarageRight { get; set; }
	}

    public class Garage
    {
        public Garage(string name)
        {
            Name = name;
            ParkingLots = new List<ParkingLot>();
        }

        public string Name { get; private set; }

        public List<ParkingLot> ParkingLots { get; set; }
    }

    public class ParkingLot
    {
        public ParkingLot(string lotNumber)
        {
            LotNumber = lotNumber;
        }

        public bool IsFree { get; set; }
        public string LotNumber { get; set; }
    }
}