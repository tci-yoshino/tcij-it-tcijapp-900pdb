/****** オブジェクト:  Trigger [aifs_PO]    スクリプト日付: 08/28/2008 13:51:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Masumi Koyama (TCI)
-- Create date: 2008-08-07
-- Description:	
--   * POHistory を作成する。
-- =============================================
CREATE TRIGGER [aifs_PO] ON [dbo].[PO] 
   AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	INSERT INTO POHistory (
		PONumber,
		POStatusCode,
		StatusChangeDate,
		Note,
		SendLocationCode,
		SendUserID,
		CreatedBy,
		UpdatedBy
	)
	SELECT
		PONumber =
			CASE WHEN I.ParPONumber IS NULL THEN I.PONumber
				ELSE I.ParPONumber END,
		POStatusCode =
			CASE WHEN I.ParPONumber IS NULL THEN 'PPI'
				ELSE 'CPI' END,
		GETDATE(),
		'New Issued.',
		U.LocationCode,
		I.CreatedBy,
		I.CreatedBy,
		I.CreatedBy
	FROM
		inserted AS I,
		s_User AS U
	WHERE
		I.CreatedBy = U.UserID
END

GO
