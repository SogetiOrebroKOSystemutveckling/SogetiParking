using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Newtonsoft.Json;
using Org.Json;
using Xamarin.Forms;

namespace SogetiParking
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
	    private List<ParkingLot> _garages;

	    public MainPageViewModel ()
		{
            RefreshCommand = new Command(() =>
            {
                Garages = GetGarages();
            } );

		    Garages = GetGarages();
		}

	    public ICommand RefreshCommand { get; private set; }

	    public List<ParkingLot> Garages
	    {
	        get { return _garages; }
	        set
	        {
	            _garages = value;
                OnPropertyChanged();
	        }
	    }

	    public List<ParkingLot> GetGarages()
	    {
            var request = WebRequest.Create("http://192.168.1.144:8080/api/Sensor/GetSensors");
	        request.ContentType = "application/json";
	        request.Method = "GET";

	        try
	        {
	            using (var response = request.GetResponse())
	            using (var responseStream = response.GetResponseStream())
	            {
	                var reader = new StreamReader(responseStream);
	                var content = reader.ReadToEnd();

                    return JsonConvert.DeserializeObject<List<ParkingLot>>(content);
	            }
	        }
	        catch (Exception z)
	        {
	            return null;
	        }
	    }

	    public event PropertyChangedEventHandler PropertyChanged;

	    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	    {
	        var handler = PropertyChanged;
	        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
	    }
	}

    public class ParkingLot
    {
        public ParkingLot(string lotNumber, string garage)
        {
            LotNumber = lotNumber;
            Garage = garage;
        }

        public bool IsFree { get; set; }
        public string LotNumber { get; set; }
        public string Garage { get; set; }
    }
}