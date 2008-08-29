/****** �I�u�W�F�N�g:  View [dbo].[v_RFQReminder]    �X�N���v�g���t: 08/28/2008 13:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_RFQReminder] AS
SELECT
	RH.RFQHistoryNumber,
	RH.RFQNumber,
	RC.Text AS RFQCorres,
	RH.RcptLocationCode,
	RL.Name AS RcptLocationName,
	RH.RcptUserID,
	RU.Name AS RcptUserName
FROM
	RFQHistory AS RH,
    RFQCorres AS RC,
	v_User AS RU,
	s_Location AS RL
WHERE
	RH.RFQHistoryNumber IN (
		SELECT
			MAX(RFQHistoryNumber)
		FROM
			RFQHistory
		WHERE
			isChecked = 0
			AND RFQCorresCode != 'NS' /* Note for Self �͑ΏۊO */
			AND RcptUserID IS NOT NULL
		GROUP BY RFQNumber)
	AND RH.RFQCorresCode = RC.RFQCorresCode
	AND RH.RcptLocationCode = RL.LocationCode
	AND RH.RcptUserID = RU.UserID

GO
