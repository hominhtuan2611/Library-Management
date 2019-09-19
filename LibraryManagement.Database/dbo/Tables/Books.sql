CREATE TABLE [dbo].[Books]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Author] NVARCHAR(30) NOT NULL, 
    [Publisher] NVARCHAR(30) NULL, 
    [PublishingYear] INT NULL, 
    [TotalPage] INT NULL, 
    [Summary] INT NULL, 
    [BookType] INT NULL, 
    [Status] BIT NOT NULL DEFAULT 1, 
    [CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL, 
    CONSTRAINT [FK_Books_ToPersonnels_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Personnels]([Id]), 
    CONSTRAINT [FK_Books_ToPersonnels_MofifiedBy] FOREIGN KEY ([ModifiedBy]) REFERENCES [Personnels]([Id]), 
    CONSTRAINT [FK_Books_ToBookTypes] FOREIGN KEY ([BookType]) REFERENCES [BookTypes]([Id])
)
