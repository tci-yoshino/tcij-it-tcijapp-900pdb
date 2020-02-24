USE [Purchase]
GO

/****** Object:  StoredProcedure [dbo].[sp_SupplierImport]    Script Date: 2020/02/20 22:06:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[sp_SupplierImport] 
@ErrNUMBER int output,
@ErrMESSAGE nvarchar(2000) output,
@ErrSTATE int output,
@ErrLINE int output,
@ErrPCOUNTRY nvarchar(2000) output
AS
SET NOCOUNT ON;
begin try
  begin transaction
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
    DECLARE @SupCty  nvarchar(20)
    DECLARE @SupZip  nvarchar(10)
    DECLARE @SupAdr1 nvarchar(70)
    DECLARE @SupAdr2 nvarchar(35)
    DECLARE @SupAdr3 nvarchar(40)
    DECLARE @SupAdr4 nvarchar(40)
    DECLARE @SupAdr5 nvarchar(40)
    DECLARE @SupAdr6 nvarchar(40)
    DECLARE @SupAdr7 nvarchar(35)
    DECLARE @SupTel  nvarchar(16)
    DECLARE @SupFax  nvarchar(31)
    DECLARE @SupMail nvarchar(50)
    DECLARE @SupTMei nvarchar(30)
    DECLARE @SupTTel nvarchar(16)
    DECLARE @SupTCom nvarchar(20)
	
	DECLARE @SupID1 nvarchar(3)
    DECLARE @SupMAD1 nvarchar(50)
    DECLARE @SupREM1 nvarchar(50)
	DECLARE @SupID2 nvarchar(3)
    DECLARE @SupMAD2 nvarchar(50)
    DECLARE @SupREM2 nvarchar(50)
	DECLARE @SupID3 nvarchar(3)
    DECLARE @SupMAD3 nvarchar(50)
    DECLARE @SupREM3 nvarchar(50)
	DECLARE @SupID4 nvarchar(3)
    DECLARE @SupMAD4 nvarchar(50)
    DECLARE @SupREM4 nvarchar(50)
	DECLARE @SupID5 nvarchar(3)
    DECLARE @SupMAD5 nvarchar(50)
    DECLARE @SupREM5 nvarchar(50)
	DECLARE @SupID6 nvarchar(3)
    DECLARE @SupMAD6 nvarchar(50)
    DECLARE @SupREM6 nvarchar(50)
	DECLARE @SupID7 nvarchar(3)
    DECLARE @SupMAD7 nvarchar(50)
    DECLARE @SupREM7 nvarchar(50)
	DECLARE @SupID8 nvarchar(3)
    DECLARE @SupMAD8 nvarchar(50)
    DECLARE @SupREM8 nvarchar(50)
	DECLARE @SupID9 nvarchar(3)
    DECLARE @SupMAD9 nvarchar(50)
    DECLARE @SupREM9 nvarchar(50)
	DECLARE @SupID10 nvarchar(3)
    DECLARE @SupMAD10 nvarchar(50)
    DECLARE @SupREM10 nvarchar(50)
	DECLARE @ExtSupNo nvarchar(50)
	DECLARE @SupplierCode  nvarchar(10)
    DECLARE s_SupplierCursor CURSOR FOR
      SELECT
        SupNo, SupCut, SupCom, SupMei1, SupMei2, SupMei3, SupMei4,
        SupRes1, SupRes2, SupCry, SupCty, SupZip,
        SupAdr1, SupAdr2, SupAdr3, SupAdr4, SupAdr5, SupAdr6, SupAdr7,
        SupTel, SupFax, SupMail, SupTMei, SupTTel, SupTCom,
		SupID1,SupMAD1,SupREM1,
		SupID2,SupMAD2,SupREM2,
		SupID3,SupMAD3,SupREM3,
		SupID4,SupMAD4,SupREM4,
		SupID5,SupMAD5,SupREM5,
		SupID6,SupMAD6,SupREM6,
		SupID7,SupMAD7,SupREM7,
		SupID8,SupMAD8,SupREM8,
		SupID9,SupMAD9,SupREM9,
		SupID10,SupMAD10,SupREM10,
		ExtSupNo
      FROM
		Supplier JOIN s_Supplier ON Supplier.S4SupplierCode  = s_Supplier.SupNo
      ORDER BY
        ExtSupNo ASC
	DECLARE s_SupplierCursorByExtSupNo CURSOR FOR
      SELECT
        SupNo, SupCut, SupCom, SupMei1, SupMei2, SupMei3, SupMei4,
        SupRes1, SupRes2, SupCry, SupCty, SupZip,
        SupAdr1, SupAdr2, SupAdr3, SupAdr4, SupAdr5, SupAdr6, SupAdr7,
        SupTel, SupFax, SupMail, SupTMei, SupTTel, SupTCom,
		SupID1,SupMAD1,SupREM1,
		SupID2,SupMAD2,SupREM2,
		SupID3,SupMAD3,SupREM3,
		SupID4,SupMAD4,SupREM4,
		SupID5,SupMAD5,SupREM5,
		SupID6,SupMAD6,SupREM6,
		SupID7,SupMAD7,SupREM7,
		SupID8,SupMAD8,SupREM8,
		SupID9,SupMAD9,SupREM9,
		SupID10,SupMAD10,SupREM10,
		ExtSupNo
      FROM
		Supplier JOIN s_Supplier ON Supplier.SupplierCode = s_Supplier.ExtSupNo
      ORDER BY
        ExtSupNo ASC

    OPEN s_SupplierCursor;
    FETCH NEXT FROM s_SupplierCursor
    INTO
      @SupNo, @SupCut, @SupCom, @SupMei1, @SupMei2, @SupMei3, @SupMei4,
      @SupRes1, @SupRes2, @SupCry, @SupCty, @SupZip,
      @SupAdr1, @SupAdr2, @SupAdr3, @SupAdr4, @SupAdr5, @SupAdr6, @SupAdr7,
      @SupTel, @SupFax, @SupMail, @SupTMei, @SupTTel, @SupTCom,
	  @SupID1,@SupMAD1,@SupREM1,
	  @SupID2,@SupMAD2,@SupREM2,
	  @SupID3,@SupMAD3,@SupREM3,
	  @SupID4,@SupMAD4,@SupREM4,
	  @SupID5,@SupMAD5,@SupREM5,
	  @SupID6,@SupMAD6,@SupREM6,
	  @SupID7,@SupMAD7,@SupREM7,
	  @SupID8,@SupMAD8,@SupREM8,
	  @SupID9,@SupMAD9,@SupREM9,
	  @SupID10,@SupMAD10,@SupREM10,@ExtSupNo;
    WHILE @@FETCH_STATUS = 0
    BEGIN    
		
	  Select @SupplierCode=SupplierCode From Supplier Where Supplier.S4SupplierCode = @SupNo;
		IF @SupplierCode <> @ExtSupNo
			begin
				Update Supplier
				Set S4SupplierCode = null
				Where SupplierCode = @SupplierCode;

				Update Supplier
				Set S4SupplierCode = @SupNo
				Where SupplierCode = @ExtSupNo;
			end

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
        Comment     = @SupCom,
        UpdatedBy   = 0,
        UpdateDate  = GETDATE(),
		isDisabled	= CASE WHEN @SupCut is null THEN 0 ELSE 1 END,
		Email=@SupMail,
		S4SupplierCode=CASE WHEN @SupCut is null THEN @SupNo ELSE null END,						
		SupplierEmailID1=@SupID1,							
		SupplierEmail1=@SupMAD1,							
		SupplierContactperson1=@SupREM1,							
		SupplierEmailID2=@SupID2,							
		SupplierEmail2=@SupMAD2,						
		SupplierContactperson2=@SupREM2,							
		SupplierEmailID3=@SupID3,							
		SupplierEmail3=@SupMAD3,							
		SupplierContactperson3=@SupREM3,							
		SupplierEmailID4=@SupID4,							
		SupplierEmail4=@SupMAD4,							
		SupplierContactperson4=@SupREM4,							
		SupplierEmailID5=@SupID5,							
		SupplierEmail5=@SupMAD5,							
		SupplierContactperson5=@SupREM5,							
		SupplierEmailID6=@SupID6,							
		SupplierEmail6=@SupMAD6,							
		SupplierContactperson6=@SupREM6,							
		SupplierEmailID7=@SupID7,							
		SupplierEmail7=@SupMAD7,							
		SupplierContactperson7=@SupREM7,							
		SupplierEmailID8=@SupID8,							
		SupplierEmail8=@SupMAD8,							
		SupplierContactperson8=@SupREM8,							
		SupplierEmailID9=@SupID9,							
		SupplierEmail9=@SupMAD9,							
		SupplierContactperson9=@SupREM9,							
		SupplierEmailID10=@SupID10,							
		SupplierEmail10=@SupMAD10,							
		SupplierContactperson10=@SupREM10		
      WHERE 
        S4SupplierCode = @SupNo;
    FETCH NEXT FROM s_SupplierCursor
    INTO
      @SupNo, @SupCut, @SupCom, @SupMei1, @SupMei2, @SupMei3, @SupMei4,
      @SupRes1, @SupRes2, @SupCry, @SupCty, @SupZip,
      @SupAdr1, @SupAdr2, @SupAdr3, @SupAdr4, @SupAdr5, @SupAdr6, @SupAdr7,
      @SupTel, @SupFax, @SupMail, @SupTMei, @SupTTel, @SupTCom,
	  @SupID1,@SupMAD1,@SupREM1,
	  @SupID2,@SupMAD2,@SupREM2,
	  @SupID3,@SupMAD3,@SupREM3,
	  @SupID4,@SupMAD4,@SupREM4,
	  @SupID5,@SupMAD5,@SupREM5,
	  @SupID6,@SupMAD6,@SupREM6,
	  @SupID7,@SupMAD7,@SupREM7,
	  @SupID8,@SupMAD8,@SupREM8,
	  @SupID9,@SupMAD9,@SupREM9,
	  @SupID10,@SupMAD10,@SupREM10,@ExtSupNo;
    END -- roop end
    CLOSE s_SupplierCursor;
    DEALLOCATE s_SupplierCursor;

	OPEN s_SupplierCursorByExtSupNo;
    FETCH NEXT FROM s_SupplierCursorByExtSupNo
    INTO
      @SupNo, @SupCut, @SupCom, @SupMei1, @SupMei2, @SupMei3, @SupMei4,
      @SupRes1, @SupRes2, @SupCry, @SupCty, @SupZip,
      @SupAdr1, @SupAdr2, @SupAdr3, @SupAdr4, @SupAdr5, @SupAdr6, @SupAdr7,
      @SupTel, @SupFax, @SupMail, @SupTMei, @SupTTel, @SupTCom,
	  @SupID1,@SupMAD1,@SupREM1,
	  @SupID2,@SupMAD2,@SupREM2,
	  @SupID3,@SupMAD3,@SupREM3,
	  @SupID4,@SupMAD4,@SupREM4,
	  @SupID5,@SupMAD5,@SupREM5,
	  @SupID6,@SupMAD6,@SupREM6,
	  @SupID7,@SupMAD7,@SupREM7,
	  @SupID8,@SupMAD8,@SupREM8,
	  @SupID9,@SupMAD9,@SupREM9,
	  @SupID10,@SupMAD10,@SupREM10,@ExtSupNo;
    WHILE @@FETCH_STATUS = 0
    BEGIN    
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
        Comment     = @SupCom,
        UpdatedBy   = 0,
        UpdateDate  = GETDATE(),
		isDisabled	= CASE WHEN @SupCut is null THEN 0 ELSE 1 END,
		Email=@SupMail,
		S4SupplierCode=CASE WHEN @SupCut is null THEN @SupNo ELSE null END,						
		SupplierEmailID1=@SupID1,							
		SupplierEmail1=@SupMAD1,							
		SupplierContactperson1=@SupREM1,							
		SupplierEmailID2=@SupID2,							
		SupplierEmail2=@SupMAD2,						
		SupplierContactperson2=@SupREM2,							
		SupplierEmailID3=@SupID3,							
		SupplierEmail3=@SupMAD3,							
		SupplierContactperson3=@SupREM3,							
		SupplierEmailID4=@SupID4,							
		SupplierEmail4=@SupMAD4,							
		SupplierContactperson4=@SupREM4,							
		SupplierEmailID5=@SupID5,							
		SupplierEmail5=@SupMAD5,							
		SupplierContactperson5=@SupREM5,							
		SupplierEmailID6=@SupID6,							
		SupplierEmail6=@SupMAD6,							
		SupplierContactperson6=@SupREM6,							
		SupplierEmailID7=@SupID7,							
		SupplierEmail7=@SupMAD7,							
		SupplierContactperson7=@SupREM7,							
		SupplierEmailID8=@SupID8,							
		SupplierEmail8=@SupMAD8,							
		SupplierContactperson8=@SupREM8,							
		SupplierEmailID9=@SupID9,							
		SupplierEmail9=@SupMAD9,							
		SupplierContactperson9=@SupREM9,							
		SupplierEmailID10=@SupID10,							
		SupplierEmail10=@SupMAD10,							
		SupplierContactperson10=@SupREM10		
      WHERE 
        SupplierCode = @ExtSupNo;
    FETCH NEXT FROM s_SupplierCursorByExtSupNo
    INTO
      @SupNo, @SupCut, @SupCom, @SupMei1, @SupMei2, @SupMei3, @SupMei4,
      @SupRes1, @SupRes2, @SupCry, @SupCty, @SupZip,
      @SupAdr1, @SupAdr2, @SupAdr3, @SupAdr4, @SupAdr5, @SupAdr6, @SupAdr7,
      @SupTel, @SupFax, @SupMail, @SupTMei, @SupTTel, @SupTCom,
	  @SupID1,@SupMAD1,@SupREM1,
	  @SupID2,@SupMAD2,@SupREM2,
	  @SupID3,@SupMAD3,@SupREM3,
	  @SupID4,@SupMAD4,@SupREM4,
	  @SupID5,@SupMAD5,@SupREM5,
	  @SupID6,@SupMAD6,@SupREM6,
	  @SupID7,@SupMAD7,@SupREM7,
	  @SupID8,@SupMAD8,@SupREM8,
	  @SupID9,@SupMAD9,@SupREM9,
	  @SupID10,@SupMAD10,@SupREM10,@ExtSupNo;
    END -- roop end
    CLOSE s_SupplierCursorByExtSupNo;
    DEALLOCATE s_SupplierCursorByExtSupNo;
 commit transaction;
end try
begin catch
 set @ErrNUMBER = ERROR_NUMBER()
 set @ErrMESSAGE = ERROR_MESSAGE()   
 set @ErrSTATE = ERROR_STATE()   
 set @ErrLINE = ERROR_LINE() 
 rollback transaction;
end catch
GO

