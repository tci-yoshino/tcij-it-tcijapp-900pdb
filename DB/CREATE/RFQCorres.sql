/****** オブジェクト:  Table [dbo].[RFQCorres]    スクリプト日付: 08/28/2008 13:39:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RFQCorres](
	[RFQCorresCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Text] [nvarchar](128) COLLATE Japanese_CI_AS NOT NULL,
	[SortOrder] [smallint] NOT NULL,
 CONSTRAINT [PK_RFQCorres] PRIMARY KEY CLUSTERED 
(
	[RFQCorresCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
