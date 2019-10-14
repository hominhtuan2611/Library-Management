USE [LibraryDB]

SET IDENTITY_INSERT [LoaiSach] ON
GO
IF NOT EXISTS (SELECT TOP 1 * FROM [LoaiSach])
BEGIN
INSERT INTO [LoaiSach] ([Id],[TenLoai]) 
VALUES (1, N'Tiểu thuyết')
END
GO
SET IDENTITY_INSERT [Loaisach] OFF
GO

IF NOT EXISTS (SELECT TOP 1 * FROM [Sach])
BEGIN
INSERT INTO [Sach] ([Id], [TenSach],[TacGia] ,[NamXuatBan],[NhaXuatBan], [TomTat],[TongSoTrang],[LoaiSach],[TrangThai]) 
VALUES ('9786045389393', N'Kẻ trộm sách', N'Markus Zuskas', 2017, N'Nhà xuất bản Hội Nhà Văn', N'Có rất nhiều câu chuyện mà tôi cho phép chúng ta làm tôi xao nhãng trong khi làm việc...',571, 1, 1)
END
GO

