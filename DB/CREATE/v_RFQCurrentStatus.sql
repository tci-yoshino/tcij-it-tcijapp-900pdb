/****** オブジェクト:  View [dbo].[v_RFQCurrentStatus]    スクリプト日付: 08/28/2008 13:44:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_RFQCurrentStatus] AS
SELECT
	RH.RFQNumber,
	RH.RFQStatusCode AS StatusCode,
	RS.Text,
	RS.SortOrder AS SortOrder,
	RH.StatusChangeDate AS ChangeDate
FROM
	RFQHistory RH,
	RFQStatus RS
WHERE
	RH.RFQHistoryNumber IN (SELECT MAX(RFQHistoryNumber) FROM RFQHistory GROUP BY RFQNumber)
	AND RH.RFQStatusCode = RS.RFQStatusCode

GO
