USE [Purchase]
GO

/****** Object:  View [dbo].[v_RFQHeader]    Script Date: 2019/10/12 19:29:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[v_RFQHeader]
AS
SELECT  RH.RFQNumber, 
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
        ISNULL(P.QuoName, 
        P.Name) AS ProductName, 
        C.isCONFIDENTIAL, 
        RH.SupplierCode, 
		LTRIM(RTRIM(ISNULL(S.Name3, '') + ' ' + ISNULL(S.Name4, ''))) AS SupplierName, 
		S.CountryCode AS SupplierCountryCode, 
        S.Info AS SupplierInfo, 
        RH.SupplierContactPerson, 
        S.R3SupplierCode, 
		S.S4SupplierCode, 
        LTRIM(RTRIM(ISNULL(S.Name1, '') + ' ' + ISNULL(S.Name2, ''))) AS R3SupplierName, 
        RH.MakerCode, 
		LTRIM(RTRIM(ISNULL(M.Name3, '') + ' ' + ISNULL(M.Name4, ''))) AS MakerName, 
        M.CountryCode AS MakerCountryCode, 
		M.Info AS MakerInfo, 
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
        RH.Priority, RH.UpdateDate, 
        RCS.Text AS Status, 
        RCS.SortOrder AS StatusSortOrder, 
		RCS.ChangeDate AS StatusChangeDate, 
        RH.CreateDate, 
        RH.QuoStorageLocation, 
        RH.EnqStorageLocation, 
		RH.SupplierContactPersonSel, 
        RH.SAPMakerCode, 
        P.ProductWarning, 
        S.SupplierWarning, 
		RH.SupplierOfferValidTo
FROM    dbo.RFQHeader AS RH 
INNER JOIN dbo.RFQCurrentStatus AS RCS ON RH.RFQNumber = RCS.RFQNumber 
INNER JOIN dbo.v_UserAll AS EU ON RH.EnqUserID = EU.UserID 
LEFT OUTER JOIN dbo.v_UserAll AS QU ON RH.QuoUserID = QU.UserID 
INNER JOIN dbo.s_Location AS EL ON RH.EnqLocationCode = EL.LocationCode 
LEFT OUTER JOIN dbo.s_Location AS QL ON RH.QuoLocationCode = QL.LocationCode 
LEFT OUTER JOIN dbo.Purpose AS PP ON RH.PurposeCode = PP.PurposeCode 
LEFT OUTER JOIN dbo.Supplier AS M ON RH.MakerCode = M.SupplierCode 
INNER JOIN dbo.Supplier AS S ON RH.SupplierCode = S.SupplierCode 
INNER JOIN dbo.Product AS P ON RH.ProductID = P.ProductID 
INNER JOIN dbo.v_CONFIDENTIAL AS C ON RH.ProductID = C.ProductID
GO


