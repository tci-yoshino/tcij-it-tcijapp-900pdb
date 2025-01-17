/****** オブジェクト:  View [dbo].[v_RFQHeader]    スクリプト日付: 09/24/2008 09:37:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_RFQHeader] AS
SELECT
	RH.RFQNumber,
	RH.EnqLocationCode,
	EL.Name AS EnqLocationName,
	RH.EnqUserID,
	EU.Name AS EnqUserName,
	RH.QuoLocationCode,
	QL.Name AS QuoLocationName,
	RH.QuoUserID,
	QU.Name AS QuoUserName,
	RH.ProductID,
	P.ProductNumber,
	ISNULL(P.QuoName, P.Name) AS ProductName,
	RH.SupplierCode,
	LTRIM(RTRIM(ISNULL(S.Name3, '') + ' ' + ISNULL(S.Name4, ''))) AS SupplierName,
	S.CountryCode AS SupplierCountryCode,
	RH.SupplierContactPerson,
	S.R3SupplierCode,
	LTRIM(RTRIM(ISNULL(S.Name1, '') + ' ' + ISNULL(S.Name2, ''))) AS R3SupplierName,
	RH.MakerCode,
	LTRIM(RTRIM(ISNULL(M.Name3, '') + ' ' + ISNULL(M.Name4, ''))) AS MakerName,
	M.CountryCode AS MakerCountryCode,
	M.R3SupplierCode AS R3MakerCode,
	LTRIM(RTRIM(ISNULL(M.Name1, '') + ' ' + ISNULL(M.Name2, ''))) AS R3MakerName,
	RH.PaymentTermCode,
	RH.RequiredPurity,
	RH.RequiredQMMethod,
	RH.RequiredSpecification,
	RH.SpecSheet,
	RH.Specification,
	RH.PurposeCode,
	PP.Text AS Purpose,
	RH.SupplierItemName,
	RH.ShippingHandlingFee,
	RH.ShippingHandlingCurrencyCode,
	RH.Comment,
	RH.QuotedDate,
	RH.RFQStatusCode AS StatusCode,
	RH.UpdateDate,
	RCS.Text AS Status,
	RCS.SortOrder AS StatusSortOrder,
	RCS.ChangeDate AS StatusChangeDate
FROM
	RFQHeader AS RH 
		INNER JOIN RFQCurrentStatus AS RCS ON RH.RFQNumber = RCS.RFQNumber 
		INNER JOIN v_User AS EU ON RH.EnqUserID = EU.UserID 
		LEFT OUTER JOIN v_User AS QU ON RH.QuoUserID = QU.UserID 
		INNER JOIN s_Location AS EL ON RH.EnqLocationCode = EL.LocationCode 
		LEFT OUTER JOIN s_Location AS QL ON RH.QuoLocationCode = QL.LocationCode 
		LEFT OUTER JOIN Purpose AS PP ON RH.PurposeCode = PP.PurposeCode 
		LEFT OUTER JOIN Supplier AS M ON RH.MakerCode = M.SupplierCode 
		INNER JOIN Supplier AS S ON RH.SupplierCode = S.SupplierCode 
		INNER JOIN Product AS P ON RH.ProductID = P.ProductID 

GO
