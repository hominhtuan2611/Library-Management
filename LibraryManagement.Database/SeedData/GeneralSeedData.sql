USE [LibraryDB]

SET IDENTITY_INSERT [BookTypes] ON
GO
IF NOT EXISTS (SELECT TOP 1 * FROM [BookTypes])
BEGIN
INSERT INTO [BookTypes] ([Id],[TypeName]) 
VALUES (1, N'Tiểu thuyết')
END
GO
SET IDENTITY_INSERT [BookTypes] OFF
GO

IF NOT EXISTS (SELECT TOP 1 * FROM [Books])
BEGIN
INSERT INTO [Books] ([Id], [Name],[Author],[PublishingYear] ,[Publisher], [Summary],[TotalPage],[BookType],[Status]) 
VALUES ('9786045389393', N'Kẻ trộm sách', N'Markus Zuskas', 2017, N'Nhà xuất bản Hội Nhà Văn', N'Có rất nhiều câu chuyện mà tôi cho phép chúng ta làm tôi xao nhãng trong khi làm việc...',571, 1, 1)
END
GO

