/****** オブジェクト:  StoredProcedure [dbo].[sp_ProductNameImport]    スクリプト日付: 02/27/2009 14:18:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		akutsu
-- Create date: 2008/07/02
-- Description:	Import Product name
-- =============================================
CREATE PROCEDURE [dbo].[sp_ProductNameImport]
-- errorr message parameter
@ErrNUMBER int output,
@ErrMESSAGE nvarchar(2000) output,
@ErrSTATE int output,
@ErrLINE int output
AS

BEGIN TRY
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION ON;
--  ALTER DATABASE s_ProductName SET ALLOW_SNAPSHOT_ISOLATION ON;
--  SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
  BEGIN TRANSACTION;
  SET NOCOUNT OFF;

---- mainproccess

/* TciMaster.Hinmei ではなく ProductName.v_ProductName を参照するように変更
  UPDATE Product
    SET Product.[Name] = 
          CASE WHEN
                 (ISNULL(s_ProductName.HinMei3,'') + ISNULL(s_ProductName.HinMei4,'')) = ''
               THEN
                 NULL
               ELSE
                 ISNULL(s_ProductName.HinMei3,'') + ISNULL(s_ProductName.HinMei4,'')
          END,         Product.JapaneseName = s_ProductName.HinMei2,
        Product.UpdatedBy = '0',
        Product.UpdateDate = GETDATE()
    FROM Product, s_ProductName
    WHERE Product.ProductNumber = s_ProductName.HinCds
      AND s_ProductName.HinEda = '1';
*/

	/* Product.Name を更新 */
	UPDATE
		Product
	SET
		[Name] = EN.ProductNameSuppl,
		UpdatedBy = 0,
		UpdateDate = GETDATE()
	FROM
		Product AS P,
		(SELECT ProductNumber, ProductNameSuppl FROM s_ProductName2
			WHERE SynonymType = '01' AND LanguageType = '00') AS EN
	WHERE
		EN.ProductNumber = P.ProductNumber
		AND P.NumberType = 'TCI'

	/* Product.JapaneseName を更新 */
	UPDATE
		Product
	SET
		JapaneseName = JA.ProductNameSuppl,
		UpdatedBy = 0,
		UpdateDate = GETDATE()
	FROM
		Product AS P,
		(SELECT ProductNumber, ProductNameSuppl FROM s_ProductName2
			WHERE SynonymType = '01' AND LanguageType = '10') AS JA
	WHERE
		JA.ProductNumber = P.ProductNumber
		AND P.NumberType = 'TCI'

---- end proccess
  COMMIT TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
--  ALTER DATABASE s_ProductName SET ALLOW_SNAPSHOT_ISOLATION OFF;

END TRY

BEGIN CATCH
  set @ErrNUMBER = ERROR_NUMBER()
  set @ErrMESSAGE = ERROR_MESSAGE()   
  set @ErrSTATE = ERROR_STATE()   
  set @ErrLINE = ERROR_LINE() 

  ROLLBACK TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
--  ALTER DATABASE s_ProductName SET ALLOW_SNAPSHOT_ISOLATION OFF;

END CATCH

GO
