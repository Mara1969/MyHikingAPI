CREATE TABLE [dbo].[Trails]
(
  [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
  [Name] NVARCHAR(100) NOT NULL,
  [DifficultyId] INT NOT NULL,
  [Distance] DECIMAL(6,2) NOT NULL, -- this is in km
  [GeographicTrailUrl] NVARCHAR(200) NULL,

    CONSTRAINT FK_Trails_Difficulty FOREIGN KEY ([DifficultyId])
        REFERENCES Difficulty([Id])
);
