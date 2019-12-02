USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE Purpose ADD IsVisiable bit default (0)  not NULL
go
INSERT INTO Purpose(
	[PurposeCode],
    [Text] ,
    [SortOrder] ,
    [IsVisiable]) 
VALUES
	('10','For Global Restock',10,1),
	('11','For Global Bulk customer',14,1),
	('12','For Global Prod. RM',11,1),
	('13','For Global New',15,1),
	('14','For Global Reference',16,1),
	('15','For Global One Time order',17,1),
	('16','For Global Supplier Development',18,1),
	('17','For Global OEM Purification',19,1),
	('18','For Global OEM Production',20,1),
	('30','For Local Restock',12,1),
	('31','For Local Bulk customer',21,1),
	('32','For Local Reference',22,1),
	('33','For Local Prod. RM',13,1),
	('34','For Local Supplier Development',23,1)
