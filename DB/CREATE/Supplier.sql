/****** オブジェクト:  Table [dbo].[Supplier]    スクリプト日付: 08/28/2008 13:41:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierCode] [int] IDENTITY(1,1) NOT NULL,
	[R3SupplierCode] [nvarchar](10) COLLATE Japanese_CI_AS NULL,
	[Name1] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Name2] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Name3] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Name4] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SearchTerm1] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[SearchTerm2] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Address1] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Address2] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[Address3] [nvarchar](255) COLLATE Japanese_CI_AS NULL,
	[PostalCode] [varchar](32) COLLATE Japanese_CI_AS NULL,
	[CountryCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[RegionCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[Telephone] [varchar](32) COLLATE Japanese_CI_AS NULL,
	[Fax] [varchar](32) COLLATE Japanese_CI_AS NULL,
	[Email] [varchar](255) COLLATE Japanese_CI_AS NULL,
	[Comment] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[Website] [varchar](255) COLLATE Japanese_CI_AS NULL,
	[Note] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[LocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[isDisabled] [bit] NOT NULL CONSTRAINT [DF_Supplier_isDisabled]  DEFAULT ((0)),
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Supplier_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Supplier_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Supplier]  WITH CHECK ADD  CONSTRAINT [FK_Supplier_PurchasingCountry] FOREIGN KEY([CountryCode])
REFERENCES [dbo].[PurchasingCountry] ([CountryCode])
GO
