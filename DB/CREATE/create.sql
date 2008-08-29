-- synonym の作成
:r $(ScliptPath)CREATE\create_synonym.sql
GO

-- table の作成
:r $(ScliptPath)CREATE\Privilege.sql
:r $(ScliptPath)CREATE\Role.sql
:r $(ScliptPath)CREATE\Role_Privilege.sql
:r $(ScliptPath)CREATE\PurchasingUser.sql
:r $(ScliptPath)CREATE\PurchasingCountry.sql
:r $(ScliptPath)CREATE\Supplier.sql
:r $(ScliptPath)CREATE\IrregularRFQLocation.sql
:r $(ScliptPath)CREATE\PurchasingUnit.sql
:r $(ScliptPath)CREATE\PurchasingCurrency.sql
:r $(ScliptPath)CREATE\PurchasingPaymentTerm.sql
:r $(ScliptPath)CREATE\Product.sql
:r $(ScliptPath)CREATE\Supplier_Product.sql
:r $(ScliptPath)CREATE\Purpose.sql
:r $(ScliptPath)CREATE\NoOfferReason.sql
:r $(ScliptPath)CREATE\RFQCorres.sql
:r $(ScliptPath)CREATE\POCorres.sql
:r $(ScliptPath)CREATE\RFQStatus.sql
:r $(ScliptPath)CREATE\POStatus.sql
:r $(ScliptPath)CREATE\RFQHeader.sql
:r $(ScliptPath)CREATE\RFQLine.sql
:r $(ScliptPath)CREATE\RFQHistory.sql
:r $(ScliptPath)CREATE\PO.sql
:r $(ScliptPath)CREATE\POHistory.sql
:r $(ScliptPath)CREATE\TmpNewProduct.sql
GO

-- trigger の作成
:r $(ScliptPath)CREATE\aifs_RFQHeader.sql
:r $(ScliptPath)CREATE\aufs_RFQHeader.sql
:r $(ScliptPath)CREATE\aifs_PO.sql
:r $(ScliptPath)CREATE\aufs_PO.sql
GO

-- view の作成
:r $(ScliptPath)CREATE\v_User.sql
:r $(ScliptPath)CREATE\v_CompetitorProduct.sql
:r $(ScliptPath)CREATE\v_Country.sql
:r $(ScliptPath)CREATE\v_POCurrentStatus.sql
:r $(ScliptPath)CREATE\v_PO.sql
:r $(ScliptPath)CREATE\v_POReminder.sql
:r $(ScliptPath)CREATE\v_RFQCurrentStatus.sql
:r $(ScliptPath)CREATE\v_RFQHeader.sql
:r $(ScliptPath)CREATE\v_RFQLine.sql
:r $(ScliptPath)CREATE\v_RFQReminder.sql
GO

-- ストアドプロシージャの作成
:r $(ScliptPath)CREATE\sp_NewProductImport.sql
:r $(ScliptPath)CREATE\sp_ProductNameImport.sql
:r $(ScliptPath)CREATE\sp_SupplierImport.sql
GO

