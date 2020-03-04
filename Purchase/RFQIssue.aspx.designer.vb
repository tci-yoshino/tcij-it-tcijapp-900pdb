'------------------------------------------------------------------------------
' <自动生成>
'     此代码由工具生成。
'
'     对此文件的更改可能导致不正确的行为，如果
'     重新生成代码，则所做更改将丢失。
' </自动生成>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class RFQIssue
    
    '''<summary>
    '''RFQForm 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents RFQForm As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Msg 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents Msg As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''EnqLocation 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqLocation As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Loc 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Loc As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''EnqUser 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqUser As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Enq_U 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Enq_U As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''ProductNumber 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ProductNumber As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''ProductSelect 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ProductSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''ProductName 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents ProductName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierCode 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierSelect 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''R3SupplierCode 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents R3SupplierCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierName 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierCountry 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierCountry As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerCode 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents MakerCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerSelect 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents MakerSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''SAPMakerCode 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SAPMakerCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerName 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents MakerName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerCountry 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents MakerCountry As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''QuoLocation 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents QuoLocation As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''QuoUser 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents QuoUser As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Que_U 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Que_U As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''Purpose 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents Purpose As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Pur 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Pur As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''Priority 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents Priority As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''RequiredPurity 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents RequiredPurity As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''RequiredQMMethod 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents RequiredQMMethod As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''RequiredSpecification 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents RequiredSpecification As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''isAdmin 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents isAdmin As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''userId 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents userId As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Comment 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents Comment As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_1 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqQuantity_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_1 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqUnit_1 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_1 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqPiece_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SDS_RFQIssue_Qua 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Qua As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''SupplierItemNumber_1 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierItemNumber_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_2 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqQuantity_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_2 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqUnit_2 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_2 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqPiece_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_2 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierItemNumber_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_3 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqQuantity_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_3 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqUnit_3 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_3 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqPiece_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_3 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierItemNumber_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_4 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqQuantity_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_4 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqUnit_4 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_4 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents EnqPiece_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_4 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents SupplierItemNumber_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Issue 控件。
    '''</summary>
    '''<remarks>
    '''自动生成的字段。
    '''若要进行修改，请将字段声明从设计器文件移到代码隐藏文件。
    '''</remarks>
    Protected WithEvents Issue As Global.System.Web.UI.WebControls.Button
End Class
