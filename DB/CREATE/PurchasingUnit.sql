/****** オブジェクト:  Table [dbo].[PurchasingUnit]    スクリプト日付: 08/28/2008 13:38:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PurchasingUnit](
	[UnitCode] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
 CONSTRAINT [PK_PurchasingUnit] PRIMARY KEY CLUSTERED 
(
	[UnitCode] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
