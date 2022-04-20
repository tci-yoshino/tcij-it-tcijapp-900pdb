
'Option Strict On
Option Explicit On
Option Infer Off
Imports System.Collections.Generic
Imports Purchase.Common
'Namespace Purchase.Control
'/ <summary>
'/ Header.ascx
'/ </summary>
Partial Public Class Header
    Inherits System.Web.UI.UserControl

    ' 要求ページ名
    Private requestPageName As String

    ' 遷移元ページ名
    Private prePageName As String

    ' メニューリスト
    Private _menuList As MenuList

    ' ページ名とメニューの変換表
    Private _pageInfo As PageInfo

    '/ <summary>
    '/ コンストラクタ
    '/ </summary>
    Public Sub New()
        Me.requestPageName = String.Empty
        Me.prePageName = String.Empty
        Me._menuList = New MenuList()
        Me._pageInfo = New PageInfo()
    End Sub


    '/ <summary>
    '/ 初期表示処理
    '/ </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' ポストバック判定
        If (IsPostBack) Then
            Return
        End If

        ' ログイン情報を表示
        Dim userName As String = Session("UserName")
        Dim locationName As String = Session("LocationName")
        Me.UserName.Text = userName
        Me.LocationName.Text = locationName

        ' 要求ページのページ名を取得
        Me.requestPageName = System.IO.Path.GetFileNameWithoutExtension(Request.Url.ToString())

        ' 遷移元ページ名を取得
        If Request.UrlReferrer IsNot Nothing Then
            Dim referer As String = Regex.Replace(Request.UrlReferrer.ToString, "\.[aA][sS][pP][xX].*", "")
            Me.prePageName = System.IO.Path.GetFileName(referer)
        End If

        ' メインメニューリスト
        Dim displayMainMenuList As List(Of Menu) = New List(Of Menu)
        ' サブメニューリスト
        Dim displaySubMenuList As List(Of Menu) = New List(Of Menu)

        ' メニュー作成
        Me.createMenu(displayMainMenuList, displaySubMenuList)

        ' メインメニュー選択状態設定
        Dim selectedMenu As String = Me._pageInfo.getMenuName(Me.requestPageName)
        If String.IsNullOrEmpty(selectedMenu) Then
            ' 遷移元の画面名より選択中メインメニューを決定する
            selectedMenu = Me._pageInfo.getMenuName(Me.prePageName)
        End If

        If Not String.IsNullOrEmpty(selectedMenu) Then
            Dim menu As Menu = displayMainMenuList.FindAll(Function(x) x.MenuName = selectedMenu).FirstOrDefault()
            If Not IsNothing(menu) Then
                ' 選択中にする
                menu.CSS = "current"
            Else
                Debug.WriteLine(selectedMenu + " Is Nothing")
            End If
        End If

        ' サブメニュー選択状態設定
        For Each item As Menu In displaySubMenuList
            item.CSS = If(item.PageName = requestPageName, "current", "")
        Next

        ' データソースにセット
        If (displayMainMenuList.Count > 0) Then
            mainMenuPanel.Visible = True
            mainMenu.Visible = True
            mainMenu.DataSource = displayMainMenuList
            mainMenu.DataBind()
            If (displaySubMenuList.Count > 0) Then
                subMenuPanel.Visible = True
                subMenu.Visible = True
                subMenu.DataSource = displaySubMenuList
                subMenu.DataBind()
            End If
        End If

    End Sub

    '/ <summary>
    '/ メニュー作成
    '/ </summary>
    '/ <param name="displayMainMenuList"></param>
    '/ <param name="displaySubMenuList"></param>
    Private Sub createMenu(displayMainMenuList As List(Of Menu), displaySubMenuList As List(Of Menu))
        ' メインメニューアイテム
        Dim mainMenuItem As Menu = Nothing
        ' サブメニューアイテム
        Dim subMenuItem As Menu = Nothing

        Dim current As String = String.Empty
        For Each menu As MenuItem In Me._menuList
            If Not current = menu.MainMenuName Then
                ' メインメニューは重複排除し追加
                mainMenuItem = New Menu() With {.CSS = "main", .MenuName = menu.MainMenuName, .PageName = menu.PageName, .IsSelected = False}
                displayMainMenuList.Add(mainMenuItem)
                current = menu.MainMenuName
            End If

            ' 要求画面のメニューにサブメニューがある場合のみサブメニューを生成
            If current = menu.MainMenuName AndAlso displaySubMenuList.Count = 0 Then
                Dim menuName As String
                menuName = Me._pageInfo.getMenuName(Me.requestPageName)
                If Not String.IsNullOrEmpty(menuName) Then
                    Dim currentMenus As List(Of MenuItem)
                    currentMenus = Me._menuList.FindAll(Function(x) x.MainMenuName = menuName)
                    If currentMenus.Count > 1 Then
                        For Each item As MenuItem In currentMenus
                            subMenuItem = New Menu() With {.CSS = String.Empty, .MenuName = item.SubMenuName, .PageName = item.PageName, .IsSelected = False}
                            displaySubMenuList.Add(subMenuItem)
                        Next
                    End If
                End If
            End If
        Next


    End Sub

