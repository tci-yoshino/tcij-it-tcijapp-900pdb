/****** オブジェクト:  Table [dbo].[Role]    スクリプト日付: 08/28/2008 13:41:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[RoleCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Name] [nvarchar](255) COLLATE Japanese_CI_AS NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Role_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Role_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
