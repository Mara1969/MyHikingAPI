CREATE TABLE [dbo].[TrailMountains]
(
  [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
  [MountainId] UNIQUEIDENTIFIER NOT NULL,
  [TrailId] UNIQUEIDENTIFIER NOT NULL

  CONSTRAINT FK_TrailMountains_Mountains FOREIGN KEY ([MountainId])
        REFERENCES Mountains([Id]),
  CONSTRAINT FK_TrailMountains_Trails FOREIGN KEY ([TrailId])
        REFERENCES Trails([Id])
);
