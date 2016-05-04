using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Azure.NotificationHubs;

namespace SensorServiceAzure.Controllers
{
    public class SensorController : ApiController
    {
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

            if (parkingSpace.IsFree.ToLower() != parkingSpaceRegistration.IsFree.ToString().ToLower())
            {
                Notification.SendNew(parkingSpace.SpaceNumber, parkingSpaceRegistration.IsFree.ToString());
            }

            var newValue = parkingSpaceRegistration.IsFree.ToString();

            parkingSpace.IsFree = newValue;
            ParkingDataRepository.StoreParkingEvent(parkingSpace);
            
            return Ok();
        }
    }

    public static class Notification
    {
        public static async void SendNew(string spaceNumber, string status)
        {
            NotificationHubClient hub = NotificationHubClient
        .CreateClientFromConnectionString("Endpoint=sb://sogetiparkingnotificationhub.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=sFVC+SZ6JlqBzo1W9wCrBRM9I/JAIyzGW554J+oiRGo=", "SogetiParkingNotificationHub");
            var msg = spaceNumber + " is now " + status;
            await hub.SendAdmNativeNotificationAsync(msg);
        }
    }
}