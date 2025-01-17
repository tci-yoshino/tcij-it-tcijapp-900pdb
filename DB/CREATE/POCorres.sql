/****** オブジェクト:  Table [dbo].[POCorres]    スクリプト日付: 08/28/2008 13:34:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POCorres](
	[POCorresCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Text] [nvarchar](128) COLLATE Japanese_CI_AS NOT NULL,
	[SortOrder] [smallint] NOT NULL,
 CONSTRAINT [PK_POCorres] PRIMARY KEY CLUSTERED 
(
	[POCorresCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF

GO