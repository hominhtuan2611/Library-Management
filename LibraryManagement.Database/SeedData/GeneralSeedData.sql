USE [LibraryDB]

SET IDENTITY_INSERT [BookTypes] ON
GO
IF NOT EXISTS (SELECT TOP 1 * FROM [BookTypes])
BEGIN
INSERT INTO [BookTypes] ([TypeName]) 
VALUES ('Tiểu thuyết')
END
GO
SET IDENTITY_INSERT [BookTypes] OFF
GO

SET IDENTITY_INSERT [Books] ON
GO
IF NOT EXISTS (SELECT TOP 1 * FROM [Books])
BEGIN
INSERT INTO [Books] ([Id], [Name],[Author],[BookType],[PublishingYear] ,[Publisher], [Summary],[TotalPage]) 
VALUES (9786045389393, 'Kẻ trộm sách', 'Markus Zuskas',1, 2017, 'Nhà xuất bản Hội Nhà Văn','Có rất nhiều câu chuyện mà tôi cho phép chúng ta làm tôi xao nhãng trong khi làm việc...',571)
END
GO
SET IDENTITY_INSERT [dbo].[Books] OFF
GO

