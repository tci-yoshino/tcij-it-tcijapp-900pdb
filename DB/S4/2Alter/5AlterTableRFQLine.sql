USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE RFQLine ADD SupplierOfferNo nvarchar(200) NULL
ALTER TABLE RFQLine ADD OutputStatus bit NULL