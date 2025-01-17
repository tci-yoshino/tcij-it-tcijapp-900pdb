
/****** オブジェクト:  Table [dbo].[PurchasingPaymentTerm]    スクリプト日付: 08/28/2008 13:37:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchasingPaymentTerm](
	[PaymentTermCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Text] [nvarchar](128) COLLATE Japanese_CI_AS NOT NULL,
 CONSTRAINT [PK_PurchasingPaymentTerm] PRIMARY KEY CLUSTERED 
(
	[PaymentTermCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
