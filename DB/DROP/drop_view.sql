IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_CompetitorProduct]'))
DROP VIEW [dbo].[v_CompetitorProduct]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_Country]'))
DROP VIEW [dbo].[v_Country]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_PO]'))
DROP VIEW [dbo].[v_PO]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_POCurrentStatus]'))
DROP VIEW [dbo].[v_POCurrentStatus]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_POReminder]'))
DROP VIEW [dbo].[v_POReminder]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RFQCurrentStatus]'))
DROP VIEW [dbo].[v_RFQCurrentStatus]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RFQHeader]'))
DROP VIEW [dbo].[v_RFQHeader]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RFQLine]'))
DROP VIEW [dbo].[v_RFQLine]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_RFQReminder]'))
DROP VIEW [dbo].[v_RFQReminder]
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_User]'))
DROP VIEW [dbo].[v_User]
GO
