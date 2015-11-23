using System;
using System.Collections.Generic;
using System.Web.Http;

namespace SensorService
{
    public class SensorController : ApiController
    {
        public IHttpActionResult GetSensors()
        {
            var random = new Random();

            var parkingLots = new List<ParkingLot>
            {
                new ParkingLot("99", "warm") {IsFree = random.Next() % 2 == 0},
                new ParkingLot("100", "warm") {IsFree = false},
                new ParkingLot("51","cold") {IsFree = true},
                new ParkingLot("52","cold") {IsFree = false},
                new ParkingLot("53","cold") {IsFree = true}
            };
            return Ok(parkingLots);
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