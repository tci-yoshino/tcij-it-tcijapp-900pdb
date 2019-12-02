USE [Purchase]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[v_RFQLine] AS
SELECT
	RH.RFQNumber,
	RL.RFQLineNumber,
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
	C.isCONFIDENTIAL,
	RH.SupplierCode,
	LTRIM(RTRIM(ISNULL(S.Name3, '') + ' ' + ISNULL(S.Name4, ''))) AS SupplierName,
	S.R3SupplierCode AS R3SupplierCode,
	LTRIM(RTRIM(ISNULL(S.Name1, '') + ' ' + ISNULL(S.Name2, ''))) AS R3SupplierName,
	RH.MakerCode,
	LTRIM(RTRIM(ISNULL(M.Name3, '') + ' ' + ISNULL(M.Name4, ''))) AS MakerName,
	M.R3SupplierCode AS R3MakerCode,
	LTRIM(RTRIM(ISNULL(M.Name1, '') + ' ' + ISNULL(M.Name2, ''))) AS R3MakerName,
	RH.PaymentTermCode,
	PT.Text AS PaymentTerm,
	RH.PurposeCode,
	PP.Text AS Purpose,
	RH.SupplierItemName,
	RH.ShippingHandlingFee,
	RH.ShippingHandlingCurrencyCode,
	RH.Comment,
	RH.QuotedDate,
	RH.RFQStatusCode AS StatusCode,
	RL.EnqQuantity,
	RL.EnqUnitCode,
	RL.EnqPiece,
	RL.CurrencyCode,
	RL.UnitPrice,
	RL.QuoPer,
	RL.QuoUnitCode,
	RL.LeadTime,
	RL.SupplierItemNumber,
	RL.IncotermsCode,
	I.[Text] AS Incoterms,
	RL.DeliveryTerm,
	RL.Packing,
	RL.Purity,
	RL.QMMethod,
	RL.SupplierOfferNo,
	RL.NoOfferReasonCode,
	RL.OutputStatus,
	NOR.Text AS NoOfferReason
FROM
	RFQHeader AS RH
		LEFT OUTER JOIN s_Location AS QL ON RH.QuoLocationCode = QL.LocationCode
		LEFT OUTER JOIN v_UserAll AS QU ON RH.QuoUserID = QU.UserID
		LEFT OUTER JOIN Supplier AS M ON RH.MakerCode = M.SupplierCode
		LEFT OUTER JOIN Purpose AS PP ON RH.PurposeCode = PP.PurposeCode
		LEFT OUTER JOIN PurchasingPaymentTerm AS PT ON RH.PaymentTermCode = PT.PaymentTermCode,
	s_Location AS EL,
	v_UserAll AS EU,
	Product AS P,
	Supplier AS S,
	RFQLine AS RL
		LEFT OUTER JOIN NoOfferReason AS NOR ON RL.NoOfferReasonCode = NOR.NoOfferReasonCode
		LEFT OUTER JOIN s_Incoterms AS I ON RL.IncotermsCode = I.IncotermsCode,
	v_CONFIDENTIAL AS C
WHERE
	RH.EnqLocationCode = EL.LocationCode
	AND RH.EnqUserID = EU.UserID
	AND RH.ProductID = P.ProductID
	AND RH.SupplierCode = S.SupplierCode
	AND RH.RFQNumber = RL.RFQNumber
	AND C.ProductID = RH.ProductID
GO


