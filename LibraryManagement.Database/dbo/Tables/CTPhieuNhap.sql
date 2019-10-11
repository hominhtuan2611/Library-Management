CREATE TABLE [dbo].[CTPhieuNhap]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[IdPhieuNhap] INT NOT NULL, 
    [IdBook] INT NOT NULL, 
    [SoLuong] INT NOT NULL, 
    [TinhTrangSach] NVARCHAR(50) NULL,
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
