/****** オブジェクト:  Table [dbo].[Privilege]    スクリプト日付: 08/28/2008 13:36:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Privilege](
	[PrivilegeCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Name] [nvarchar](255) COLLATE Japanese_CI_AS NOT NULL,
	[ScriptName] [varchar](255) COLLATE Japanese_CI_AS NOT NULL,
	[Action] [varchar](255) COLLATE Japanese_CI_AS NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Privilege_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Privilege_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Privilege] PRIMARY KEY CLUSTERED 
(
	[PrivilegeCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
