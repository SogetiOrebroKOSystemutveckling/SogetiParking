CREATE TABLE [dbo].ParkingSpaces
(
	[Space_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SpaceNumber] INT NOT NULL, 
    [ParkingLot] INT NOT NULL, FOREIGN KEY (ParkingLot) REFERENCES ParkingLots(Lot_Id)
)
