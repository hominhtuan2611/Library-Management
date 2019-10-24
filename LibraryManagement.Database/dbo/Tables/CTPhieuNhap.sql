CREATE TABLE [dbo].[CTPhieuNhap]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[PhieuNhap] INT NOT NULL, 
    [Book] VARCHAR(20) NOT NULL, 
    [SoLuong] INT NOT NULL, 
    [TinhTrangSach] NVARCHAR(50) NULL, 
    CONSTRAINT [FK_CTPhieuNhap_PhieuNhap] FOREIGN KEY ([PhieuNhap]) REFERENCES [PhieuNhap]([Id]), 
    CONSTRAINT [FK_CTPhieuNhap_Books] FOREIGN KEY ([Book]) REFERENCES [Sach]([Id])
)
