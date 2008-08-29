IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_CompetitorPrice')
DROP SYNONYM [dbo].[s_CompetitorPrice]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Country')
DROP SYNONYM [dbo].[s_Country]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Currency')
DROP SYNONYM [dbo].[s_Currency]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_EhsPhrase')
DROP SYNONYM [dbo].[s_EhsPhrase]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Incoterms')
DROP SYNONYM [dbo].[s_Incoterms]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Location')
DROP SYNONYM [dbo].[s_Location]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_PaymentTerm')
DROP SYNONYM [dbo].[s_PaymentTerm]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_ProductName')
DROP SYNONYM [dbo].[s_ProductName]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Region')
DROP SYNONYM [dbo].[s_Region]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Supplier')
DROP SYNONYM [dbo].[s_Supplier]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_Unit')
DROP SYNONYM [dbo].[s_Unit]
GO
IF  EXISTS (SELECT * FROM sys.synonyms WHERE name = N's_User')
DROP SYNONYM [dbo].[s_User]
GO
