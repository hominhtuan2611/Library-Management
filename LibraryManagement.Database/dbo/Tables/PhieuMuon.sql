CREATE TABLE [dbo].[PhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MaDG] INT NOT NULL, 
	[MaNV] INT NOT NULL, 
    [NgayMuon] DATE NOT NULL, 
    [TongSachMuon] INT NULL, 
	[HanTra] DATE NOT NULL, 
	[DaTra] BIT NOT NULL DEFAULT 0,
	[TrangThai] INT, 
    CONSTRAINT [FK_PhieuMuon_NhanVien] FOREIGN KEY ([MaNV]) REFERENCES [NhanVien]([Id]), 
    CONSTRAINT [FK_PhieuMuon_KhachHang] FOREIGN KEY ([MaDG]) REFERENCES [DocGia]([Id])
)
