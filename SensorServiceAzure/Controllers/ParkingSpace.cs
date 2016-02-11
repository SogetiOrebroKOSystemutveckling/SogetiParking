using System;

namespace SensorServiceAzure.Controllers
{
    public class ParkingSpace
    {
        public ParkingSpace(string spaceNumber, string garage)
        {
            SpaceNumber = spaceNumber;
            Garage = garage;

        }

        public ParkingSpace(string spaceNumber, string garage, string isFree, string lastDateTime)
        {
            IsFree = isFree;
            LastDateTime = lastDateTime;
            SpaceNumber = spaceNumber;
            Garage = garage;
        }

        public string IsFree { get; set; }
        public string LastDateTime { get; set; }
        public string SpaceNumber { get; set; }
        public string Garage { get; set; }
    }

    public enum ParkingSpaceStatus
    {
        Occupied = 0,
        Free = 1,
        Unknown = -1
    }
}