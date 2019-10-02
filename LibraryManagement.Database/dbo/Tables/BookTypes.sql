CREATE TABLE [dbo].[BookTypes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeName] NVARCHAR(50) NOT NULL,
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
