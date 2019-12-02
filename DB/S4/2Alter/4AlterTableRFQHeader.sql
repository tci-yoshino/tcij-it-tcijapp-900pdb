USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE RFQHeader ADD EnqStorageLocation varchar(5) NULL
ALTER TABLE RFQHeader ADD QuoStorageLocation varchar(5) NULL
ALTER TABLE RFQHeader ADD SupplierContactPersonSel nvarchar(55) NULL
ALTER TABLE RFQHeader ADD SAPMakerCode int NULL
