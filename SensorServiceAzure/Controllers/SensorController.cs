using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SensorServiceAzure.Controllers
{
    public class SensorController : ApiController
    {
        private static readonly List<ParkingSpace> _parkingLot;

        static SensorController()
        {
            _parkingLot = new List<ParkingSpace>()
            {
                new ParkingSpace("99", "warm") {IsFree = false},
                new ParkingSpace("100", "warm") {IsFree = false},
                new ParkingSpace("51","cold") {IsFree = false},
                new ParkingSpace("52","cold") {IsFree = false},
                new ParkingSpace("53","cold") {IsFree = false}
            };
        }

        public IHttpActionResult GetSensors()
        {
            return Ok(_parkingLot);
        }

        public IHttpActionResult PutParkingSpace(ParkingSpaceRegistration parkingSpaceRegistration)
        {
            var parkingSpace = _parkingLot.SingleOrDefault(x => x.SpaceNumber == parkingSpaceRegistration.SpaceNumber);
            if (parkingSpace == null)
            {
                return NotFound();
            }

            parkingSpace.IsFree = parkingSpaceRegistration.IsFree;
            return Ok();
        }
    }
}