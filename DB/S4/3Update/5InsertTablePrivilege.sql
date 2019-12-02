USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
INSERT INTO [dbo].[Privilege] (
	[PrivilegeCode], 
    [Name] , 
    [ScriptName], 
    [Action],
    [CreatedBy],
    [CreateDate],
    [UpdatedBy],
    [UpdateDate])
VALUES
	('9210','PurchaseGroup一覧','PurchaseGroup',NULL,0,GETDATE(),0,GETDATE()),
	('9211','PurchaseGroup設定','PurchaseGroupSetting',NULL,0,GETDATE(),0,GETDATE()),
	('9212','PurchaseGroup設定','PurchaseGroupSetting','Edit',0,GETDATE(),0,GETDATE()),
	('9213','PurchaseGroup設定','PurchaseGroupSetting','Save',0,GETDATE(),0,GETDATE())
