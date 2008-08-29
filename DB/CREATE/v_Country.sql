/****** オブジェクト:  View [dbo].[v_Country]    スクリプト日付: 08/28/2008 13:43:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_Country] AS
SELECT
	PC.CountryCode,
	C.Name AS CountryName,
	PC.DefaultQuoLocationCode,
	ISNULL(L.Name, 'Direct') AS DefaultQuoLocationName
FROM
	PurchasingCountry AS PC
		LEFT OUTER JOIN s_Location AS L	ON PC.DefaultQuoLocationCode = L.LocationCode,
	s_Country AS C
WHERE
	PC.CountryCode = C.CountryCode

GO
