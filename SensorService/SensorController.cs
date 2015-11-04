using System.Collections.Generic;
using System.Web.Http;

namespace SensorService
{
    public class SensorController : ApiController
    {
        public IHttpActionResult GetSensors()
        {
            var warmGarage = new Garage("Varmgarage");
            warmGarage.ParkingLots.Add(new ParkingLot("99") { IsFree = true });
            warmGarage.ParkingLots.Add(new ParkingLot("100") { IsFree = false });

            var coldGarage = new Garage("Kallgarage");
            coldGarage.ParkingLots.Add(new ParkingLot("51") { IsFree = true });
            coldGarage.ParkingLots.Add(new ParkingLot("52") { IsFree = false });
            coldGarage.ParkingLots.Add(new ParkingLot("53") { IsFree = true });

            var garages = new List<Garage> {warmGarage, coldGarage};

            return Ok(garages);
        }
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