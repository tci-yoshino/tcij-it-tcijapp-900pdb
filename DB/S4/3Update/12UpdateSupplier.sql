USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE Supplier ALTER COLUMN S4SupplierCode  NVARCHAR(20) NULL;
ALTER TABLE supplier ADD SupplierWarning NVARCHAR(3000) NULL;