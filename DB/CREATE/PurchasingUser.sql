/****** オブジェクト:  Table [dbo].[PurchasingUser]    スクリプト日付: 08/28/2008 13:38:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchasingUser](
	[UserID] [int] NOT NULL,
	[RoleCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[PrivilegeLevel] [varchar](1) COLLATE Japanese_CI_AS NOT NULL CONSTRAINT [DF_PurchasingUser_PrivilegeLevel]  DEFAULT ('P'),
	[R3PurchasingGroup] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[isAdmin] [bit] NOT NULL CONSTRAINT [DF_PurchasingUser_isAdmin]  DEFAULT ((0)),
	[isDisabled] [bit] NOT NULL CONSTRAINT [DF_PurchasingUser_isDisabled]  DEFAULT ((0)),
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_PurchasingUser_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_PurchasingUser_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_PurchasingUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PurchasingUser]  WITH CHECK ADD  CONSTRAINT [FK_PurchasingUser_Role] FOREIGN KEY([RoleCode])
REFERENCES [dbo].[Role] ([RoleCode])
GO
ALTER TABLE [dbo].[PurchasingUser]  WITH CHECK ADD  CONSTRAINT [CK_PurchasingUser_PrivilegeLevel] CHECK  (([PrivilegeLevel]='P' OR [PrivilegeLevel]='A'))
GO
