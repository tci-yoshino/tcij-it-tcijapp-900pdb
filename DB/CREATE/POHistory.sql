
/****** オブジェクト:  Table [dbo].[POHistory]    スクリプト日付: 08/28/2008 13:35:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[POHistory](
	[POHistoryNumber] [int] IDENTITY(1,1) NOT NULL,
	[PONumber] [int] NOT NULL,
	[POStatusCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[StatusChangeDate] [datetime] NOT NULL,
	[POCorresCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[Note] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[SendLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[SendUserID] [int] NULL,
	[RcptLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[RcptUserID] [int] NULL,
	[isChecked] [bit] NOT NULL CONSTRAINT [DF_POHistory_isChecked]  DEFAULT ((0)),
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_POHistory_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_POHistory_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_POHistory] PRIMARY KEY CLUSTERED 
(
	[POHistoryNumber] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[POHistory]  WITH CHECK ADD  CONSTRAINT [FK_POHistory_PO] FOREIGN KEY([PONumber])
REFERENCES [dbo].[PO] ([PONumber])
GO
ALTER TABLE [dbo].[POHistory]  WITH CHECK ADD  CONSTRAINT [FK_POHistory_POCorres] FOREIGN KEY([POCorresCode])
REFERENCES [dbo].[POCorres] ([POCorresCode])
GO
ALTER TABLE [dbo].[POHistory]  WITH CHECK ADD  CONSTRAINT [FK_POHistory_POStatus] FOREIGN KEY([POStatusCode])
REFERENCES [dbo].[POStatus] ([POStatusCode])
GO
ALTER TABLE [dbo].[POHistory]  WITH CHECK ADD  CONSTRAINT [FK_POHistory_PurchasingUser1] FOREIGN KEY([RcptUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[POHistory]  WITH CHECK ADD  CONSTRAINT [FK_POHistory_PurchasingUser2] FOREIGN KEY([SendUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
