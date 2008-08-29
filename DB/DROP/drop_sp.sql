
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_NewProductImport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_NewProductImport]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_ProductNameImport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_ProductNameImport]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SupplierImport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_SupplierImport]
GO