#Region "Inner Class"

#Region "Menu表示用"
    Public Class Menu
        Public Property CSS As String = String.Empty
        Public Property MenuName As String = String.Empty
        Public Property IsSelected As Boolean            ' 選択状態
        Public Property PageName As String = String.Empty
        Public ReadOnly Property NavigateUrl() As String
            Get
                Return "~/" + Me.PageName + ".aspx"
            End Get
        End Property
    End Class
#End Region

#Region "MenuItem"

    '/ <summary>
    '/ メニューアイテムクラス
    '/ </summary>
    Public Class MenuItem


        ' メニュー名
        Public Property MainMenuName() As String
        Public Property SubMenuName() As String
        Public Property PageName() As String
        Public Property IsSelected() As String

        Private _pageInfo As PageInfo

        '/ <summary>
        '/ コンストラクタ
        '/ </summary>
        Public Sub New()
            Me.IsSelected = False
        End Sub

    End Class
#End Region
#Region "MenuList"
    ' Menuに表示する項目
    Public Class MenuList
        Inherits List(Of MenuItem)
        Public Sub New()
            Me.Add(New MenuItem With {.MainMenuName = "Home", .SubMenuName = "My Tasks", .PageName = "MyTask"})
            Me.Add(New MenuItem With {.MainMenuName = "Home", .SubMenuName = "Requested Tasks", .PageName = "RequestedTask"})
            Me.Add(New MenuItem With {.MainMenuName = "Home", .SubMenuName = "Unassigned Tasks", .PageName = "UnassignedTask"})
            Me.Add(New MenuItem With {.MainMenuName = "Home", .SubMenuName = "JFYI", .PageName = "JFYISearch"})
            Me.Add(New MenuItem With {.MainMenuName = "Product", .SubMenuName = "", .PageName = "RFQSearchByProduct"})
            Me.Add(New MenuItem With {.MainMenuName = "Supplier", .SubMenuName = "", .PageName = "RFQSearchBySupplier"})
            Me.Add(New MenuItem With {.MainMenuName = "RFQ Search", .SubMenuName = "", .PageName = "RFQSearch"})
            Me.Add(New MenuItem With {.MainMenuName = "Product Search", .SubMenuName = "Keyword Search", .PageName = "ProductSearchByKeyword"})
            Me.Add(New MenuItem With {.MainMenuName = "Product Search", .SubMenuName = "Structure Search", .PageName = "ProductSearchByStructure"})
            Me.Add(New MenuItem With {.MainMenuName = "Setting", .SubMenuName = "", .PageName = "Setting"})
        End Sub
    End Class
#End Region

#Region "Page"
    ' ページ名とメニューの変換表
    Protected Class PageInfo
        Protected Class PageItem
            Public Property MainMenuName As String = String.Empty
            Public Property PageName As String = String.Empty
        End Class

        Private Property pageList As List(Of PageItem) = New List(Of PageItem)

        ' 利用側でMainMenuName = String.Emptyの場合はリファラの画面名でgetMenuName()を使用する
        Public Function getMenuName(name As String) As String
            Dim page As PageItem = pageList.FindAll(Function(x) x.PageName.ToLower() = name.ToLower()).FirstOrDefault()
            If IsNothing(page) Then
                Return String.Empty
            End If
            Return page.MainMenuName
        End Function

        Public Sub New()
            ' メニューに表示されない画面も設定する
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "CountryList"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "CountrySelect"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "CountrySetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Home", .PageName = "JFYISearch"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "MakerSelect"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Home", .PageName = "MyTask"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "ProductListBySupplier"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Product Search", .PageName = "ProductSearchByKeyword"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Product Search", .PageName = "ProductSearchByStructure"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "ProductSelect"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "ProductSetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "PurchaseGroup"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "PurchaseGroupSetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "RFQCorrespondence"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "RFQIssue"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Product", .PageName = "RFQListByProduct"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Supplier", .PageName = "RFQListBySupplier"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "RFQ Search", .PageName = "RFQSearch"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Product", .PageName = "RFQSearchByProduct"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Supplier", .PageName = "RFQSearchBySupplier"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "RFQSupplierSelect"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "RFQUpdate"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "ReminderList"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "ReminderSetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Home", .PageName = "RequestedTask"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "Setting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "SupplierListByProduct"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "SupplierSelect"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "SupplierSetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "SuppliersProductImport"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "SuppliersProductSetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "SystemError"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Home", .PageName = "UnassignedTask"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "UserList"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "UserSelect"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "UserSetting"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Setting", .PageName = "ProductInfoRegulation"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "Product", .PageName = "HeaderEhs"})
            Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = "MultipleList"})
            'Me.pageList.Add(New PageItem With {.MainMenuName = "", .PageName = ""})
        End Sub

    End Class
#End Region
#End Region
End Class



