/****** オブジェクト:  Table [dbo].[Role_Privilege]    スクリプト日付: 08/28/2008 13:41:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role_Privilege](
	[RoleCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[PrivilegeCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Role_Privilege_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Role_Privilege_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Role_Privilege] PRIMARY KEY CLUSTERED 
(
	[RoleCode] ASC,
	[PrivilegeCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Role_Privilege]  WITH CHECK ADD  CONSTRAINT [FK_Role_Privilege_Privilege] FOREIGN KEY([PrivilegeCode])
REFERENCES [dbo].[Privilege] ([PrivilegeCode])
GO
ALTER TABLE [dbo].[Role_Privilege]  WITH CHECK ADD  CONSTRAINT [FK_Role_Privilege_Role] FOREIGN KEY([RoleCode])
REFERENCES [dbo].[Role] ([RoleCode])
GO
