namespace SensorServiceAzure.Controllers
{
    public class ParkingSpace
    {
        public ParkingSpace(string spaceNumber, string garage)
        {
            SpaceNumber = spaceNumber;
            Garage = garage;
        }

        public bool IsFree { get; set; }
        public string SpaceNumber { get; set; }
        public string Garage { get; set; }
    }
}