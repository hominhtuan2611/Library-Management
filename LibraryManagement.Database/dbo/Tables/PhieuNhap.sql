CREATE TABLE [dbo].[PhieuNhap]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [NgayNhap] DATE NOT NULL, 
    [SoLuong] INT NULL, 
    [NhaCungCap] NVARCHAR(50) NULL, 
    [NhanVienNhap] INT NULL, 
    CONSTRAINT [FK_PhieuNhap_Pesonnels] FOREIGN KEY ([NhanVienNhap]) REFERENCES [Personnels]([Id]) 
)
