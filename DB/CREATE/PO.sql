/****** オブジェクト:  Table [dbo].[PO]    スクリプト日付: 08/28/2008 13:34:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PO](
	[PONumber] [int] IDENTITY(1000000000,1) NOT NULL,
	[R3PONumber] [varchar](10) COLLATE Japanese_CI_AS NULL,
	[R3POLineNumber] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[PODate] [datetime] NOT NULL,
	[POLocationCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[POUserID] [int] NOT NULL,
	[SOLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[SOUserID] [int] NULL,
	[ProductID] [int] NOT NULL,
	[SupplierCode] [int] NOT NULL,
	[MakerCode] [int] NULL,
	[OrderQuantity] [decimal](10, 3) NOT NULL,
	[OrderUnitCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[DeliveryDate] [datetime] NULL,
	[CurrencyCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[UnitPrice] [decimal](13, 3) NOT NULL,
	[PerQuantity] [decimal](8, 3) NOT NULL,
	[PerUnitCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[PaymentTermCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[IncotermsCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[DeliveryTerm] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[PurposeCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[RawMaterialFor] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[RequestedBy] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SupplierItemNumber] [nvarchar](128) COLLATE Japanese_CI_AS NULL,
	[SupplierLotNumber] [nvarchar](128) COLLATE Japanese_CI_AS NULL,
	[DueDate] [datetime] NULL,
	[GoodsArrivedDate] [datetime] NULL,
	[LotNumber] [varchar](10) COLLATE Japanese_CI_AS NULL,
	[InvoiceReceivedDate] [datetime] NULL,
	[ImportCustomClearanceDate] [datetime] NULL,
	[QMStartingDate] [datetime] NULL,
	[QMFinishDate] [datetime] NULL,
	[QMResult] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[RequestQuantity] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[ScheduledExportDate] [datetime] NULL,
	[PurchasingRequisitionNumber] [nvarchar](128) COLLATE Japanese_CI_AS NULL,
	[CancellationDate] [datetime] NULL,
	[RFQLineNumber] [int] NOT NULL,
	[ParPONumber] [int] NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_PO_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_PO_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_PO] PRIMARY KEY CLUSTERED 
(
	[PONumber] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PO] FOREIGN KEY([ParPONumber])
REFERENCES [dbo].[PO] ([PONumber])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PurchasingCurrency] FOREIGN KEY([CurrencyCode])
REFERENCES [dbo].[PurchasingCurrency] ([CurrencyCode])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PurchasingUnit1] FOREIGN KEY([OrderUnitCode])
REFERENCES [dbo].[PurchasingUnit] ([UnitCode])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PurchasingUnit2] FOREIGN KEY([PerUnitCode])
REFERENCES [dbo].[PurchasingUnit] ([UnitCode])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PurchasingUser1] FOREIGN KEY([POUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PurchasingUser2] FOREIGN KEY([SOUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_Purpose] FOREIGN KEY([PurposeCode])
REFERENCES [dbo].[Purpose] ([PurposeCode])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_RFQLine] FOREIGN KEY([RFQLineNumber])
REFERENCES [dbo].[RFQLine] ([RFQLineNumber])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_Supplier1] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Supplier] ([SupplierCode])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_Supplier2] FOREIGN KEY([MakerCode])
REFERENCES [dbo].[Supplier] ([SupplierCode])
GO
ALTER TABLE [dbo].[PO]  WITH CHECK ADD  CONSTRAINT [FK_PO_PurchasingPaymentTerm] FOREIGN KEY([PaymentTermCode])
REFERENCES [dbo].[PurchasingPaymentTerm] ([PaymentTermCode])
GO

/****** オブジェクト:  Index [IX_PO_SOUserID]    スクリプト日付: 04/02/2009 12:02:44 ******/
CREATE NONCLUSTERED INDEX [IX_PO_SOUserID] ON [dbo].[PO] 
(
	[SOUserID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO