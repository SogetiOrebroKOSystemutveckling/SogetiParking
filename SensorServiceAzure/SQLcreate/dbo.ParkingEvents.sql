CREATE TABLE [dbo].ParkingEvents
(
	[Event_Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SpaceNumber] INT NOT NULL, FOREIGN KEY (SpaceNumber) REFERENCES ParkingSpaces(Space_Id),
    [TimeStamp] DATETIME2 NOT NULL, 
    [IsFree] BIT NULL, 
    [ErrorCode] NVARCHAR(50) NULL

)
