/****** オブジェクト:  Table [dbo].[NoOfferReason]    スクリプト日付: 08/28/2008 13:33:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NoOfferReason](
	[NoOfferReasonCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Text] [nvarchar](128) COLLATE Japanese_CI_AS NOT NULL,
	[SortOrder] [smallint] NOT NULL,
 CONSTRAINT [PK_NoOfferReason] PRIMARY KEY CLUSTERED 
(
	[NoOfferReasonCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
