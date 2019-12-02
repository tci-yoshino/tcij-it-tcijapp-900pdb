USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageByPurchasingUser](
	[UserID] [int] NOT NULL,
	[Storage] [varchar](5) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StorageByPurchasingUser]  WITH CHECK ADD  CONSTRAINT [FK_StorageByPurchasingUser_Storage] FOREIGN KEY([Storage])
REFERENCES [dbo].[StorageLocation] ([Storage])
GO
ALTER TABLE [dbo].[StorageByPurchasingUser] CHECK CONSTRAINT [FK_StorageByPurchasingUser_Storage]
GO
ALTER TABLE [dbo].[StorageByPurchasingUser]  WITH CHECK ADD  CONSTRAINT [FK_StorageByPurchasingUser_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[StorageByPurchasingUser] CHECK CONSTRAINT [FK_StorageByPurchasingUser_UserID]
GO


