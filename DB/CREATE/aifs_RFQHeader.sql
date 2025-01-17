/****** オブジェクト:  Trigger [aifs_RFQHeader]    スクリプト日付: 08/28/2008 13:49:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Masumi Koyama (TCI)
-- Create date: 2008-05-15
-- Description:	
--   * RFQHistory を作成する。
-- =============================================
CREATE TRIGGER [aifs_RFQHeader] ON [dbo].[RFQHeader] 
   AFTER INSERT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	INSERT INTO RFQHistory (
		RFQNumber,
		RFQStatusCode,
		StatusChangeDate,
		Note,
		SendLocationCode,
		SendUserID,
		CreatedBy,
		UpdatedBy
	)
	SELECT
		I.RFQNumber,
		I.RFQStatusCode,
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

