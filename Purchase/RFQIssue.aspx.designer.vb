'------------------------------------------------------------------------------
' <�Զ�����>
'     �˴����ɹ������ɡ�
'
'     �Դ��ļ��ĸ��Ŀ��ܵ��²���ȷ����Ϊ�����
'     �������ɴ��룬���������Ľ���ʧ��
' </�Զ�����>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class RFQIssue
    
    '''<summary>
    '''RFQForm �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents RFQForm As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Msg �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents Msg As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''EnqLocation �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqLocation As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Loc �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Loc As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''EnqUser �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqUser As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Enq_U �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Enq_U As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''ProductNumber �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents ProductNumber As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''ProductSelect �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents ProductSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''ProductName �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents ProductName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierCode �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierSelect �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''R3SupplierCode �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents R3SupplierCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierName �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierCountry �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierCountry As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerCode �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents MakerCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerSelect �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents MakerSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''SAPMakerCode �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SAPMakerCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerName �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents MakerName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerCountry �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents MakerCountry As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''QuoLocation �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents QuoLocation As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''QuoUser �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents QuoUser As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Que_U �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Que_U As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''Purpose �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents Purpose As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Pur �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Pur As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''Priority �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents Priority As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''RequiredPurity �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents RequiredPurity As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''RequiredQMMethod �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents RequiredQMMethod As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''RequiredSpecification �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents RequiredSpecification As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''isAdmin �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents isAdmin As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''userId �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents userId As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Comment �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents Comment As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_1 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqQuantity_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_1 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqUnit_1 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_1 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqPiece_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SDS_RFQIssue_Qua �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Qua As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''SupplierItemNumber_1 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierItemNumber_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_2 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqQuantity_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_2 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqUnit_2 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_2 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqPiece_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_2 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierItemNumber_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_3 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqQuantity_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_3 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqUnit_3 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_3 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqPiece_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_3 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierItemNumber_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_4 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqQuantity_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_4 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqUnit_4 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_4 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents EnqPiece_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_4 �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents SupplierItemNumber_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Issue �ؼ���
    '''</summary>
    '''<remarks>
    '''�Զ����ɵ��ֶΡ�
    '''��Ҫ�����޸ģ��뽫�ֶ�������������ļ��Ƶ����������ļ���
    '''</remarks>
    Protected WithEvents Issue As Global.System.Web.UI.WebControls.Button
End Class
