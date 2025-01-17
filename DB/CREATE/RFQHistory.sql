/****** オブジェクト:  Table [dbo].[RFQHistory]    スクリプト日付: 08/28/2008 13:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RFQHistory](
	[RFQHistoryNumber] [int] IDENTITY(1,1) NOT NULL,
	[RFQNumber] [int] NOT NULL,
	[RFQStatusCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[StatusChangeDate] [datetime] NOT NULL CONSTRAINT [DF_RFQHistory_StatusChangeDate]  DEFAULT (getdate()),
	[RFQCorresCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[Note] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[SendLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[SendUserID] [int] NULL,
	[RcptLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[RcptUserID] [int] NULL,
	[isChecked] [bit] NOT NULL CONSTRAINT [DF_RFQHistory_isChecked]  DEFAULT ((0)),
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_RFQHistory_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_RFQHistory_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_RFQHistory] PRIMARY KEY CLUSTERED 
(
	[RFQHistoryNumber] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RFQHistory]  WITH CHECK ADD  CONSTRAINT [FK_RFQHistory_PurchasingUser1] FOREIGN KEY([RcptUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[RFQHistory]  WITH CHECK ADD  CONSTRAINT [FK_RFQHistory_PurchasingUser2] FOREIGN KEY([SendUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[RFQHistory]  WITH CHECK ADD  CONSTRAINT [FK_RFQHistory_RFQCorres] FOREIGN KEY([RFQCorresCode])
REFERENCES [dbo].[RFQCorres] ([RFQCorresCode])
GO
ALTER TABLE [dbo].[RFQHistory]  WITH CHECK ADD  CONSTRAINT [FK_RFQHistory_RFQHeader] FOREIGN KEY([RFQNumber])
REFERENCES [dbo].[RFQHeader] ([RFQNumber])
GO
ALTER TABLE [dbo].[RFQHistory]  WITH CHECK ADD  CONSTRAINT [FK_RFQHistory_RFQStatus] FOREIGN KEY([RFQStatusCode])
REFERENCES [dbo].[RFQStatus] ([RFQStatusCode])
GO
