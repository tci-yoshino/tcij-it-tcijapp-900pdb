/****** オブジェクト:  View [dbo].[v_POCurrentStatus]    スクリプト日付: 08/28/2008 13:43:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_POCurrentStatus] AS
SELECT
	PH.PONumber,
	PH.POStatusCode AS StatusCode,
	PS.Text,
	PS.SortOrder AS SortOrder,
	PH.StatusChangeDate AS ChangeDate
FROM
	POHistory PH,
	POStatus PS
WHERE
	PH.POHistoryNumber IN (SELECT MAX(POHistoryNumber) FROM POHistory GROUP BY PONumber)
	AND PH.POStatusCode = PS.POStatusCode

GO
