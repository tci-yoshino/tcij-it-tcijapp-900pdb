-- synonym の作成
:r $(ScriptPath)CREATE\create_synonym.sql
GO

-- table の作成
:r $(ScriptPath)CREATE\Privilege.sql
:r $(ScriptPath)CREATE\Role.sql
:r $(ScriptPath)CREATE\Role_Privilege.sql
:r $(ScriptPath)CREATE\PurchasingUser.sql
:r $(ScriptPath)CREATE\PurchasingCountry.sql
:r $(ScriptPath)CREATE\Supplier.sql
:r $(ScriptPath)CREATE\IrregularRFQLocation.sql
:r $(ScriptPath)CREATE\PurchasingUnit.sql
:r $(ScriptPath)CREATE\PurchasingCurrency.sql
:r $(ScriptPath)CREATE\PurchasingPaymentTerm.sql
:r $(ScriptPath)CREATE\Product.sql
:r $(ScriptPath)CREATE\Supplier_Product.sql
:r $(ScriptPath)CREATE\Purpose.sql
:r $(ScriptPath)CREATE\NoOfferReason.sql
:r $(ScriptPath)CREATE\RFQCorres.sql
:r $(ScriptPath)CREATE\POCorres.sql
:r $(ScriptPath)CREATE\RFQStatus.sql
:r $(ScriptPath)CREATE\POStatus.sql
:r $(ScriptPath)CREATE\RFQHeader.sql
:r $(ScriptPath)CREATE\RFQLine.sql
:r $(ScriptPath)CREATE\RFQHistory.sql
:r $(ScriptPath)CREATE\PO.sql
:r $(ScriptPath)CREATE\POHistory.sql
:r $(ScriptPath)CREATE\TmpNewProduct.sql
GO

-- trigger の作成
:r $(ScriptPath)CREATE\aifs_RFQHeader.sql
:r $(ScriptPath)CREATE\aufs_RFQHeader.sql
:r $(ScriptPath)CREATE\aifs_PO.sql
:r $(ScriptPath)CREATE\aufs_PO.sql
GO

-- view の作成
:r $(ScriptPath)CREATE\v_User.sql
:r $(ScriptPath)CREATE\v_CompetitorProduct.sql
:r $(ScriptPath)CREATE\v_Country.sql
:r $(ScriptPath)CREATE\v_POCurrentStatus.sql
:r $(ScriptPath)CREATE\v_PO.sql
:r $(ScriptPath)CREATE\v_POReminder.sql
:r $(ScriptPath)CREATE\v_RFQCurrentStatus.sql
:r $(ScriptPath)CREATE\v_RFQHeader.sql
:r $(ScriptPath)CREATE\v_RFQLine.sql
:r $(ScriptPath)CREATE\v_RFQReminder.sql
GO

-- ストアドプロシージャの作成
:r $(ScriptPath)CREATE\sp_NewProductImport.sql
:r $(ScriptPath)CREATE\sp_ProductNameImport.sql
:r $(ScriptPath)CREATE\sp_SupplierImport.sql
GO

