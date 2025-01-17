/****** オブジェクト:  Table [dbo].[Product]    スクリプト日付: 08/28/2008 13:36:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductNumber] [varchar](32) COLLATE Japanese_CI_AS NOT NULL,
	[NumberType] [varchar](5) COLLATE Japanese_CI_AS NOT NULL,
	[Name] [nvarchar](1000) COLLATE Japanese_CI_AS NULL,
	[QuoName] [nvarchar](1000) COLLATE Japanese_CI_AS NULL,
	[JapaneseName] [nvarchar](1000) COLLATE Japanese_CI_AS NULL,
	[ChineseName] [nvarchar](1000) COLLATE Japanese_CI_AS NULL,
	[CASNumber] [varchar](32) COLLATE Japanese_CI_AS NULL,
	[MolecularFormula] [varchar](128) COLLATE Japanese_CI_AS NULL,
	[Status] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[ProposalDept] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[ProcumentDept] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[PD] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[Reference] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[Comment] [nvarchar](3000) COLLATE Japanese_CI_AS NULL,
	[CreatedBy] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_Product_CreateDate]  DEFAULT (getdate()),
	[UpdatedBy] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL CONSTRAINT [DF_Product_UpdateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CK_Product_CASNumber] CHECK  ((case when [NumberType]='CAS' then case when [ProductNumber]=[CASNumber] then (1) else (0) end else (1) end=(1)))
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [CK_Product_NumberType] CHECK  (([NumberType]='CAS' OR [NumberType]='TCI' OR [NumberType]='NEW'))
GO

/****** オブジェクト:  Index [IX_Product_ProductNumber]    スクリプト日付: 09/01/2008 15:25:54 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_ProductNumber] ON [dbo].[Product] 
(
	[ProductNumber] ASC
)WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]

GO