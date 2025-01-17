/****** オブジェクト:  Table [dbo].[RFQLine]    スクリプト日付: 08/28/2008 13:40:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RFQLine](
	[RFQLineNumber] [int] IDENTITY(1,1) NOT NULL,
	[RFQNumber] [int] NOT NULL,
	[EnqQuantity] [decimal](10, 3) NOT NULL,
	[EnqUnitCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[EnqPiece] [int] NOT NULL,
	[CurrencyCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[UnitPrice] [decimal](13, 3) NULL,
	[QuoPer] [decimal](8, 3) NULL,
	[QuoUnitCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[LeadTime] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SupplierItemNumber] [nvarchar](128) COLLATE Japanese_CI_AS NULL,
	[IncotermsCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[DeliveryTerm] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Packing] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Purity] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[QMMethod] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[NoOfferReasonCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_RFQLine_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_RFQLine_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_RFQLine] PRIMARY KEY CLUSTERED 
(
	[RFQLineNumber] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RFQLine]  WITH CHECK ADD  CONSTRAINT [FK_RFQLine_NoOfferReason] FOREIGN KEY([NoOfferReasonCode])
REFERENCES [dbo].[NoOfferReason] ([NoOfferReasonCode])
GO
ALTER TABLE [dbo].[RFQLine]  WITH CHECK ADD  CONSTRAINT [FK_RFQLine_PurchasingCurrency] FOREIGN KEY([CurrencyCode])
REFERENCES [dbo].[PurchasingCurrency] ([CurrencyCode])
GO
ALTER TABLE [dbo].[RFQLine]  WITH CHECK ADD  CONSTRAINT [FK_RFQLine_PurchasingUnit_Enq] FOREIGN KEY([EnqUnitCode])
REFERENCES [dbo].[PurchasingUnit] ([UnitCode])
GO
ALTER TABLE [dbo].[RFQLine]  WITH CHECK ADD  CONSTRAINT [FK_RFQLine_PurchasingUnit_Quo] FOREIGN KEY([QuoUnitCode])
REFERENCES [dbo].[PurchasingUnit] ([UnitCode])
GO
ALTER TABLE [dbo].[RFQLine]  WITH CHECK ADD  CONSTRAINT [FK_RFQLine_RFQHeader] FOREIGN KEY([RFQNumber])
REFERENCES [dbo].[RFQHeader] ([RFQNumber])
GO
