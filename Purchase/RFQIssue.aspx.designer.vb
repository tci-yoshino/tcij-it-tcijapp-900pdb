'------------------------------------------------------------------------------
' <��������>
'     ���̃R�[�h�̓c�[���ɂ���Đ�������܂����B
'
'     ���̃t�@�C���ւ̕ύX�́A�ȉ��̏󋵉��ŕs���ȓ���̌����ɂȂ�����A
'     �R�[�h���Đ��������Ƃ��ɑ��������肵�܂��B 
' </��������>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class RFQIssue
    
    '''<summary>
    '''HeaderMenu �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents HeaderMenu As Global.Purchase.Header
    
    '''<summary>
    '''RFQForm �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents RFQForm As Global.System.Web.UI.HtmlControls.HtmlForm
    
    '''<summary>
    '''Msg �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents Msg As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''EnqLocation �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqLocation As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Loc �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Loc As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''EnqUser �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqUser As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Enq_U �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Enq_U As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''ProductNumber �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents ProductNumber As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''ProductSelect �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents ProductSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''CodeExtensionList �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents CodeExtensionList As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''CASNumber �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents CASNumber As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''ProductName �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents ProductName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierCode �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierSelect �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''R3SupplierCode �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents R3SupplierCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierName �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierCountry �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierCountry As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerCode �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents MakerCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerSelect �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents MakerSelect As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''SAPMakerCode �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SAPMakerCode As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerName �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents MakerName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''MakerCountry �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents MakerCountry As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''QuoLocation �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents QuoLocation As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''QuoUser �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents QuoUser As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Que_U �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Que_U As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''Purpose �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents Purpose As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''SDS_RFQIssue_Pur �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Pur As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''Priority �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents Priority As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''RequiredPurity �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents RequiredPurity As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''RequiredQMMethod �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents RequiredQMMethod As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''RequiredSpecification �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents RequiredSpecification As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''isAdmin �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents isAdmin As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''userId �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents userId As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Comment �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents Comment As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemName �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierItemName As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierContactPerson �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierContactPerson As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierContactPersonCodeList �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierContactPersonCodeList As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqQuantity_1 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqQuantity_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_1 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqUnit_1 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_1 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqPiece_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SDS_RFQIssue_Qua �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SDS_RFQIssue_Qua As Global.System.Web.UI.WebControls.SqlDataSource
    
    '''<summary>
    '''SupplierItemNumber_1 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierItemNumber_1 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_2 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqQuantity_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_2 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqUnit_2 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_2 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqPiece_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_2 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierItemNumber_2 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_3 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqQuantity_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_3 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqUnit_3 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_3 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqPiece_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_3 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierItemNumber_3 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqQuantity_4 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqQuantity_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''EnqUnit_4 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqUnit_4 As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''EnqPiece_4 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents EnqPiece_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''SupplierItemNumber_4 �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents SupplierItemNumber_4 As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Issue �R���g���[���B
    '''</summary>
    '''<remarks>
    '''�����������ꂽ�t�B�[���h�B
    '''�ύX����ɂ́A�t�B�[���h�̐錾���f�U�C�i�[ �t�@�C�����番���R�[�h �t�@�C���Ɉړ����܂��B
    '''</remarks>
    Protected WithEvents Issue As Global.System.Web.UI.WebControls.Button
End Class
