'------------------------------------------------------------------------------
' <自動生成>
'     このコードはツールによって生成されました。
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。 
' </自動生成>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class ProductListBySupplier
    
    '''<summary>
    '''HeaderMenu コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents HeaderMenu As Global.Purchase.Header
    
    '''<summary>
    '''PageForm コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents PageForm As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Msg コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents Msg As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''SupplierCode コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierCode As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''SupplierName コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierName As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Territory コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents Territory As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''ExcelExportBtn コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ExcelExportBtn As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''hidSourceID コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents hidSourceID As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''ProductID コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents ProductID As Global.System.Web.UI.HtmlControls.HtmlInputHidden
    
    '''<summary>
    '''Action コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents Action As Global.System.Web.UI.HtmlControls.HtmlInputHidden
    
    '''<summary>
    '''HiddenSelectedValidityFilter コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents HiddenSelectedValidityFilter As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''HiddenSortType コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents HiddenSortType As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''HiddenSortField コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents HiddenSortField As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''SupplierProductPagerCountTop コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierProductPagerCountTop As Global.System.Web.UI.WebControls.DataPager
    
    '''<summary>
    '''SupplierProductPagerLinkTop コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierProductPagerLinkTop As Global.System.Web.UI.WebControls.DataPager
    
    '''<summary>
    '''SupplierProductList コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierProductList As Global.System.Web.UI.WebControls.ListView
    
    '''<summary>
    '''SupplierProductPagerLinkBottom コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierProductPagerLinkBottom As Global.System.Web.UI.WebControls.DataPager
    
    '''<summary>
    '''SupplierProductPagerCountBottom コントロール。
    '''</summary>
    '''<remarks>
    '''自動生成されたフィールド。
    '''変更するには、フィールドの宣言をデザイナー ファイルから分離コード ファイルに移動します。
    '''</remarks>
    Protected WithEvents SupplierProductPagerCountBottom As Global.System.Web.UI.WebControls.DataPager
End Class
