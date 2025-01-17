/****** オブジェクト:  Trigger [aufs_PO]    スクリプト日付: 08/28/2008 13:51:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Masumi Koyama (TCI)
-- Create date: 2008-08-07
-- Description: 
--   * ステータスが変更された場合に限り POHistory を作成する。
-- =============================================
CREATE TRIGGER [aufs_PO] ON [dbo].[PO] 
   AFTER UPDATE
AS
BEGIN
	DECLARE @PONumber int;
	DECLARE @LocationCode varchar(5);
	DECLARE @UpdatedBy int;
	DECLARE @ChiPOLocationCode varchar(5);
	DECLARE @ChiPOUserID int;
	DECLARE @ParPONumber int;
	DECLARE @ParPOLocationCode varchar(5);
	DECLARE @ParPOUserID int;
	DECLARE @BeforeStatus varchar(5);
	DECLARE @AfterStatus varchar(5);

	DECLARE C_INSERTED CURSOR FOR
		SELECT
			I.PONumber,
			U.LocationCode,
			I.UpdatedBy,
			I.SOLocationCode AS ChiPOLocationCode,
			I.SOUserID AS ChiPOUserID,
			I.ParPONumber,
			P.POLocationCode AS ParPOLocationCode,
			P.POUserID AS ParPOUserID
		FROM
			inserted AS I LEFT OUTER JOIN PO AS P ON P.PONumber = I.ParPONumber,
			s_User AS U
		WHERE
			I.UpdatedBy = U.UserID;

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	OPEN C_INSERTED;
	FETCH NEXT FROM C_INSERTED
		INTO @PONumber, @LocationCode, @UpdatedBy, @ChiPOLocationCode, @ChiPOUserID, @ParPONumber, @ParPOLocationCode, @ParPOUserID;

	WHILE @@FETCH_STATUS = 0
	BEGIN

		IF (@ParPONumber IS NOT NULL)
		BEGIN
			SET @BeforeStatus =
				(SELECT POStatus =
					CASE
						WHEN CP.CancellationDate IS NOT NULL THEN 'CPC'
						WHEN PP.CancellationDate IS NOT NULL THEN 'PPC'
						--WHEN PP.ScheduledExportDate IS NOT NULL THEN 'PED'
						WHEN PP.QMFinishDate IS NOT NULL THEN 'PQF'
						WHEN PP.QMStartingDate IS NOT NULL THEN 'PQS'
						WHEN PP.GoodsArrivedDate IS NOT NULL THEN 'PGA'
						WHEN PP.ImportCustomClearanceDate IS NOT NULL THEN 'PCC'
						WHEN PP.InvoiceReceivedDate IS NOT NULL THEN 'PIR'
						WHEN CP.ScheduledExportDate IS NOT NULL THEN 'CED'
						WHEN CP.QMFinishDate IS NOT NULL THEN 'CQF'
						WHEN CP.QMStartingDate IS NOT NULL THEN 'CQS'
						WHEN CP.GoodsArrivedDate IS NOT NULL THEN 'CGA'
						WHEN CP.ImportCustomClearanceDate IS NOT NULL THEN 'CCC'
						WHEN CP.InvoiceReceivedDate IS NOT NULL THEN 'CIR'
						WHEN CP.PODate IS NOT NULL THEN 'CPI'
						WHEN PP.PODate IS NOT NULL THEN 'PPI'
						ELSE '' END
				FROM
					deleted AS CP,
					PO AS PP
				WHERE
					CP.ParPONumber = PP.PONumber);

			SET @AfterStatus =
				(SELECT POStatus =
					CASE
						WHEN CP.CancellationDate IS NOT NULL THEN 'CPC'
						WHEN PP.CancellationDate IS NOT NULL THEN 'PPC'
						--WHEN PP.ScheduledExportDate IS NOT NULL THEN 'PED'
						WHEN PP.QMFinishDate IS NOT NULL THEN 'PQF'
						WHEN PP.QMStartingDate IS NOT NULL THEN 'PQS'
						WHEN PP.GoodsArrivedDate IS NOT NULL THEN 'PGA'
						WHEN PP.ImportCustomClearanceDate IS NOT NULL THEN 'PCC'
						WHEN PP.InvoiceReceivedDate IS NOT NULL THEN 'PIR'
						WHEN CP.ScheduledExportDate IS NOT NULL THEN 'CED'
						WHEN CP.QMFinishDate IS NOT NULL THEN 'CQF'
						WHEN CP.QMStartingDate IS NOT NULL THEN 'CQS'
						WHEN CP.GoodsArrivedDate IS NOT NULL THEN 'CGA'
						WHEN CP.ImportCustomClearanceDate IS NOT NULL THEN 'CCC'
						WHEN CP.InvoiceReceivedDate IS NOT NULL THEN 'CIR'
						WHEN CP.PODate IS NOT NULL THEN 'CPI'
						WHEN PP.PODate IS NOT NULL THEN 'PPI'
						ELSE '' END
				FROM
					inserted AS CP,
					PO AS PP
				WHERE
					CP.ParPONumber = PP.PONumber);
		END
		ELSE
		BEGIN
			SET @BeforeStatus =
				(SELECT POStatus =
					CASE
						WHEN CP.CancellationDate IS NOT NULL THEN 'CPC'
						WHEN PP.CancellationDate IS NOT NULL THEN 'PPC'
						--WHEN PP.ScheduledExportDate IS NOT NULL THEN 'PED'
						WHEN PP.QMFinishDate IS NOT NULL THEN 'PQF'
						WHEN PP.QMStartingDate IS NOT NULL THEN 'PQS'
						WHEN PP.GoodsArrivedDate IS NOT NULL THEN 'PGA'
						WHEN PP.ImportCustomClearanceDate IS NOT NULL THEN 'PCC'
						WHEN PP.InvoiceReceivedDate IS NOT NULL THEN 'PIR'
						WHEN CP.ScheduledExportDate IS NOT NULL THEN 'CED'
						WHEN CP.QMFinishDate IS NOT NULL THEN 'CQF'
						WHEN CP.QMStartingDate IS NOT NULL THEN 'CQS'
						WHEN CP.GoodsArrivedDate IS NOT NULL THEN 'CGA'
						WHEN CP.ImportCustomClearanceDate IS NOT NULL THEN 'CCC'
						WHEN CP.InvoiceReceivedDate IS NOT NULL THEN 'CIR'
						WHEN CP.PODate IS NOT NULL THEN 'CPI'
						WHEN PP.PODate IS NOT NULL THEN 'PPI'
						ELSE '' END
				FROM
					deleted AS PP LEFT OUTER JOIN PO AS CP ON CP.ParPONumber = PP.PONumber);

			SET @AfterStatus =
				(SELECT POStatus =
					CASE
						WHEN CP.CancellationDate IS NOT NULL THEN 'CPC'
						WHEN PP.CancellationDate IS NOT NULL THEN 'PPC'
						--WHEN PP.ScheduledExportDate IS NOT NULL THEN 'PED'
						WHEN PP.QMFinishDate IS NOT NULL THEN 'PQF'
						WHEN PP.QMStartingDate IS NOT NULL THEN 'PQS'
						WHEN PP.GoodsArrivedDate IS NOT NULL THEN 'PGA'
						WHEN PP.ImportCustomClearanceDate IS NOT NULL THEN 'PCC'
						WHEN PP.InvoiceReceivedDate IS NOT NULL THEN 'PIR'
						WHEN CP.ScheduledExportDate IS NOT NULL THEN 'CED'
						WHEN CP.QMFinishDate IS NOT NULL THEN 'CQF'
						WHEN CP.QMStartingDate IS NOT NULL THEN 'CQS'
						WHEN CP.GoodsArrivedDate IS NOT NULL THEN 'CGA'
						WHEN CP.ImportCustomClearanceDate IS NOT NULL THEN 'CCC'
						WHEN CP.InvoiceReceivedDate IS NOT NULL THEN 'CIR'
						WHEN CP.PODate IS NOT NULL THEN 'CPI'
						WHEN PP.PODate IS NOT NULL THEN 'PPI'
						ELSE '' END
				FROM
					inserted AS PP LEFT OUTER JOIN PO AS CP ON CP.ParPONumber = PP.PONumber);
		END

		IF (@BeforeStatus <> @AfterStatus)
		BEGIN
			INSERT INTO POHistory (
				PONumber,
				POStatusCode,
				StatusChangeDate,
				Note,
				SendLocationCode,
				SendUserID,
				RcptLocationCode,
				RcptUserID,
				CreatedBy,
				UpdatedBy
			) VALUES (
				CASE WHEN @ParPONumber IS NULL THEN @PONumber
					ELSE @ParPONumber END,
				@AfterStatus,
				GETDATE(),
				'Status Changed.',
				@LocationCode,
				@UpdatedBy,
				CASE WHEN @AfterStatus IN ('CGA', 'CQS', 'CQF', 'CED') THEN @ParPOLocationCode
					WHEN @AfterStatus = 'PPC' THEN @ChiPOLocationCode
					ELSE NULL END,
				CASE WHEN @AfterStatus IN ('CGA', 'CQS', 'CQF', 'CED') THEN @ParPOUserID
					WHEN @AfterStatus = 'PPC' THEN @ChiPOUserID
					ELSE NULL END,
				@UpdatedBy,
				@UpdatedBy
			);
		END

		FETCH NEXT FROM C_INSERTED
			INTO @PONumber, @LocationCode, @UpdatedBy, @ChiPOLocationCode, @ChiPOUserID, @ParPONumber, @ParPOLocationCode, @ParPOUserID;

	END
	CLOSE C_INSERTED;
	DEALLOCATE C_INSERTED;
END

GO
