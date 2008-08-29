
/****** オブジェクト:  StoredProcedure [dbo].[sp_SupplierImport]    スクリプト日付: 08/28/2008 13:30:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_SupplierImport] 
-- 仕入先マスタデータのインポートを行う。
-- R/3 仕入先コードに該当する、TCI マスタの仕入先マスタで更新を行う。

@ErNUMBER int output,
@ErMESSAGE nvarchar(2000) output,
@ErSTATE int output,
@ErLINE int output

AS
SET NOCOUNT ON;

begin try
-- ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION ON;

 begin transaction

 UPDATE Supplier
  SET Name1 = s_Supplier.SupMei1, Name2 = s_Supplier.SupMei2, Name3 = s_Supplier.SupMei3, Name4 = s_Supplier.SupMei4, 
      SearchTerm1 = s_Supplier.SupRes1, SearchTerm2 = s_Supplier.SupRes2, 
      Address1 = CASE WHEN s_Supplier.SupCry = 'JP' THEN s_Supplier.SupAdr1 ELSE s_Supplier.SupAdr5 END, 
      Address2 = CASE WHEN s_Supplier.SupCry = 'JP' THEN s_Supplier.SupAdr2 ELSE s_Supplier.SupAdr6 END, 
      Address3 = CASE WHEN s_Supplier.SupCry = 'JP' THEN s_Supplier.SupAdr7 ELSE s_Supplier.SupAdr2 END, 
      PostalCode = s_Supplier.SupZip, CountryCode =s_Supplier.SupCry, RegionCode = s_Supplier.SupCty, Telephone = s_Supplier.SupTel, 
      Fax = s_Supplier.SupFax, Email = s_Supplier.SupMail, Comment = s_Supplier.SupCom, UpdatedBy = 1252,
      UpdateDate = GETDATE()
  FROM Supplier JOIN s_Supplier ON Supplier.R3SupplierCode = s_Supplier.SupNo

 commit transaction;

-- ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;

end try

begin catch
 set @ErNUMBER = ERROR_NUMBER()
 set @ErMESSAGE = ERROR_MESSAGE()   
 set @ErSTATE = ERROR_STATE()   
 set @ErLINE = ERROR_LINE() 

 rollback transaction;

-- ALTER DATABASE Purchase SET ALLOW_SNAPSHOT_ISOLATION OFF;

end catch

GO
