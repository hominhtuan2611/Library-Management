CREATE TABLE [dbo].[Books]
(
	[Id] VARCHAR(20) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Author] NVARCHAR(30) NOT NULL, 
    [Publisher] NVARCHAR(30) NULL, 
    [PublishingYear] INT NULL, 
    [TotalPage] INT NULL, 
    [Summary] NVARCHAR(100) NULL, 
    [BookType] INT NULL, 
    [Status] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Books_ToBookTypes] FOREIGN KEY ([BookType]) REFERENCES [BookTypes]([Id])
)
