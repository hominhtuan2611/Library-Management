﻿CREATE TABLE [dbo].[CTPhieuMuon]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdPhieuMuon] INT NOT NULL, 
    [IdBook] INT NOT NULL, 
    [SoLuong] INT NOT NULL, 
    [NgayMuon] DATE NULL, 
    [NgayTra] DATE NULL, 
    [TinhTrangSach] NVARCHAR(50) NULL,
	[CreatedDay] DATE NULL, 
    [CreatedBy] INT NULL, 
    [ModifiedDay] DATE NULL, 
    [ModifiedBy] INT NULL
)