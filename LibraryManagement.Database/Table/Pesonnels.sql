CREATE TABLE [dbo].[Pesonnels]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(30) NOT NULL, 
    [Gender] NVARCHAR(10) NOT NULL, 
    [Birthday] DATE NOT NULL, 
    [IdCard] NCHAR(12) NOT NULL, 
    [Address] NVARCHAR(50) NULL, 
    [Phone] NCHAR(11) NULL, 
	[Position] NVARCHAR(30) NULL,
    [Enabe] BIT NOT NULL DEFAULT 1, 
    [CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
