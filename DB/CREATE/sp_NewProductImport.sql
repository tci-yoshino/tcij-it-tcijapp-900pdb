/****** オブジェクト:  StoredProcedure [dbo].[sp_NewProductImport]    スクリプト日付: 02/27/2009 16:18:32 ******/
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
@ErrNUMBER int output,
@ErrMESSAGE nvarchar(2000) output,
@ErrSTATE int output,
@ErrLINE int output,
@ErrNotice nvarchar(2000) output
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
    DECLARE @Record varchar(32)   -- @WProductNumber + @WNewProductRegNumber + @WCASNumber

    -- 仕入先コードを格納するtbele変数
    DECLARE @SupplierCodeTableVer table(ProductID int, SupplierCode int, delflag int)

------ Main proccess

    DECLARE WNProductCursor CURSOR FOR
      SELECT ProductNumber, NewProductRegNumber, CASNumber, ProductName, 
             MolecularFormula, Status, ProposalDept, ProcumentDept, PD
      FROM TmpNewProduct
      ORDER BY CASNumber;

    OPEN WNProductCursor;

    FETCH NEXT FROM WNProductCursor
      INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
           @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;

    -- ループ処理
    WHILE @@FETCH_STATUS = 0
    BEGIN
      SET @Record = ISNULL(@WProductNumber,'') + char(9) + ISNULL(@WNewProductRegNumber,'') + char(9) + ISNULL(@WCASNumber,'');

      -- a : Product.ProductNumber = 現レコードの CASNumber
      SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WCASNumber);
      IF @PID IS NOT NULL BEGIN

        -- Set CAS, NumberType, ProductNumber
        SET @CAS = @WCASNumber;

        IF @WProductNumber IS NULL BEGIN SET @NType = 'NEW'  END
        ELSE BEGIN SET @NType = 'TCI'  END

        IF @WProductNumber IS NULL BEGIN SET @PNumber = @WNewProductRegNumber END
        ELSE BEGIN SET @PNumber = @WProductNumber END

        -- CAS番号 で Update
        BEGIN TRY
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

          -- 仕入先コード取得
          DELETE FROM @SupplierCodeTableVer;
          INSERT INTO @SupplierCodeTableVer (ProductID, SupplierCode) SELECT ProductID, SupplierCode FROM Supplier_Product WHERE ProductID = @PID;

        END TRY
        BEGIN CATCH
          DECLARE @ErrSeverity INT;

          SELECT @ErrNumber = ERROR_NUMBER(),
                 @ErrMessage = ERROR_MESSAGE(),
                 @ErrSeverity = ERROR_SEVERITY(),
                 @ErrState = ERROR_STATE();

          IF @ErrNumber = '2601' BEGIN
            SET @ErrNotice = ISNULL(@ErrNotice, '') + '重複エラー(CAS 番号) : '+ @Record + char(13) + char(10);
          END
          ELSE BEGIN
            RAISERROR ('(%d) %s', @ErrSeverity, @ErrState, @ErrNumber, @ErrMessage);
          END
        END CATCH

        FETCH NEXT FROM WNProductCursor
          INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
               @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;
        CONTINUE;
      END
      -- b : Product.ProductNumber = 現レコードの NewProductRegNumber
      SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WNewProductRegNumber);
      IF @PID IS NOT NULL BEGIN

        -- Set NumberType, ProductNumber
        IF @WProductNumber IS NULL BEGIN SET @NType = 'NEW'  END
        ELSE BEGIN SET @NType = 'TCI'  END

        IF @WProductNumber IS NULL BEGIN SET @PNumber = @WNewProductRegNumber END
        ELSE BEGIN SET @PNumber = @WProductNumber END

        -- 新製品番号で Update (重複チェックあり）
        IF @NType = 'TCI' AND 1 = (SELECT 1 FROM [Product] WHERE ProductNumber = @PNumber) BEGIN
          SET @ErrNotice = ISNULL(@ErrNotice, '') + '重複エラー(新製品番号) : '+ @Record + char(13) + char(10);
        END
        ELSE BEGIN
			UPDATE Product 
			  SET ProductNumber = @PNumber, 
                  NumberType = @NType, 
				  [Name] = @WProductName, 
                  CASNumber = @WCASNumber,
				  MolecularFormula = @WMolecularFormula, 
                  Status = @WStatus, 
				  ProposalDept = @WProposalDept, 
                  ProcumentDept = @WProcumentDept, 
				  PD = @WPD, 
                  UpdatedBy = '0', 
                  UpdateDate = GETDATE()
			  WHERE ProductID = @PID;
	    END
      END
      -- c : Product.ProductNumber != 現レコードの NewProductRegNumber
      ELSE BEGIN
        -- c - 1 : 現レコードの ProductNumber IS NULL
        IF @WProductNumber IS NULL BEGIN
          -- 新製品登録番号で Insert
          INSERT INTO Product 
                   (ProductNumber,
                    NumberType,[Name],
                    CASNumber,
                    MolecularFormula,
                    Status,
                    ProposalDept,
                    ProcumentDept,
                    PD,
                    CreatedBy,
                    CreateDate,
                    UpdatedBy,
                    UpdateDate)
            OUTPUT INSERTED.ProductID
            VALUES (@WNewProductRegNumber,
                    'NEW',
                    @WProductName,
                    @WCASNumber,
                    @WMolecularFormula,
                    @WStatus,
                    @WProposalDept,
                    @WProcumentDept,
                    @WPD,
                    '0',
                    GETDATE(),
                    '0',
                    GETDATE());
          SET @PID = SCOPE_IDENTITY();
        END
        -- c - 2 : 現レコードの ProductNumber IS NOT NULL
        ELSE BEGIN
          SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WProductNumber);

          -- c - 2 -1 : Product.ProductNumber = 現レコードの ProductNumber
          IF @PID IS NOT NULL BEGIN
              -- 製品コードで Update
              UPDATE Product 
                SET ProductNumber = @WProductNumber, 
                    NumberType = 'TCI', 
                    [Name] = @WProductName, 
                    CASNumber = @WCASNumber,
                    MolecularFormula = @WMolecularFormula, 
                    Status = @WStatus, 
                    ProposalDept = @WProposalDept, 
                    ProcumentDept = @WProcumentDept, 
                    PD = @WPD, 
                    UpdatedBy = '0', 
                    UpdateDate = GETDATE()
                WHERE ProductID = @PID;
          END

          -- c - 2 - 2 : Product.ProductNumber != 現レコードの ProductNumber
          ELSE BEGIN
            -- 製品コードで Insert
            INSERT INTO Product 
                     (ProductNumber,
                      NumberType,
                      [Name],
                      CASNumber,
                      MolecularFormula,
                      Status,
                      ProposalDept,
                      ProcumentDept,
                      PD,
                      CreatedBy,
                      CreateDate,
                      UpdatedBy,
                      UpdateDate)
              VALUES (@WProductNumber,
                      'TCI',
                      @WProductName,
                      @WCASNumber,
                      @WMolecularFormula,
                      @WStatus,
                      @WProposalDept,
                      @WProcumentDept,
                      @WPD,
                      '0',
                      GETDATE(),
                      '0',
                      GETDATE());
            SET @PID = SCOPE_IDENTITY();
          END
        END
      END

      -- 仕入先製品登録
      IF @CAS = @WCASNumber BEGIN
        -- 登録済みチェック
        UPDATE @SupplierCodeTableVer SET delflag = 0; -- delflag 初期化
        UPDATE @SupplierCodeTableVer
          SET delflag = 1
          FROM Supplier_Product, @SupplierCodeTableVer AS SuppTableVer
          WHERE @PID = Supplier_Product.ProductID
            AND SuppTableVer.SupplierCode = Supplier_Product.SupplierCode;

        -- 仕入先製品 Insert
		INSERT INTO Supplier_Product 
                 (ProductID,
                  SupplierCode,
                  CreatedBy,
                  CreateDate,
                  UpdatedBy,
                  UpdateDate)
		  SELECT 
			@PID AS ProductID,
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

      FETCH NEXT FROM WNProductCursor
        INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
             @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;
    END

---- end proccess
  CLOSE WNProductCursor;
  DEALLOCATE WNProductCursor;
  COMMIT TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
  RETURN 0;
END TRY

BEGIN CATCH
  set @ErrNUMBER = ERROR_NUMBER()
  set @ErrMESSAGE = ERROR_MESSAGE() + char(13) + char(10) + 'IMPORT DATA : ' + @Record + char(13) + char(10)
  set @ErrSTATE = ERROR_STATE()   
  set @ErrLINE = ERROR_LINE() 

  ROLLBACK TRANSACTION;
--  ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;
END CATCH



