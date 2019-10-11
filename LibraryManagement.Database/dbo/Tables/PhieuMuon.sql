CREATE TABLE [dbo].[PhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MaKH] INT NOT NULL, 
    [NgayMuon] DATE NOT NULL, 
    [TongSachMuon] INT NULL, 
	[HanTra] DATE NOT NULL, 
	[DaTra] BIT NOT NULL DEFAULT 0,
	[TrangThai] INT,	
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
