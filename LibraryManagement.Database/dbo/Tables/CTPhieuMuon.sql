CREATE TABLE [dbo].[CTPhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PhieuMuon] INT NOT NULL, 
    [Book] VARCHAR(20) NOT NULL, 
    [SoLuong] INT NOT NULL, 
    [TinhTrangSach] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_CTPhieuMuon_PhieuMuon] FOREIGN KEY ([PhieuMuon]) REFERENCES [PhieuMuon]([Id]), 
    CONSTRAINT [FK_CTPhieuMuon_Books] FOREIGN KEY ([Book]) REFERENCES [Sach]([Id])
)
