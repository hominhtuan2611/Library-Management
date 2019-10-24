CREATE TABLE [dbo].[Sach]
(
	[Id] VARCHAR(20) NOT NULL PRIMARY KEY, 
    [TenSach] NVARCHAR(100) NOT NULL, 
    [TacGia] NVARCHAR(30) NOT NULL, 
    [NhaXuatBan] NVARCHAR(100) NULL, 
    [NamXuatBan] INT NULL, 
    [TongSoTrang] INT NULL, 
    [TomTat] NVARCHAR(MAX) NULL, 
    [LoaiSach] INT NULL, 
	[SoLuong] INT NULL, 
	[HinhAnh] NVARCHAR(MAX) NULL, 
    [TrangThai] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [FK_Books_ToBookTypes] FOREIGN KEY ([LoaiSach]) REFERENCES [LoaiSach]([Id])
)
