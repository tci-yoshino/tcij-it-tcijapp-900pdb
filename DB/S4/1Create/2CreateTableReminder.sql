USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reminder](
	[SupplyingPlant] [nvarchar](200) NOT NULL,
	[FirstRem] [nvarchar](200) NULL,
	[SecondRem] [nvarchar](200) NULL,
	[ThirdRem] [nvarchar](200) NULL,
	CONSTRAINT [PK_Reminder] PRIMARY KEY CLUSTERED ([SupplyingPlant] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO [dbo].[Reminder] (
	[SupplyingPlant], 
	[FirstRem] , 
	[SecondRem], 
	[ThirdRem]
)
VALUES
	('AP10','1','',''),
	('AP20','1','',''),
	('CP10','-14','0','7'),
	('CP20','-14','0','7'),
	('NP10','-7','-3','5'),
	('NP20','-7','-3','5'),
	('EP10','-1','3','7'),
	('FP10','-1','3','7'),
	('GP10','-1','3','7'),
	('HP10','cal','cal','cal'),
	('HP30','cal','cal','cal')