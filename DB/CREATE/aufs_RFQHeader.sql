/****** オブジェクト:  Trigger [aufs_RFQHeader]    スクリプト日付: 08/28/2008 13:50:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Masumi Koyama (TCI)
-- Create date: 2008-05-16
-- Description:
--   * ステータスを N (Create) 以外から A (Assigned) に変更できないようにする。
--   * ステータスが変更された場合に限り RFQHistory を作成する。
-- =============================================
CREATE TRIGGER [aufs_RFQHeader] ON [dbo].[RFQHeader]
   AFTER UPDATE
AS
BEGIN
	DECLARE @RFQNumber int;
	DECLARE @BeforeStatus varchar(5);
	DECLARE @AfterStatus varchar(5);
	DECLARE @LocationCode varchar(5);
	DECLARE @UpdatedBy int;

	DECLARE C_INSERTED CURSOR FOR
		SELECT
			I.RFQNumber,
			D.RFQStatusCode AS BeforeStatus,
			I.RFQStatusCode AS AfterStatus,
			U.LocationCode,
			I.UpdatedBy
		FROM
			inserted AS I,
			deleted AS D,
			s_User AS U
		WHERE
			I.RFQNumber = D.RFQNumber
			AND I.RFQStatusCode <> D.RFQStatusCode
			AND I.UpdatedBy = U.UserID;

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	OPEN C_INSERTED;
	FETCH NEXT FROM C_INSERTED
		INTO @RFQNumber, @BeforeStatus, @AfterStatus, @LocationCode, @UpdatedBy;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@BeforeStatus <> 'N' AND @AfterStatus = 'A')
		BEGIN
			RAISERROR
				(N'RFQStatusCode を N (Create) 以外から A (Assigned) に変更できません。', 11, 1);
		END

		INSERT INTO RFQHistory (
			RFQNumber,
			RFQStatusCode,
			StatusChangeDate,
			Note,
			SendLocationCode,
			SendUserID,
			CreatedBy,
			UpdatedBy
		) VALUES (
			@RFQNumber,
			@AfterStatus,
			GETDATE(),
			'Status Changed.',
			@LocationCode,
			@UpdatedBy,
			@UpdatedBy,
			@UpdatedBy
		);

		FETCH NEXT FROM C_INSERTED
			INTO @RFQNumber, @BeforeStatus, @AfterStatus, @LocationCode, @UpdatedBy;

	END
	CLOSE C_INSERTED;
	DEALLOCATE C_INSERTED;
END

GO
