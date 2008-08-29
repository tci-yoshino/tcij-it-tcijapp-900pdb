/****** オブジェクト:  View [dbo].[v_PO]    スクリプト日付: 08/28/2008 13:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_PO]
AS
SELECT 
	PO.PONumber,
	PO.R3PONumber,
	PO.R3POLineNumber,
	PO.PODate,
	PO.POLocationCode,
	PL.Name AS POLocationName,
	PO.POUserID,
	PU.Name AS POUserName,
	PO.SOLocationCode,
	SL.Name AS SOLocationName,
	PO.SOUserID,
	SU.Name AS SOUserName,
	PO.ProductID,
	P.ProductNumber,
	ISNULL(P.QuoName, P.Name) AS ProductName, 
	PO.SupplierCode, 
	LTRIM(RTRIM(ISNULL(S.Name3, '') + ' ' + ISNULL(S.Name4, ''))) AS SupplierName, 
	S.R3SupplierCode, 
	LTRIM(RTRIM(ISNULL(S.Name1, '') + ' ' + ISNULL(S.Name2, ''))) AS R3SupplierName, 
	PO.MakerCode, 
	LTRIM(RTRIM(ISNULL(M.Name3, '') + ' ' + ISNULL(M.Name4, ''))) AS MakerName, 
	M.R3SupplierCode AS R3MakerCode, 
	LTRIM(RTRIM(ISNULL(M.Name1, '') + ' ' + ISNULL(M.Name2, ''))) AS R3MakerName,
	PO.OrderQuantity,
	PO.OrderUnitCode,
	PO.DeliveryDate,
	PO.CurrencyCode,
	PO.UnitPrice,
	PO.PerQuantity,
	PO.PerUnitCode,
	PO.PaymentTermCode,
	PT.Text AS PaymentTermText,
	PO.IncotermsCode,
	PO.DeliveryTerm,
	PO.PurposeCode,
	PP.Text AS PurposeText,
	PO.RawMaterialFor,
	PO.RequestedBy,
	PO.SupplierItemNumber,
	PO.SupplierLotNumber,
	PO.DueDate,
	PO.GoodsArrivedDate,
	PO.LotNumber,
	PO.InvoiceReceivedDate,
	PO.ImportCustomClearanceDate,
	PO.QMStartingDate,
	PO.QMFinishDate,
	PO.QMResult,
	PO.RequestQuantity,
	PO.ScheduledExportDate,
	PO.PurchasingRequisitionNumber,
	PO.isCancelled,
	PO.CancellationDate,
	RL.RFQNumber,
	PO.RFQLineNumber,
	PO.ParPONumber,
	PCS.StatusCode,
	PCS.Text AS Status,
	PCS.ChangeDate AS StatusChangeDate,
	PCS.SortOrder AS StatusSortOrder,
	PO.CreatedBy,
	PO.CreateDate,
	PO.UpdatedBy,
	PO.UpdateDate
FROM 
	PO AS PO
		LEFT OUTER JOIN v_User AS SU ON PO.POUserID = SU.UserID 
		LEFT OUTER JOIN Supplier AS M ON PO.MakerCode = M.SupplierCode
		LEFT OUTER JOIN Purpose AS PP ON PO.PurposeCode = PP.PurposeCode 
		LEFT OUTER JOIN s_PaymentTerm AS PT ON PO.PaymentTermCode = PT.PaymentTermCode,
	s_Location AS PL,
	v_User AS PU,
	s_Location AS SL,
	Product AS P,
	Supplier AS S,
	RFQLine AS RL,
	v_POCurrentStatus AS PCS
WHERE 
	PO.POLocationCode = PL.LocationCode AND 
	PO.POUserID = PU.UserID AND 
	PO.SOLocationCode = SL.LocationCode AND 
	PO.ProductID = P.ProductID AND 
	PO.SupplierCode = S.SupplierCode AND 
	PO.RFQLineNumber = RL.RFQLineNumber AND 
	PO.PONumber = PCS.PONumber
GO
