USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE PurchasingCurrency ADD SortOrder int NULL
go
update PurchasingCurrency  set SortOrder=1 where CurrencyCode='JPY'
update PurchasingCurrency  set SortOrder=16 where CurrencyCode='RMB'
update PurchasingCurrency  set SortOrder=3 where CurrencyCode='USD'
update PurchasingCurrency  set SortOrder=4 where CurrencyCode='INR'
update PurchasingCurrency  set SortOrder=5 where CurrencyCode='EUR'
update PurchasingCurrency  set SortOrder=6 where CurrencyCode='GBP'
update PurchasingCurrency  set SortOrder=7 where CurrencyCode='CHF'
update PurchasingCurrency  set SortOrder=8 where CurrencyCode='BEF'
update PurchasingCurrency  set SortOrder=9 where CurrencyCode='CAD'
update PurchasingCurrency  set SortOrder=10 where CurrencyCode='DEM'
update PurchasingCurrency  set SortOrder=11 where CurrencyCode='DKK'
update PurchasingCurrency  set SortOrder=12 where CurrencyCode='FRF'
update PurchasingCurrency  set SortOrder=13 where CurrencyCode='NLG'
update PurchasingCurrency  set SortOrder=14 where CurrencyCode='NOK'
update PurchasingCurrency  set SortOrder=15 where CurrencyCode='SEK'
insert into PurchasingCurrency(SortOrder,CurrencyCode)values(2,'CNY')
