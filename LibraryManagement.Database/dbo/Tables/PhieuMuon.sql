CREATE TABLE [dbo].[PhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MaKH] INT NOT NULL, 
    [NgayMuon] DATE NOT NULL, 
    [TongSachMuon] INT NULL, 
	[HanTra] DATE NULL, 
	[TrangThai] NVARCHAR(50),	
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
