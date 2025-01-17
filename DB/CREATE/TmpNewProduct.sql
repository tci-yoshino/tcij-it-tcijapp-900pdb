/****** オブジェクト:  Table [dbo].[TmpNewProduct]    スクリプト日付: 08/28/2008 13:42:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TmpNewProduct](
	[ProductNumber] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[NewProductRegNumber] [varchar](32) COLLATE Japanese_CI_AS NULL,
	[ProductName] [nvarchar](1000) COLLATE Japanese_CI_AS NULL,
	[CASNumber] [varchar](32) COLLATE Japanese_CI_AS NULL,
	[Status] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[ProposalDept] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[ProcumentDept] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[PD] [nvarchar](50) COLLATE Japanese_CI_AS NULL,
	[MolecularFormula] [varchar](128) COLLATE Japanese_CI_AS NULL,
	[ImportStatus] [varchar](5) COLLATE Japanese_CI_AS NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_TmpNewProduct_CreateDate]  DEFAULT (getdate())
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
