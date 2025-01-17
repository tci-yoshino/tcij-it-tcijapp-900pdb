/****** オブジェクト:  Table [dbo].[IrregularRFQLocation]    スクリプト日付: 08/28/2008 13:31:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IrregularRFQLocation](
	[EnqLocationCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[SupplierCode] [int] NOT NULL,
	[QuoLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_IrregularRFQLocation_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_IrregularRFQLocation_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_IrregularRFQLocation] PRIMARY KEY CLUSTERED 
(
	[EnqLocationCode] ASC,
	[SupplierCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IrregularRFQLocation]  WITH CHECK ADD  CONSTRAINT [FK_IrregularRFQLocation_Supplier] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Supplier] ([SupplierCode])

GO