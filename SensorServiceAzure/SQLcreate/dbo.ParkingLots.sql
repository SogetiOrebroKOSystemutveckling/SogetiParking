CREATE TABLE [dbo].ParkingLots
(
	[Lot_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(50) NOT NULL, 
    [StreetAddress] NVARCHAR(50) NULL
)
