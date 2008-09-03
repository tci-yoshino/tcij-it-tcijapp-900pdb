
/****** �I�u�W�F�N�g:  StoredProcedure [dbo].[sp_NewProductImport]    �X�N���v�g���t: 08/28/2008 13:29:05 ******/
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
    -- �e���|�����e�[�u���̃��R�[�h���i�[����ϐ�
    DECLARE @WProductNumber varchar(5)
    DECLARE @WNewProductRegNumber varchar(32)
    DECLARE @WCASNumber varchar(32)
    DECLARE @WProductName nvarchar(1000)
    DECLARE @WMolecularFormula varchar(128)
    DECLARE @WStatus nvarchar(50)
    DECLARE @WProposalDept nvarchar(50)
    DECLARE @WProcumentDept nvarchar(50)
    DECLARE @WPD nvarchar(50)

    -- ���[�v���Ɏg�p����ϐ�
    DECLARE @PID int            -- ProductID
    DECLARE @PNumber varchar(32) -- ProductNumber
    DECLARE @CAS varchar(32)    -- CASNumber
    DECLARE @NType varchar(5)   -- Number Type

    -- �d����R�[�h���i�[����tbele�ϐ��B
    -- delflag �� 1 �̍s�� Insert ���Ȃ��B
    DECLARE @SupplierCodeTableVer table(ProductID int, SupplierCode int, delflag int)

	-- �J�[�\�����g�p���ăe���|�����e�[�u������S���擾����
    DECLARE WNProductCursor CURSOR FOR
      SELECT ProductNumber, NewProductRegNumber, CASNumber, ProductName, 
             MolecularFormula, Status, ProposalDept, ProcumentDept, PD
      FROM TmpNewProduct
      ORDER BY CASNumber;

------ Main proccess
    -- �J�[�\�����J���A��s�ڂ̃��R�[�h���擾����
    OPEN WNProductCursor;
    FETCH NEXT FROM WNProductCursor
      INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
           @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;

    -- ���R�[�h�̐��Ԃ�A���[�v����
    WHILE @@FETCH_STATUS = 0
    BEGIN
      -- Product.ProductNumber �� CAS �̏ꍇ
      SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WCASNumber);
print @PID;
print ISNULL(@WProductNumber,'-') + ISNULL(@WNewProductRegNumber,'-') + ISNULL(@WCASNumber,'-');
      IF @PID IS NOT NULL BEGIN
        -- ���݂� CAS ��ێ�
        SET @CAS = @WCASNumber;
        -- NumberType �ݒ�
        IF @WProductNumber IS NULL BEGIN SET @NType = 'NEW'  END
        ELSE BEGIN SET @NType = 'TCI'  END
        -- ProductNumber �ݒ�
        IF @WProductNumber IS NULL BEGIN SET @PNumber = @WNewProductRegNumber END
        ELSE BEGIN SET @PNumber = @WProductNumber END

        -- �����R�[�h�� CAS �ƈ�v���� Product.ProductNumber ���X�V
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

          -- �d����R�[�h���擾���Atable �ϐ��Ɋi�[����
          DELETE FROM @SupplierCodeTableVer;
          INSERT INTO @SupplierCodeTableVer (ProductID, SupplierCode) SELECT ProductID, SupplierCode FROM Supplier_Product WHERE ProductID = @PID;

        -- ���̍s��Ǎ���
        FETCH NEXT FROM WNProductCursor
          INTO @WProductNumber, @WNewProductRegNumber, @WCASNumber, @WProductName, 
               @WMolecularFormula, @WStatus, @WProposalDept, @WProcumentDept, @WPD;
        CONTINUE;
      END
      -- Product.ProductNumber �� �V���i�o�^�ԍ��̏ꍇ
      SET @PID = (SELECT ProductID FROM Product WHERE ProductNumber = @WNewProductRegNumber);
      IF @PID IS NOT NULL BEGIN

        -- Product.ProductNumber �� �����R�[�h�̐V���i�o�^�ԍ��ƈ�v����f�[�^���X�V
        -- NumberType �ݒ�
        IF @WProductNumber IS NULL BEGIN SET @NType = 'NEW'  END
        ELSE BEGIN SET @NType = 'TCI'  END
        -- ProductNumber �ݒ�
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
      -- Product.ProductNumber �� �V���i�o�^�ԍ��ȊO�̏ꍇ
      ELSE BEGIN
        -- �����R�[�h�� ���i�R�[�h�� NULL �̏ꍇ
        IF @WProductNumber IS NULL BEGIN
          -- �����R�[�h�̐V���i�o�^�ԍ��� Product.ProductNumber �Ƃ��ĐV�K�o�^
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
        -- �����R�[�h�̐��i�R�[�h�� NULL �ȊO�̏ꍇ
        ELSE BEGIN
          -- Product.ProductNumber �� �����R�[�h�� ProductNumber �ƈ�v����ꍇ�͍X�V�A
          -- �����łȂ���Ό����R�[�h�� ProductNumber �� Product.Procductnumber �Ƃ��ĐV�K�o�^
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

      -- ���ݕێ����Ă��� CAS �Ɠ����ꍇ
      IF @CAS = @WCASNumber BEGIN
        -- ���� Supplier_product �ɓo�^���Ă��鐻�i�� delflag �� 1 �ɂ���
        UPDATE @SupplierCodeTableVer SET delflag = 0; -- delflag ������
        UPDATE @SupplierCodeTableVer
          SET delflag = 1
          FROM Supplier_Product, @SupplierCodeTableVer AS SuppTableVer
          WHERE @PID = Supplier_Product.ProductID
            AND SuppTableVer.SupplierCode = Supplier_Product.SupplierCode;

        -- Supplier_Product �� ���i�R�[�h�Ǝd����R�[�h�� Insert ����
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

      -- ���R�[�h�����̍s�ɂ���
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