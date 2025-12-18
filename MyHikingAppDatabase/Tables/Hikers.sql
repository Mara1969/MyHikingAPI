CREATE TABLE [dbo].[Hikers]
(
  [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
  [Name] VARCHAR(100) NOT NULL,
  [DateOfBirth] DATE NOT NULL,
  [ExperienceLevelId] INT NOT NULL

  CONSTRAINT FK_Hikers_ExperienceLevel FOREIGN KEY ([ExperienceLevelId])
        REFERENCES ExperienceLevel([Id])
);
