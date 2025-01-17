/****** オブジェクト:  Trigger [dbo].[aifs_RFQHistory]    スクリプト日付: 08/10/2009 13:56:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[aifs_RFQHistory]
   ON  [dbo].[RFQHistory]
   AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;

		UPDATE 
			RFQCurrentStatus 
		SET
			RFQNumber=RH.RFQNumber,
			StatusCode=RH.RFQStatusCode,
			"Text"=RS.Text,
			SortOrder=RS.SortOrder,
			ChangeDate=RH.StatusChangeDate
		FROM
			inserted AS RH,
			RFQStatus AS RS
		WHERE
			RH.RFQStatusCode = RS.RFQStatusCode AND
			RH.RFQHistoryNumber IN (SELECT MAX(RFQHistoryNumber) FROM inserted GROUP BY RFQNumber) AND
			RH.RFQNumber = RFQCurrentStatus.RFQNumber

		INSERT INTO 
			RFQCurrentStatus 
		SELECT 
			RH.RFQNumber,
			RH.RFQStatusCode,
			RS.Text,
			RS.SortOrder,
			RH.StatusChangeDate
		FROM
			inserted AS RH,
			RFQStatus AS RS
		WHERE
			RH.RFQStatusCode = RS.RFQStatusCode AND
			RH.RFQHistoryNumber IN (SELECT MAX(RFQHistoryNumber) FROM inserted GROUP BY RFQNumber) AND
			RH.RFQNumber NOT IN (SELECT RFQNumber FROM RFQCurrentStatus)

END

GO

