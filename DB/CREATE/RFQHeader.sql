/****** オブジェクト:  Table [dbo].[RFQHeader]    スクリプト日付: 08/28/2008 13:39:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RFQHeader](
	[RFQNumber] [int] IDENTITY(1000000000,1) NOT NULL,
	[EnqLocationCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[EnqUserID] [int] NOT NULL,
	[QuoLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[QuoUserID] [int] NULL,
	[ProductID] [int] NOT NULL,
	[SupplierCode] [int] NOT NULL,
	[MakerCode] [int] NULL,
	[PurposeCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[RequiredPurity] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[RequiredQMMethod] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[RequiredSpecification] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SpecSheet] [bit] NOT NULL CONSTRAINT [DF_RFQHeader_SpecSheet]  DEFAULT ((0)),
	[Specification] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SupplierContactPerson] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SupplierItemName] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[ShippingHandlingFee] [decimal](13, 3) NULL,
	[ShippingHandlingCurrencyCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[PaymentTermCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[Comment] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[QuotedDate] [datetime] NULL,
	[RFQStatusCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_RFQHeader_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_RFQHeader_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_RFQHeader] PRIMARY KEY CLUSTERED 
(
	[RFQNumber] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_PurchasingCurrency] FOREIGN KEY([ShippingHandlingCurrencyCode])
REFERENCES [dbo].[PurchasingCurrency] ([CurrencyCode])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_PurchasingUser1] FOREIGN KEY([EnqUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_PurchasingUser2] FOREIGN KEY([QuoUserID])
REFERENCES [dbo].[PurchasingUser] ([UserID])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_Purpose] FOREIGN KEY([PurposeCode])
REFERENCES [dbo].[Purpose] ([PurposeCode])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_RFQStatus] FOREIGN KEY([RFQStatusCode])
REFERENCES [dbo].[RFQStatus] ([RFQStatusCode])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_Supplier1] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Supplier] ([SupplierCode])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_Supplier2] FOREIGN KEY([MakerCode])
REFERENCES [dbo].[Supplier] ([SupplierCode])
GO
ALTER TABLE [dbo].[RFQHeader]  WITH CHECK ADD  CONSTRAINT [FK_RFQHeader_PurchasingPaymentTerm] FOREIGN KEY([PaymentTermCode])
REFERENCES [dbo].[PurchasingPaymentTerm] ([PaymentTermCode])
GO

/****** オブジェクト:  Index [IX_RFQHeader_EnqUserID]    スクリプト日付: 04/02/2009 11:58:03 ******/
CREATE NONCLUSTERED INDEX [IX_RFQHeader_EnqUserID] ON [dbo].[RFQHeader] 
(
	[EnqUserID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** オブジェクト:  Index [IX_RFQHeader_QuoUserID]    スクリプト日付: 04/02/2009 11:58:03 ******/
CREATE NONCLUSTERED INDEX [IX_RFQHeader_QuoUserID] ON [dbo].[RFQHeader] 
(
	[QuoUserID] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]
GO

