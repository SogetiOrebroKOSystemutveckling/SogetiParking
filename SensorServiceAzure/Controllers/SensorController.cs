using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace SensorServiceAzure.Controllers
{
    public class SensorController : ApiController
    {
        private static readonly List<ParkingSpace> _parkingLot;

        static SensorController()
        {
          
        }

        public IHttpActionResult GetSensors()
        {
            var a = ParkingDataRepository.GetParkingSpaces();
            return Ok(a);
        }

        public IHttpActionResult PutParkingSpace(ParkingSpaceRegistration parkingSpaceRegistration)
        {
            var parkingLot = ParkingDataRepository.GetParkingSpaces();
            var parkingSpace = parkingLot.SingleOrDefault(x => x.SpaceNumber == parkingSpaceRegistration.SpaceNumber);
            if (parkingSpace == null)
            {
                return NotFound();
            }
            var newValue = parkingSpaceRegistration.IsFree.ToString();

            parkingSpace.IsFree = newValue;
            ParkingDataRepository.StoreParkingEvent(parkingSpace);
            
            return Ok();
        }
    }
}