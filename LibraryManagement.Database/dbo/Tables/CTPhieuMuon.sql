CREATE TABLE [dbo].[CTPhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdPhieuMuon] INT NOT NULL, 
    [IdBook] INT NOT NULL, 
    [SoLuong] INT NOT NULL, 
    [NgayMuon] DATE NULL, 
    [NgayTra] DATE NULL, 
    [TinhTrangSach] NVARCHAR(MAX) NULL,
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
