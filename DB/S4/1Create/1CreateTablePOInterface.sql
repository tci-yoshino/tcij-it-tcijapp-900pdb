USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[POInterface](
	[ID] [int] NOT NULL,
	[RFQLineNumber] [int] NOT NULL,
	[RFQNumber] [int] NOT NULL,
	[Pattern] [nvarchar](200) NULL,
	[SupplyingPlant] [nvarchar](200) NULL,
	[ReceivingPlant] [nvarchar](200) NULL,
	[PurOrgShipping] [nvarchar](200) NULL,
	[PurOrgReceving] [nvarchar](200) NULL,
	[MaterialNumber] [nvarchar](200) NULL,
	[Vendor] [nvarchar](200) NULL,
	[Price] [nvarchar](200) NULL,
	[PriceUnit] [nvarchar](200) NULL,
	[OrderPriceUnit] [nvarchar](200) NULL,
	[Currency] [nvarchar](200) NULL,
	[RFQReferenceNumber] [nvarchar](200) NULL,
	[SupplierContactPersonCode] [nvarchar](200) NULL,
	[MakerCode] [nvarchar](200) NULL,
	[SupplierItemName] [nvarchar](200) NULL,
	[PaymentTerms] [nvarchar](200) NULL,
	[HandlingFee] [nvarchar](200) NULL,
	[ShipmentCost] [nvarchar](200) NULL,
	[Purpose] [nvarchar](200) NULL,
	[Priority] [nvarchar](200) NULL,
	[EnqUser] [nvarchar](200) NULL,
	[Quouser] [nvarchar](200) NULL,
	[EnqQuantity] [nvarchar](200) NULL,
	[LeadTime] [nvarchar](200) NULL,
	[SupplierItemNumber] [nvarchar](200) NULL,
	[Incoterms] [nvarchar](200) NULL,
	[TermsDelivery] [nvarchar](200) NULL,
	[PurityMethod] [nvarchar](200) NULL,
	[Packing] [nvarchar](200) NULL,
	[SupplyingOfferVaildDateFrom] [nvarchar](200) NULL,
	[SupplyingOfferVaildDateTo] [nvarchar](200) NULL,
	[SupplyingPlantReminding1] [nvarchar](200) NULL,
	[SupplyingPlantReminding2] [nvarchar](200) NULL,
	[SupplyingPlantReminding3] [nvarchar](200) NULL,
	[ReceivingOfferVaildDateFrom] [nvarchar](200) NULL,
	[ReceivingOfferVaildDateTo] [nvarchar](200) NULL,
	[SupplyingStorageLocation] [nvarchar](200) NULL,
	[ReceivingStorageLocation] [nvarchar](200) NULL,
	[SupplierOfferNo] [nvarchar](200) NULL,
 CONSTRAINT [PK_POInterface] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[RFQLineNumber] ASC,
	[RFQNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[POInterface]  WITH CHECK ADD  CONSTRAINT [RFQLineNumber_FK] FOREIGN KEY([RFQLineNumber])
REFERENCES [dbo].[RFQLine] ([RFQLineNumber])
GO
ALTER TABLE [dbo].[POInterface] CHECK CONSTRAINT [RFQLineNumber_FK]
GO
ALTER TABLE [dbo].[POInterface]  WITH CHECK ADD  CONSTRAINT [RFQNumber_FK] FOREIGN KEY([RFQNumber])
REFERENCES [dbo].[RFQHeader] ([RFQNumber])
GO
ALTER TABLE [dbo].[POInterface] CHECK CONSTRAINT [RFQNumber_FK]
GO


