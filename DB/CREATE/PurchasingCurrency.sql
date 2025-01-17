/****** オブジェクト:  Table [dbo].[PurchasingCurrency]    スクリプト日付: 08/28/2008 13:37:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchasingCurrency](
	[CurrencyCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
 CONSTRAINT [PK_PurchasingCurrency] PRIMARY KEY CLUSTERED 
(
	[CurrencyCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
