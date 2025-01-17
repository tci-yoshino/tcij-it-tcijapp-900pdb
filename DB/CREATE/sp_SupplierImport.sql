/****** オブジェクト:  StoredProcedure [dbo].[sp_SupplierImport]    スクリプト日付: 02/27/2009 14:19:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[sp_SupplierImport] 
-- 仕入先マスタデータのインポートを行う。
-- R/3 仕入先コードに該当する、TCI マスタの仕入先マスタで更新を行う。

@ErrNUMBER int output,
@ErrMESSAGE nvarchar(2000) output,
@ErrSTATE int output,
@ErrLINE int output,
@ErrPCOUNTRY nvarchar(2000) output

AS
SET NOCOUNT ON;

begin try
-- ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION ON;

  begin transaction

    -- s_Supplier のレコード格納変数
    DECLARE @SupNo   nvarchar(10)
    DECLARE @SupCut  nvarchar(1)
    DECLARE @SupCom  nvarchar(50)
    DECLARE @SupMei1 nvarchar(35)
    DECLARE @SupMei2 nvarchar(35)
    DECLARE @SupMei3 nvarchar(35)
    DECLARE @SupMei4 nvarchar(35)
    DECLARE @SupRes1 nvarchar(20)
    DECLARE @SupRes2 nvarchar(20)
    DECLARE @SupCry  nvarchar(3)
    DECLARE @SupCty  nvarchar(3)
    DECLARE @SupZip  nvarchar(10)
    DECLARE @SupAdr1 nvarchar(35)
    DECLARE @SupAdr2 nvarchar(35)
    DECLARE @SupAdr3 nvarchar(40)
    DECLARE @SupAdr4 nvarchar(40)
    DECLARE @SupAdr5 nvarchar(40)
    DECLARE @SupAdr6 nvarchar(40)
    DECLARE @SupAdr7 nvarchar(40)
    DECLARE @SupTel  nvarchar(16)
    DECLARE @SupFax  nvarchar(31)
    DECLARE @SupMail nvarchar(50)
    DECLARE @SupTMei nvarchar(30)
    DECLARE @SupTTel nvarchar(16)
    DECLARE @SupTCom nvarchar(20)

	-- Cursor make by "s_supplier".
    DECLARE s_SupplierCursor CURSOR FOR
      SELECT
        SupNo, SupCut, SupCom, SupMei1, SupMei2, SupMei3, SupMei4,
        SupRes1, SupRes2, SupCry, SupCty, SupZip,
        SupAdr1, SupAdr2, SupAdr3, SupAdr4, SupAdr5, SupAdr6, SupAdr7,
        SupTel, SupFax, SupMail, SupTMei, SupTTel, SupTCom
      FROM
        Supplier JOIN s_Supplier ON Supplier.R3SupplierCode = s_Supplier.SupNo
      ORDER BY
        SupNo ASC

    OPEN s_SupplierCursor;

    FETCH NEXT FROM s_SupplierCursor
    INTO
      @SupNo, @SupCut, @SupCom, @SupMei1, @SupMei2, @SupMei3, @SupMei4,
      @SupRes1, @SupRes2, @SupCry, @SupCty, @SupZip,
      @SupAdr1, @SupAdr2, @SupAdr3, @SupAdr4, @SupAdr5, @SupAdr6, @SupAdr7,
      @SupTel, @SupFax, @SupMail, @SupTMei, @SupTTel, @SupTCom;

    -- roop start
    WHILE @@FETCH_STATUS = 0
    BEGIN

    IF 1 = (SELECT 1 FROM PurchasingCountry WHERE CountryCode = @SupCry) BEGIN
      UPDATE Supplier
      SET
        Name1 = @SupMei1,
        Name2 = @SupMei2,
        Name3 = @SupMei3,
        Name4 = @SupMei4, 
        SearchTerm1 = @SupRes1,
        SearchTerm2 = @SupRes2, 
        Address1 = CASE WHEN @SupCry = 'JP' THEN @SupAdr1 ELSE @SupAdr5 END, 
        Address2 = CASE WHEN @SupCry = 'JP' THEN @SupAdr2 ELSE @SupAdr6 END, 
        Address3 = CASE WHEN @SupCry = 'JP' THEN @SupAdr7 ELSE @SupAdr2 END, 
        PostalCode  = @SupZip,
        CountryCode = @SupCry,
        RegionCode  = @SupCty,
        Telephone   = @SupTel, 
        Fax         = @SupFax,
        Email       = @SupMail,
        Comment     = @SupCom,
        UpdatedBy   = 0,
        UpdateDate  = GETDATE()
      WHERE 
        R3SupplierCode = @SupNo;

    END
    ELSE BEGIN
        set @ErrPCOUNTRY = ISNULL(@ErrPCOUNTRY, '') + '国コードが PurchasingCountry に存在しません : SupNo = ' + @SupNo + ', SupCry = ' + @SupCry + '.' + char(13) + char(10);
    END

      -- レコードを次の行にする
    FETCH NEXT FROM s_SupplierCursor
    INTO
      @SupNo, @SupCut, @SupCom, @SupMei1, @SupMei2, @SupMei3, @SupMei4,
      @SupRes1, @SupRes2, @SupCry, @SupCty, @SupZip,
      @SupAdr1, @SupAdr2, @SupAdr3, @SupAdr4, @SupAdr5, @SupAdr6, @SupAdr7,
      @SupTel, @SupFax, @SupMail, @SupTMei, @SupTTel, @SupTCom;

    END -- roop end

    CLOSE s_SupplierCursor;
    DEALLOCATE s_SupplierCursor;

 commit transaction;

-- ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;

end try

begin catch
 set @ErrNUMBER = ERROR_NUMBER()
 set @ErrMESSAGE = ERROR_MESSAGE()   
 set @ErrSTATE = ERROR_STATE()   
 set @ErrLINE = ERROR_LINE() 

 rollback transaction;

-- ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;

end catch

GO


