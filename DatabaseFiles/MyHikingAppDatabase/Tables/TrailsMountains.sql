CREATE TABLE [dbo].[TrailsMountains]
(
  [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
  [MountainId] UNIQUEIDENTIFIER NOT NULL,
  [TrailId] UNIQUEIDENTIFIER NOT NULL,
  CONSTRAINT FK_TrailsMountains_Mountains FOREIGN KEY ([MountainId])
    REFERENCES Mountains([Id]),
  CONSTRAINT FK_TrailsMountains_Trails FOREIGN KEY ([TrailId])
    REFERENCES Trails([Id])
);
