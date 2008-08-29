IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TmpNewProduct]') AND type in (N'U'))
DROP TABLE [dbo].[TmpNewProduct]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[POHistory]') AND type in (N'U'))
DROP TABLE [dbo].[POHistory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PO]') AND type in (N'U'))
DROP TABLE [dbo].[PO]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQHistory]') AND type in (N'U'))
DROP TABLE [dbo].[RFQHistory]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQLine]') AND type in (N'U'))
DROP TABLE [dbo].[RFQLine]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQHeader]') AND type in (N'U'))
DROP TABLE [dbo].[RFQHeader]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[POStatus]') AND type in (N'U'))
DROP TABLE [dbo].[POStatus]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQStatus]') AND type in (N'U'))
DROP TABLE [dbo].[RFQStatus]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[POCorres]') AND type in (N'U'))
DROP TABLE [dbo].[POCorres]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RFQCorres]') AND type in (N'U'))
DROP TABLE [dbo].[RFQCorres]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NoOfferReason]') AND type in (N'U'))
DROP TABLE [dbo].[NoOfferReason]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Purpose]') AND type in (N'U'))
DROP TABLE [dbo].[Purpose]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier_Product]') AND type in (N'U'))
DROP TABLE [dbo].[Supplier_Product]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
DROP TABLE [dbo].[Product]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchasingPaymentTerm]') AND type in (N'U'))
DROP TABLE [dbo].[PurchasingPaymentTerm]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchasingCurrency]') AND type in (N'U'))
DROP TABLE [dbo].[PurchasingCurrency]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchasingUnit]') AND type in (N'U'))
DROP TABLE [dbo].[PurchasingUnit]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IrregularRFQLocation]') AND type in (N'U'))
DROP TABLE [dbo].[IrregularRFQLocation]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Supplier]') AND type in (N'U'))
DROP TABLE [dbo].[Supplier]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchasingCountry]') AND type in (N'U'))
DROP TABLE [dbo].[PurchasingCountry]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PurchasingUser]') AND type in (N'U'))
DROP TABLE [dbo].[PurchasingUser]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role_Privilege]') AND type in (N'U'))
DROP TABLE [dbo].[Role_Privilege]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Role]') AND type in (N'U'))
DROP TABLE [dbo].[Role]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Privilege]') AND type in (N'U'))
DROP TABLE [dbo].[Privilege]
GO

