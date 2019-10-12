CREATE TABLE [dbo].[CTPhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PhieuMuon] INT NOT NULL, 
    [Book] INT NOT NULL, 
    [SoLuong] INT NOT NULL, 
    [NgayMuon] DATE NULL, 
    [NgayTra] DATE NULL, 
    [TinhTrangSach] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_CTPhieuMuon_PhieuMuon] FOREIGN KEY ([PhieuMuon]) REFERENCES [PhieuMuon]([Id]), 
    CONSTRAINT [FK_CTPhieuMuon_ToBooks] FOREIGN KEY ([Book]) REFERENCES [Books]([Id])
)
