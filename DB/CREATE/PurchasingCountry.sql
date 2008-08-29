/****** オブジェクト:  Table [dbo].[PurchasingCountry]    スクリプト日付: 08/28/2008 13:36:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchasingCountry](
	[CountryCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL CONSTRAINT [DF_PurchasingCountry_CountryCode]  DEFAULT (NULL),
	[DefaultQuoLocationCode] [varchar](5) COLLATE Japanese_CI_AS NULL CONSTRAINT [DF_PurchasingCountry_DefaultQuoLocationCode]  DEFAULT (NULL),
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_PurchasingCountry_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_PurchasingCountry_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_PurchasingCountry] PRIMARY KEY CLUSTERED 
(
	[CountryCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
