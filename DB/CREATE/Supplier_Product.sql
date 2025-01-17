/****** オブジェクト:  Table [dbo].[Supplier_Product]    スクリプト日付: 08/28/2008 13:42:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier_Product](
	[SupplierCode] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[SupplierItemNumber] [nvarchar](128) COLLATE Japanese_CI_AS NULL,
	[Note] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Supplier_Product_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Supplier_Product_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Supplier_Product] PRIMARY KEY CLUSTERED 
(
	[SupplierCode] ASC,
	[ProductID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Supplier_Product]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_Product_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Supplier_Product]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_Product_Supplier] FOREIGN KEY([SupplierCode])
REFERENCES [dbo].[Supplier] ([SupplierCode])
GO
