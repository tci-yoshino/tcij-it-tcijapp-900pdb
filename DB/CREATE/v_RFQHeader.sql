/****** オブジェクト:  View [dbo].[v_RFQHeader]    スクリプト日付: 08/28/2008 13:45:01 ******/
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
		LEFT OUTER JOIN s_Location AS QL ON RH.QuoLocationCode = QL.LocationCode
		LEFT OUTER JOIN v_User AS QU ON RH.QuoUserID = QU.UserID
		LEFT OUTER JOIN Supplier AS M ON RH.MakerCode = M.SupplierCode
		LEFT OUTER JOIN Purpose AS PP ON RH.PurposeCode = PP.PurposeCode,
	s_Location AS EL,
	v_User AS EU,
	Product AS P,
	Supplier AS S,
	v_RFQCurrentStatus AS RCS
WHERE
	RH.EnqLocationCode = EL.LocationCode
	AND RH.EnqUserID = EU.UserID
	AND RH.ProductID = P.ProductID
	AND RH.SupplierCode = S.SupplierCode
	AND RH.RFQNumber = RCS.RFQNumber

GO
