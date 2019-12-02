USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageLocation](
	[Plant] [varchar](5) NOT NULL,
	[Storage] [varchar](5) NOT NULL,
	[Description] [nvarchar](200) NULL
	CONSTRAINT [PK_StorageLocation] PRIMARY KEY CLUSTERED ([Storage] ASC) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO [dbo].[StorageLocation] (
	[Plant],
    [Storage] ,
    [Description]
)
VALUES
	('AP10' , 'AL10' , 'Fukaya Factory'),
	('AP10' , 'AL11' , 'Kumagaya Factory'),
	('AP20' , 'AL20' , 'Tsukuba Repac'),
	('AP40' , 'AL40' , 'Oji R&D'),
	('AP50' , 'AL50' , 'Toda R&D'),
	('CP10' , 'CL10' , 'EWM: Common SLOC'),
	('CP10' , 'CL20' , 'ORC Lab'),
	('CP10' , 'CL30' , 'Offsite material'),
	('CP10' , 'CL40' , 'EWM: Common SLOC'),
	('CP20' , 'CL70' , 'EWM: Common SLOC'),
	('EP10' , 'EL10' , 'EWM: Bonded SLOC'),
	('EP10' , 'EL20' , 'EWM: Common SLOC'),
	('HP10' , 'HL10' , 'EWM: Common SLOC'),
	('HP10' , 'HL30' , 'EWM: Procurement'),
	('NP10' , 'NL10' , 'EWM: Bonded SLOC'),
	('NP20' , 'NL20' , 'EWM: Common SLOC'),
	('HP30','HL50','EWM: Procurement')
