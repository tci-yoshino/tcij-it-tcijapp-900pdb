USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE PurchasingUnit ADD SortOrder int NULL
go
update PurchasingUnit set SortOrder=1 where UnitCode='KG'
update PurchasingUnit set SortOrder=2 where UnitCode='G'
update PurchasingUnit set SortOrder=3 where UnitCode='MG'
update PurchasingUnit set SortOrder=4 where UnitCode='LB'
update PurchasingUnit set SortOrder=5 where UnitCode='L'
update PurchasingUnit set SortOrder=6 where UnitCode='ML'
update PurchasingUnit set SortOrder=7 where UnitCode='PC'
update PurchasingUnit set SortOrder=8 where UnitCode='TON'
update PurchasingUnit set SortOrder=9 where UnitCode='ZZ'