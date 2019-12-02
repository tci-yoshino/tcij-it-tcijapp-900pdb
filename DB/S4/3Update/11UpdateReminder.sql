USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
INSERT INTO Reminder(
	SupplyingPlant,
    FirstRem,
    SecondRem,
    ThirdRem
)
VALUES
	('AP40','1','0','0'),
	('AP50','1','0','0')