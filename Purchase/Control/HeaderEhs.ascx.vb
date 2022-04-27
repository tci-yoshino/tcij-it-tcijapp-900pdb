Option Strict On
Option Explicit On
Option Infer Off

Imports System.Reflection

'Namespace Purchase.Control

''' <summary>
''' HeaderEhs.ascx
''' </summary>
Public Class HeaderEhs
    Inherits System.Web.UI.UserControl

    '' 表示リスト
    Protected _EhsItemList As List(Of EhsItem) = Nothing 
    Protected _UserID As Integer = 0
    Protected _LocationCode As String = String.Empty
    Protected _ProductNumber As String = String.Empty

    ''' <summary> 
    ''' EhsItemList を設定、または取得する 
    ''' </summary> 
    Public Property EhsItemList() As List(Of EhsItem)
        Get
            Return _EhsItemList
        End Get
        Set(ByVal value As List(Of EhsItem))
            _EhsItemList = value
        End Set
    End Property

    ''' <summary> 
    ''' UserID を設定、または取得する 
    ''' </summary> 
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    ''' <summary> 
    ''' locationCode を設定、または取得する 
    ''' </summary> 
    Public Property LocationCode() As String
        Get
            Return _LocationCode
        End Get
        Set(ByVal value As String)
            _LocationCode = value
        End Set
    End Property

    ''' <summary> 
    ''' ProductNumber を設定、または取得する 
    ''' </summary> 
    Public Property ProductNumber() As String
        Get
            Return _ProductNumber
        End Get
        Set(ByVal value As String)
            _ProductNumber = value
        End Set
    End Property

    ''' <summary> 
    ''' コンストラクタ
    ''' </summary> 
    Public Sub New()
        _EhsItemList = New List(Of EhsItem)
    End Sub

    ''' <summary>
    ''' 初期表示処理
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ポストバック判定
        If (IsPostBack) Then
            Return
        End If

    End Sub

    ''' <summary>
    ''' 画面表示処理
    ''' </summary>
    Public Sub GetEhsHeader()

        '' s_SPECS取得
        Dim s_SPECS As TCIDataAccess.s_SPECS = New TCIDataAccess.s_SPECS
        s_SPECS.Load(_ProductNumber)

        '' ヘッダの個人設定情報を取得
        Dim headerEhsDisp As TCIDataAccess.Join.HeaderEhsDisp = New TCIDataAccess.Join.HeaderEhsDisp
        headerEhsDisp.UserID = Me.UserID
        headerEhsDisp.GetEhsHeader()

        If headerEhsDisp.EhsHeaderList.Count <> 0 Then
            '' 個人設定の表示リストを作成
            Me.CreateEhsListPersonalize(s_SPECS)
        Else
            '' 拠点単位の初期表示リストを作成
            Select Case _LocationCode
                Case Common.LOCATION_JP
                    Me.CreateEhsListLocation(Common.LOCATION_JP, s_SPECS)
                Case Common.LOCATION_US
                    Me.CreateEhsListLocation(Common.LOCATION_US, s_SPECS)
                Case Common.LOCATION_EU
                    Me.CreateEhsListLocation(Common.LOCATION_EU, s_SPECS)
                Case Common.LOCATION_IN
                    Me.CreateEhsListLocation(Common.LOCATION_IN, s_SPECS)
                Case Common.LOCATION_CN
                    Me.CreateEhsListLocation(Common.LOCATION_CN, s_SPECS)
                Case Else

            End Select

        End If

        '' EhsHeaderに表示リストをバインド
        ehsList.DataSource = Me.EhsItemList
        ehsList.DataBind()

    End Sub

    ''' <summary>
    ''' 個人設定の表示リスト作成
    ''' </summary>
    ''' <param name="s_SPECS"></param>
    Private Sub CreateEhsListPersonalize(ByVal s_SPECS As TCIDataAccess.s_SPECS)

        '' HeaderEhs項目（全件）
        Dim ehsHeaderList As TCIDataAccess.Join.HeaderEhsDisp = New TCIDataAccess.Join.HeaderEhsDisp
        ehsHeaderList.UserID = Me.UserID
        ehsHeaderList.LocationCode = Me.LocationCode
        ehsHeaderList.GetEhsHeaderPersonalize()

        '' Header に表示するリストを作成
        For Each ehsHeader As TCIDataAccess.s_EhsHeader In ehsHeaderList.EhsHeaderListForPersonalize
            Me.CreateEhsItem(ehsHeader, s_SPECS)
        Next

    End Sub

    ''' <summary>
    ''' 拠点単位の表示リスト作成
    ''' </summary>
    ''' <param name="s_SPECS"></param>
    Private Sub CreateEhsListLocation(ByVal st_LocationCode As String, ByVal s_SPECS As TCIDataAccess.s_SPECS)

        '' HeaderEhs項目（拠点）
        Dim ehsHeaderList As TCIDataAccess.Join.HeaderEhsDisp = New TCIDataAccess.Join.HeaderEhsDisp
        ehsHeaderList.LocationCode = st_LocationCode
        ehsHeaderList.GetEhsHeaderLocation()

        '' Header に表示するリストを作成
        For Each ehsHeader As TCIDataAccess.s_EhsHeader In ehsHeaderList.EhsHeaderListForLocation
            Me.CreateEhsItem(ehsHeader, s_SPECS)
        Next

    End Sub

    ''' <summary>
    ''' Ehsリスト作成
    ''' </summary>
    ''' <param name="ehsHeader"></param>
    Private Sub CreateEhsItem(ByVal ehsHeader As TCIDataAccess.s_EhsHeader, ByVal s_SPECS As TCIDataAccess.s_SPECS)

        '' Ehs項目データクラス
        Dim EhsItem As EhsItem = New EhsItem

        '' Header に表示するリストを作成
        If ehsHeader.Item.Equals("CATPUB") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CATPUB)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("CC") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CC)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("CCC") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CCC)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("COM805") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.COM805)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("CPC") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CPC)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("CTSL") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CTSL)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("CWSITEI") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CWSITEI)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("CWSITEISYU") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.CWSITEISYU)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("DCSNUM") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.DCSNUM)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("DELI") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.DELI)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("DGLQ") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.DGLQ)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("ECNUM") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.ECNUM)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("ECSC") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.ECSC)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("ENCS") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.ENCS)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("ENCSNUM") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.ENCSNUM)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("EQ") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.EQ)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("EUREG") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.EUREG)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("EXPORT") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.EXPORT)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("FLAG") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.FLAG)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("FZSHIP") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.FZSHIP)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("HAZ") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.HAZ)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("HS_A") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.HS_A)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("HS_E") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.HS_E)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("HS_J") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.HS_J)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("IFC") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.IFC)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("IMI") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.IMI)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("INOEX") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.INOEX)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("IPL") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.IPL)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("KAKUSEIZAI") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.KAKUSEIZAI)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("KOUSEI") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.KOUSEI)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("LIM0") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.LIM0)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("LIM1") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.LIM1)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("LIM2") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.LIM2)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("LQVAL") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.LQVAL)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("MAGEN") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.MAGEN)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("NDPS") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.NDPS)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("OZONE") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.OZONE)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("PD") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.PD)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("PRE") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.PRE)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("PURITY") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.PURITY)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("REPFLAG") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.REPFLAG)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("RES1") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.RES1)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("RES2") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.RES2)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("RES3") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.RES3)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("RES4") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.RES4)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("SEIZOU") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.SEIZOU)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("SG") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.SG)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("SHITEI") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.SHITEI)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("SNOEX") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.SNOEX)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("SPITEM") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.SPITEM)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("STORAGE") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.STORAGE)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("SVHC") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.SVHC)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("TSCA") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.TSCA)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("UNCLASS") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.UNCLASS)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("UNNUM") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.UNNUM)
            Me.EhsItemList.Add(EhsItem)
        ElseIf ehsHeader.Item.Equals("UNSUB") Then
            EhsItem = New EhsItem(ehsHeader.Text, s_SPECS.UNSUB)
            Me.EhsItemList.Add(EhsItem)
        End If

    End Sub

    ''' <summary>
    ''' EHS項目名と値の組み合わせが1行に収まらない場合は、EHS項目名と値の間で改行を行う
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ehsList_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs)

        Dim ItemName As Label = CType(e.Item.FindControl("ItemName"), Label)
        Dim ItemValue As Label = CType(e.Item.FindControl("ItemValue"), Label)

        '' 1行超え(EHS 1項目の幅≒38バイト)
        If (ItemName.Text.Length + ItemValue.Text.Length) > 38 Then
            ItemName.Text = ItemName.Text + "<br>"
        End If

    End Sub

End Class

''' <summary>
''' 汎用クラス
''' </summary>
''' <remarks>プロパティ名称の取得</remarks>
Public Class EhsProperty

    Shared Function GetName() As String
        Dim t As Type = GetType(EhsItem)
        Dim inst As Object = Activator.CreateInstance(t)

        ' プロパティPのPropertyInfoを取得する
        Dim p As PropertyInfo = t.GetProperty("ItemName")

        ' プロパティの値を取得する
        Return p.GetValue(inst).ToString

    End Function

End Class

''' <summary>
''' Ehsアイテムクラス
''' </summary>
Public Class EhsItem

    Protected _ItemName As String = String.Empty
    Protected _ItemValue As String = String.Empty

    ''' <summary> 
    ''' ItemName を設定、または取得する 
    ''' </summary> 
    Public Property ItemName() As String
        Get
            Return _ItemName
        End Get
        Set(ByVal value As String)
            _ItemName = value
        End Set
    End Property

    ''' <summary> 
    ''' ItemValue を設定、または取得する 
    ''' </summary> 
    Public Property ItemValue() As String
        Get
            Return _ItemValue
        End Get
        Set(ByVal value As String)
            _ItemValue = value
        End Set
    End Property

    ''' <summary> 
    ''' コンストラクタ
    ''' </summary> 
    Public Sub New()

    End Sub

    ''' <summary> 
    ''' コンストラクタ
    ''' </summary> 
    ''' <param name="st_ItemName">ItemName</param>
    ''' <param name="st_ItemValue">ItemValue</param>
    Public Sub New(ByVal st_ItemName As String, ByVal st_ItemValue As String)

        ' 初期化
        Me.ItemName = String.Empty
        Me.ItemValue = String.Empty

        ' 格納
        Me.ItemName = st_ItemName
        Me.ItemValue = st_ItemValue

    End Sub

End Class

'End Namespace
