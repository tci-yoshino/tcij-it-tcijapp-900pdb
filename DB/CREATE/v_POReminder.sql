/****** オブジェクト:  View [dbo].[v_POReminder]    スクリプト日付: 08/28/2008 13:44:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_POReminder] AS
SELECT
	PH.POHistoryNumber,
	PH.PONumber,
	ISNULL(PC.Text, PS.Text) AS POCorres,
	PH.RcptLocationCode,
	RL.Name AS RcptLocationName,
	PH.RcptUserID,
	RU.Name AS RcptUserName
FROM
	POHistory PH LEFT OUTER JOIN POCorres PC ON PH.POCorresCode = PC.POCorresCode,
	POStatus PS,
	v_User AS RU,
	s_Location AS RL
WHERE
	PH.POHistoryNumber IN (
		SELECT
			MAX(POHistoryNumber)
		FROM
			POHistory
		WHERE
			isChecked = 0
			AND POCorresCode != 'NS' /* Note for Self は対象外 */
			AND RcptUserID IS NOT NULL
		GROUP BY PONumber)
	AND PH.RcptLocationCode = RL.LocationCode
	AND PH.RcptUserID = RU.UserID
	AND PH.POStatusCode = PS.POStatusCode

GO
