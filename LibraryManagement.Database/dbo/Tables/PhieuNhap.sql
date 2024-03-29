﻿CREATE TABLE [dbo].[PhieuNhap]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [NgayNhap] DATE NOT NULL, 
    [SoLuong] INT NULL, 
    [NhaCungCap] NVARCHAR(50) NULL, 
    [NhanVienNhap] INT NULL, 
	[TrangThai] INT, 
    CONSTRAINT [FK_PhieuNhap_NhanVien] FOREIGN KEY ([NhanVienNhap]) REFERENCES [NhanVien]([Id]) 
)
