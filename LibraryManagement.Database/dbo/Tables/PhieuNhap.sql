CREATE TABLE [dbo].[PhieuNhap]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [NgayNhap] DATE NOT NULL, 
    [SoLuong] INT NULL, 
    [NhaCungCap] NVARCHAR(50) NULL,
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)
