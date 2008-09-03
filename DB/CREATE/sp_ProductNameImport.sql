/****** オブジェクト:  StoredProcedure [dbo].[sp_ProductNameImport]    スクリプト日付: 08/28/2008 13:30:21 ******/
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
@ErNUMBER int output,
@ErMESSAGE nvarchar(2000) output,
@ErSTATE int output,
@ErLINE int output
AS

BEGIN TRY
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION ON;
--  ALTER DATABASE s_ProductName SET ALLOW_SNAPSHOT_ISOLATION ON;
--  SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
  BEGIN TRANSACTION;
  SET NOCOUNT OFF;

---- mainproccess
  UPDATE Product
    SET Product.[Name] = ISNULL(s_ProductName.HinMei3,'') + ISNULL(s_ProductName.HinMei4,''),
        Product.JapaneseName = s_ProductName.HinMei2,
        Product.UpdatedBy = '0',
        Product.UpdateDate = GETDATE()
    FROM Product, s_ProductName
    WHERE Product.ProductNumber = s_ProductName.HinCds
      AND s_ProductName.HinEda = '1';

---- end proccess
  COMMIT TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
--  ALTER DATABASE s_ProductName SET ALLOW_SNAPSHOT_ISOLATION OFF;

END TRY

BEGIN CATCH
  set @ErNUMBER = ERROR_NUMBER()
  set @ErMESSAGE = ERROR_MESSAGE()   
  set @ErSTATE = ERROR_STATE()   
  set @ErLINE = ERROR_LINE() 

  ROLLBACK TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
--  ALTER DATABASE s_ProductName SET ALLOW_SNAPSHOT_ISOLATION OFF;

END CATCH

GO