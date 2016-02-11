using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Markup;
using SensorServiceAzure.Controllers;

namespace SensorServiceAzure
{
    public class ParkingDataRepository
    {
        public static List<ParkingSpace> GetParkingSpaces()
        {
            List<ParkingSpace> parkingSpaces = new List<ParkingSpace>();

            var connString =
                System.Configuration.ConfigurationManager.ConnectionStrings["parkingDataConnectionString"]
                    .ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();

                var command =
                    new SqlCommand(
                        @"SELECT ParkingSpaces.SpaceNumber, ParkingLots.LotDescription, a.IsFree, a.Last_TimeStamp " +
                        "FROM ParkingSpaces INNER JOIN ParkingLots ON ParkingSpaces.ParkingLot = ParkingLots.Lot_Id " +
                        "LEFT JOIN ( " +
                        "SELECT space.SpaceNumber as Number, lot.LotDescription, IsFree, EventTimeStamp AS Last_TimeStamp " +
                        "FROM ParkingEvents " +
                        "INNER JOIN ParkingSpaces space ON space.Space_Id = ParkingEvents.SpaceNumber " +
                        "INNER JOIN ParkingLots lot ON lot.Lot_Id = space.ParkingLot " +
                        "WHERE EventTimeStamp IN " +
                        "(SELECT MAX(EventTimeStamp) " +
                        "FROM ParkingEvents " +
                        "GROUP BY SpaceNumber)" +
                        ") AS a " +
                        "ON ParkingSpaces.SpaceNumber = a.Number") {Connection = sqlConnection};
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    parkingSpaces = ParseParkingSpaces(reader);
                }
            }

            return parkingSpaces;
        }

        private static List<ParkingSpace> ParseParkingSpaces(SqlDataReader reader)
        {
            var spaceList = new List<ParkingSpace>();
            
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ParkingSpaceStatus isFree;
                    DateTime lastTimeStamp;
                    var spaceNumber = reader.GetValue(0);
                    var lot = reader.GetValue(1);

                    try
                    {
                        lastTimeStamp = reader.GetDateTime(3);
                        isFree = ProcessSpaceStatus(reader.GetBoolean(2), lastTimeStamp);
                    }
                    catch (Exception)
                    {
                        isFree = ParkingSpaceStatus.Unknown;
                        lastTimeStamp = DateTime.MinValue;
                    }
                    spaceList.Add(new ParkingSpace(spaceNumber.ToString(), lot.ToString(), isFree.ToString(),
                        lastTimeStamp.ToString("G", CultureInfo.CreateSpecificCulture("sv-SE"))));
                }
            }
            
            return spaceList;
        }
    

        private static ParkingSpaceStatus ProcessSpaceStatus(bool status, DateTime timeStamp)
        {
                if (DateTime.UtcNow - timeStamp > TimeSpan.FromMinutes(15))
                {
                    return ParkingSpaceStatus.Unknown;
                }
                
                if(status) return ParkingSpaceStatus.Free;

                return ParkingSpaceStatus.Occupied;
        }

        public static void GetParkingSpace(string spaceNumber)
        {
            var connString =
                System.Configuration.ConfigurationManager.ConnectionStrings["parkingDataConnectionString"]
                    .ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();

                var command =
                    new SqlCommand(
                        @"SELECT ParkingSpaces.SpaceNumber, ParkingLots.LotDescription, a.IsFree, a.Last_TimeStamp " +
                        "FROM ParkingSpaces INNER JOIN ParkingLots ON ParkingSpaces.ParkingLot = ParkingLots.Lot_Id " +
                        "LEFT JOIN ( " +
                        "SELECT space.SpaceNumber as Number, lot.LotDescription, IsFree, EventTimeStamp AS Last_TimeStamp " +
                        "FROM ParkingEvents " +
                        "INNER JOIN ParkingSpaces space ON space.Space_Id = ParkingEvents.SpaceNumber " +
                        "INNER JOIN ParkingLots lot ON lot.Lot_Id = space.ParkingLot " +
                        "WHERE EventTimeStamp IN " +
                        "(SELECT MAX(EventTimeStamp) " +
                        "FROM ParkingEvents " +
                        "GROUP BY SpaceNumber)" +
                        ") AS a ON ParkingSpaces.SpaceNumber = a.Number") {Connection = sqlConnection};
            }
        }

        public static void StoreParkingEvent(ParkingSpace space, string errorCode = "")
        {
            var connString = System.Configuration.ConfigurationManager.ConnectionStrings["parkingDataConnectionString"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(connString))
            {
                sqlConnection.Open();
                var idCommand =
                    new SqlCommand("SELECT Space_Id FROM ParkingSpaces WHERE SpaceNumber=" + space.SpaceNumber + ";");
                idCommand.Connection = sqlConnection;
                var reader = idCommand.ExecuteReader();

                int spaceId = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        spaceId = reader.GetInt32((reader.GetOrdinal("Space_Id")));
                    }
                }

                reader.Close();
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "INSERT INTO ParkingEvents (SpaceNumber, IsFree, ErrorCode, EventTimeStamp) " +
                                  "VALUES(@SpaceNumber, @IsFree, @errorCode, @EventTimeStamp)"
                };
                cmd.Parameters.Add("@SpaceNumber", SqlDbType.Int).Value = spaceId;
                cmd.Parameters.Add("@IsFree", SqlDbType.Bit).Value = space.IsFree;
                cmd.Parameters.Add("@errorCode", SqlDbType.NVarChar).Value = errorCode;
                cmd.Parameters.Add("@EventTimeStamp", SqlDbType.DateTime2).Value = DateTime.UtcNow;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = sqlConnection;

                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }
    }
}