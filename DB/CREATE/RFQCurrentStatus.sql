/****** オブジェクト:  Table [dbo].[RFQCurrentStatus]    スクリプト日付: 08/10/2009 14:40:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RFQCurrentStatus](
	[RFQNumber] [int] NOT NULL,
	[StatusCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Text] [nvarchar](128) COLLATE Japanese_CI_AS NOT NULL,
	[SortOrder] [smallint] NOT NULL,
	[ChangeDate] [datetime] NOT NULL,
 CONSTRAINT [PK_RFQCurrentStatus] PRIMARY KEY CLUSTERED 
(
	[RFQNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[RFQCurrentStatus]  WITH CHECK ADD  CONSTRAINT [FK_RFQCurrentStatus_StatusCode] FOREIGN KEY([RFQNumber])
REFERENCES [dbo].[RFQCurrentStatus] ([RFQNumber])
GO