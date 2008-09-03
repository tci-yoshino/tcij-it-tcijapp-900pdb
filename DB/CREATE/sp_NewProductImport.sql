
/****** オブジェクト:  StoredProcedure [dbo].[sp_NewProductImport]    スクリプト日付: 08/28/2008 13:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		akutsu
-- Create date: 2008/06/30
-- Description:	New product import.
-- =============================================
CREATE PROCEDURE  [dbo].[sp_NewProductImport]
-- errorr message parameter
@ErNUMBER int output,
@ErMESSAGE nvarchar(2000) output,
@ErSTATE int output,
@ErLINE int output
AS
SET NOCOUNT OFF;
BEGIN TRY
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION ON;
--  SET TRANSACTION ISOLATION LEVEL SNAPSHOT;
  BEGIN TRANSACTION;

---- Settings
    -- テンポラリテーブルのレコードを格納する変数
    DECLARE @WProductNumber varchar(5)
    DECLARE @WNewProductRegNumber varchar(32)
    DECLARE @WCASNumber varchar(32)
    DECLARE @WProductName nvarchar(1000)
    DECLARE @WMolecularFormula varchar(128)
    DECLARE @WStatus nvarchar(50)
    DECLARE @WProposalDept nvarchar(50)
    DECLARE @WProcumentDept nvarchar(50)
    DECLARE @WPD nvarchar(50)

    -- ループ中に使用する変数
    DECLARE @PID int            -- ProductID
    DECLARE @PNumber varchar(32) -- ProductNumber
    DECLARE @CAS varchar(32)    -- CASNumber
    DECLARE @NType varchar(5)   -- Number Type

    -- 仕入先コードを格納するtbele変数。
    -- delflag が 1 の行は Insert しない。
    DECLARE @SupplierCodeTableVer table(ProductID int, SupplierCode int, delflag int)

	-- カーソルを使用してテンポラリテーブルから全件取得する
    DECLARE WNProductCursor CURSOR FOR
      SELECT ProductNumber, NewProductRegNumber, CASNumber, ProductName, 
             MolecularFormula, Status, ProposalDept, ProcumentDept, PD
      FROM TmpNewProduct
      ORDER BY CASNumber;

------ Main proccess
    -- カーソルを開き、一行目のレコードを取得する
    OPEN WNProductCursor;
    FETCH NEXT FROM WNProductCursor
      INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
           @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;

    -- レコードの数ぶん、ループを回す
    WHILE @@FETCH_STATUS = 0
    BEGIN
      -- Product.ProductNumber が CAS の場合
      SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WCASNumber);
print @PID;
print ISNULL(@WProductNumber,'-') + ISNULL(@WNewProductRegNumber,'-') + ISNULL(@WCASNumber,'-');
      IF @PID IS NOT NULL BEGIN
        -- 現在の CAS を保持
        SET @CAS = @WCASNumber;
        -- NumberType 設定
        IF @WProductNumber IS NULL BEGIN SET @NType = 'NEW'  END
        ELSE BEGIN SET @NType = 'TCI'  END
        -- ProductNumber 設定
        IF @WProductNumber IS NULL BEGIN SET @PNumber = @WNewProductRegNumber END
        ELSE BEGIN SET @PNumber = @WProductNumber END

        -- 現レコードの CAS と一致する Product.ProductNumber を更新
        UPDATE Product 
          SET ProductNumber = @PNumber, 
              NumberType = @NType, 
              [Name]     = @WProductName, 
              CASNumber  = @WCASNumber, 
              MolecularFormula = @WMolecularFormula, 
              Status = @WStatus, 
              ProposalDept  = @WProposalDept, 
              ProcumentDept = @WProcumentDept, 
              PD = @WPD, 
              UpdatedBy  = '0', 
              UpdateDate = GETDATE()
          WHERE ProductID = @PID;

          -- 仕入先コードを取得し、table 変数に格納する
          DELETE FROM @SupplierCodeTableVer;
          INSERT INTO @SupplierCodeTableVer (ProductID, SupplierCode) SELECT ProductID, SupplierCode FROM Supplier_Product WHERE ProductID = @PID;

        -- 次の行を読込む
        FETCH NEXT FROM WNProductCursor
          INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
               @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;
        CONTINUE;
      END
      -- Product.ProductNumber が 新製品登録番号の場合
      SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WNewProductRegNumber);
      IF @PID IS NOT NULL BEGIN

        -- Product.ProductNumber が 現レコードの新製品登録番号と一致するデータを更新
        -- NumberType 設定
        IF @WProductNumber IS NULL BEGIN SET @NType = 'NEW'  END
        ELSE BEGIN SET @NType = 'TCI'  END
        -- ProductNumber 設定
        IF @WProductNumber IS NULL BEGIN SET @PNumber = @WNewProductRegNumber END
        ELSE BEGIN SET @PNumber = @WProductNumber END

        UPDATE Product 
          SET ProductNumber = @PNumber, NumberType = @NType, 
              [Name] = @WProductName, CASNumber = @WCASNumber,
              MolecularFormula = @WMolecularFormula, Status = @WStatus, 
              ProposalDept = @WProposalDept, ProcumentDept = @WProcumentDept, 
              PD = @WPD, UpdatedBy = '0', UpdateDate = GETDATE()
          WHERE ProductID = @PID;
      END
      -- Product.ProductNumber が 新製品登録番号以外の場合
      ELSE BEGIN
        -- 現レコードの 製品コードが NULL の場合
        IF @WProductNumber IS NULL BEGIN
          -- 現レコードの新製品登録番号を Product.ProductNumber として新規登録
          INSERT INTO Product 
                   (ProductNumber,NumberType,[Name],CASNumber,
                    MolecularFormula,Status,ProposalDept,ProcumentDept,
                    PD,CreatedBy,CreateDate,UpdatedBy,UpdateDate)
            OUTPUT INSERTED.ProductID
            VALUES (@WNewProductRegNumber,'NEW',@WProductName,@WCASNumber,
                    @WMolecularFormula,@WStatus,@WProposalDept,@WProcumentDept,
                    @WPD,'0',GETDATE(),'0',GETDATE());
          SET @PID = SCOPE_IDENTITY();
        END
        -- 現レコードの製品コードが NULL 以外の場合
        ELSE BEGIN
          -- Product.ProductNumber が 現レコードの ProductNumber と一致する場合は更新、
          -- そうでなければ現レコードの ProductNumber を Product.Procductnumber として新規登録
          SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WProductNumber);
          IF @PID IS NOT NULL BEGIN
			IF not exists (SELECT ProductID FROM Product 
						   WHERE ProductNumber = @WProductNumber
							 AND NumberType = 'TCI' 
							 AND [Name] = @WProductName 
							 AND CASNumber = @WCASNumber
							 AND MolecularFormula = @WMolecularFormula
							 AND Status = @WStatus
							 AND ProposalDept = @WProposalDept
							 AND ProcumentDept = @WProcumentDept
                             AND PD = @WPD) BEGIN
              UPDATE Product 
                SET ProductNumber = @WProductNumber, NumberType = 'TCI', 
                    [Name] = @WProductName, CASNumber = @WCASNumber,
                    MolecularFormula = @WMolecularFormula, Status = @WStatus, 
                   ProposalDept = @WProposalDept, ProcumentDept = @WProcumentDept, 
                   PD = @WPD, UpdatedBy = '0', UpdateDate = GETDATE()
                WHERE ProductID = @PID;
            END
          END
          ELSE BEGIN
            INSERT INTO Product 
                     (ProductNumber,NumberType,[Name],CASNumber,
                      MolecularFormula,Status,ProposalDept,ProcumentDept,
                      PD,CreatedBy,CreateDate,UpdatedBy,UpdateDate)
              VALUES (@WProductNumber,'TCI',@WProductName,@WCASNumber,
                      @WMolecularFormula,@WStatus,@WProposalDept,@WProcumentDept,
                      @WPD,'0',GETDATE(),'0',GETDATE());
            SET @PID = SCOPE_IDENTITY();
          END
        END
      END

      -- 現在保持している CAS と同じ場合
      IF @CAS = @WCASNumber BEGIN
        -- 既に Supplier_product に登録してある製品は delflag を 1 にする
        UPDATE @SupplierCodeTableVer SET delflag = 0; -- delflag 初期化
        UPDATE @SupplierCodeTableVer
          SET delflag = 1
          FROM Supplier_Product, @SupplierCodeTableVer AS SuppTableVer
          WHERE @PID = Supplier_Product.ProductID
            AND SuppTableVer.SupplierCode = Supplier_Product.SupplierCode;

        -- Supplier_Product に 製品コードと仕入先コードを Insert する
		INSERT INTO Supplier_Product (ProductID,SupplierCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate)
		  SELECT 
			@PID AS ProductID ,
			SupplierCode,
			'0' AS CreatedBy,
			GETDATE() AS CreateDate,
			'0' AS UpdatedBy,
			GETDATE() AS UpdateDate
		  FROM @SupplierCodeTableVer
          WHERE delflag = 0;
      END
      ELSE BEGIN 
        SET @CAS = NULL;
        DELETE FROM @SupplierCodeTableVer;
      END

      -- レコードを次の行にする
      FETCH NEXT FROM WNProductCursor
        INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
             @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;
    END

---- end proccess
  CLOSE WNProductCursor;
  DEALLOCATE WNProductCursor;
  COMMIT TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
END TRY

BEGIN CATCH
  set @ErNUMBER = ERROR_NUMBER()
  set @ErMESSAGE = ERROR_MESSAGE()   
  set @ErSTATE = ERROR_STATE()   
  set @ErLINE = ERROR_LINE() 

  ROLLBACK TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
END CATCH

GO