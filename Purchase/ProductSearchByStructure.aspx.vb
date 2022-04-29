Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient
Imports Purchase.Common
Imports System.IO

Partial Public Class ProductSearchByStructure
    Inherits CommonPage
    'DA定義
    Private srcStructure As TCIDataAccess.Join.StructureSearchDispList = New TCIDataAccess.Join.StructureSearchDispList

    '変数定義
    '定数定義
    Private Const NHITS As String = "100"


    ''' <summary>
    ''' ページロードイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = String.Empty

        UnDsp_SearchResultList()

        Dim st_Action As String = Request.QueryString("Search")
        If Common.PRIORITY_FOR_SEARCH.Equals(st_Action) Then
            '検索実行
            SimilaritySearch_Click(SimilaritySearch, New EventArgs)
        End If

    End Sub

    ''' <summary>
    ''' Structure Search ボタンクリックイベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SimilaritySearch_Click(sender As Object, e As EventArgs) Handles SimilaritySearch.Click
        Msg.Text = String.Empty

        'Action の判定
        If Action.Value <> "Search" Then        '--Search以外の場合
            Msg.Text = ERR_INVALID_PARAMETER
            UnDsp_SearchResultList()
            Exit Sub
        End If

        '検索条件:構造式の判定
        If String.IsNullOrEmpty(search_smiles.Value) Then
            '--未入力の場合
            Msg.Text = Common.ERR_NO_MATCH_FOUND
            UnDsp_SearchResultList()
            Exit Sub
        End If

        'リスト部表示設定
        StructureList.Visible = True

        '一覧表示
        If Not SetListData() Then
            '--該当データが存在しない場合
            UnDsp_SearchResultList()
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' StructureListページプロパティ変更時イベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub StructureList_PagePropertiesChanging(sender As Object, e As PagePropertiesChangingEventArgs) Handles StructureList.PagePropertiesChanging

        '一覧表示
        If Not SetListData() Then
            '--該当データが存在しない場合
            UnDsp_SearchResultList()
            Exit Sub
        End If

    End Sub

    ''' <summary>
    ''' 検索結果一覧設定機能
    ''' </summary>
    ''' <returns>True = 正常 、False = エラーあり</returns>
    ''' <remarks></remarks>
    Private Function SetListData() As Boolean
        Dim lst_RegistryNumber As New List(Of String)
        Dim lst_Score As New Dictionary(Of String, String)    'Key = RegistryNumber, Value = Score

        '構造式検索
        If Not Search(lst_RegistryNumber, lst_Score) Then   '--構造式が取得出来ない場合
            Return False
        End If

        'ListView内容の取得・設定（StructureList）
        SearchStructureList(lst_Score)

        Return True

    End Function

    ''' <summary>
    ''' ListView(StructureList)データ検索処理
    ''' </summary>
    ''' <param name="lst_Score">構造式ディクショナリ(RegistryNumber, Similarity)</param>
    ''' <remarks></remarks>
    Protected Sub SearchStructureList(ByVal lst_Score As Dictionary(Of String, String))
        'データテーブル定義
        Dim dt As New DataTable("LineItem")
        dt.Columns.Add("pStructure")
        dt.Columns.Add("Similarity", GetType(Integer))
        dt.Columns.Add("ProductNumber")
        dt.Columns.Add("CASNumber")
        dt.Columns.Add("ProductName")
        dt.Columns.Add("ProductID")
        '検索情報取得
        srcStructure = New TCIDataAccess.Join.StructureSearchDispList
        Dim blnResult As Boolean
        Dim cntRecord As Integer = 0
        For i As Integer = 0 To lst_Score.Count - 1                 '--繰り返し(取得構造式数）
            srcStructure.Load(lst_Score.Keys(i).ToString, lst_Score.Values(i).ToString, Session(Common.SESSION_ROLE_CODE).ToString(), blnResult)
            '検索情報読み込み判定
            Dim dr As DataRow
            If blnResult Then                                       '--読み込めた場合
                Debug.WriteLine(srcStructure.Item(cntRecord).ProductNumber.ToString)
                '検索情報の設定
                dr = dt.NewRow
                dr("pStructure") = Common.NPMSURL & String.Format(IMG_URL_FORMAT, lst_Score.Keys(i).ToString, DateTime.Now)
                dr("Similarity") = lst_Score.Values(i) ' %はViewで出力する
                dr("ProductNumber") = srcStructure.Item(cntRecord).ProductNumber.ToString
                dr("CASNumber") = srcStructure.Item(cntRecord).CASNumber.ToString
                dr("ProductName") = srcStructure.Item(cntRecord).ProductName.ToString
                dr("ProductID") = srcStructure.Item(cntRecord).ProductID.ToString
                dt.Rows.Add(dr)
                cntRecord += 1
                'リスト表示件数を超えた場合は抜ける
                If cntRecord >= Common.LIST_ONEPAGE_ROW("ProductSearchByStructure") Then
                    Exit For
                End If
            End If
        Next
        '並び替え(一致率の大きい順)
        Dim dv As DataView = New DataView(dt)
        dv.Sort = "Similarity DESC"
        dt = dv.ToTable
        'ListView(StructureList)に反映
        StructureList.DataSource = dt
        StructureList.DataBind()
    End Sub

    ''' <summary>
    ''' 構造式検索機能
    ''' </summary>
    ''' <param name="lst_RegistryNumber">[OUT] 新製品登録番号リスト</param>
    ''' <param name="lst_Score">[OUT] スコアリスト</param>
    ''' <returns>True = 正常 、False = エラーあり</returns>
    ''' <remarks></remarks>
    Private Function Search(ByRef lst_RegistryNumber As List(Of String),
                            ByRef lst_Score As Dictionary(Of String, String)) As Boolean

        'Structure.dbファイルの存在判定
        If Not File.Exists(Common.FILE_NAME_STRUCTUREDB) Then      '--存在しない場合
            Return True
        End If

        'Structure.db検索コマンドライン生成
        Dim st_Command As String = String.Format(
                "/c C:\ProgramData\Oracle\Java\javapath\java -jar {0}misearch.jar -db {1} -simisearch -smi {2} -nhits {3} -jme 2>&1",
                Common.FILE_PATH_MISEARCH,
                Common.FILE_NAME_STRUCTUREDB,
                search_smiles.Value,
                NHITS
        )

        'Process インスタンス準備
        Dim psi As New System.Diagnostics.ProcessStartInfo()
        With psi
            .FileName = System.Environment.GetEnvironmentVariable("ComSpec")
            .WorkingDirectory = System.Environment.CurrentDirectory
            .RedirectStandardInput = False
            .RedirectStandardOutput = True
            .RedirectStandardError = True
            .UseShellExecute = False
            .CreateNoWindow = True
            .Arguments = st_Command
        End With

        '取得情報のリスト化（改行文字分割）
        Dim lst_Result As New List(Of String)

        'Process 実行
        Dim st_Results As String = String.Empty
        Using p As System.Diagnostics.Process = System.Diagnostics.Process.Start(psi)
            Dim line As String = String.Empty
            Do While Not p.StandardOutput.EndOfStream
                line = p.StandardOutput.ReadLine()
                ' TABが存在しない行は不要
                If line.Contains(vbTab) Then
                    lst_Result.Add(line)
                    Debug.WriteLine(line)
                End If
                ' 行頭に Exception があった場合は構造検索ができないので終了
                If line.StartsWith("Exception") Then
                    Msg.Text = Common.ERR_STRUCTURE_SEARCH
                    Exit Function
                End If
            Loop
            p.WaitForExit()
        End Using

        '対象情報取得
        For Each st_Line As String In lst_Result                '--取得レコード数分繰り返し
            If st_Line = String.Empty Then Continue For         '--取得行が空の場合、読み飛ばし

            '項目分割(Tab文字)
            Dim st_Split As String() = st_Line.Split(Chr(9))
            '分割数の判定
            If st_Split.Count > 1 Then                          '--分割数が 1 超の場合
                Dim st_RegistryNumber As String = st_Split(1).Trim()
                Dim de_Score As Decimal = 0

                '取得レコード内容判定
                '--検索タイプが -simisearch & 分割数が 3 以上 & 2番目の項目が数値変換可能 & st_RegistryKeyが初期値の場合
                If st_Split.Count >= 3 AndAlso
                   Decimal.TryParse(st_Split(2).Trim(), de_Score) AndAlso Not lst_Score.ContainsKey(st_RegistryNumber) Then
                    'RegistryNumber と対応するスコア（一致率）を格納
                    lst_Score.Add(st_RegistryNumber, String.Format("{0}", Math.Round(de_Score * 100, 0, MidpointRounding.AwayFromZero)))
                End If

                'RegistryNumberリストに登録
                lst_RegistryNumber.Add(st_RegistryNumber)
            End If
        Next

        Return True
    End Function

    ''' <summary>
    ''' 画面表示項目を非表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UnDsp_SearchResultList()

        'リスト部
        StructureList.DataSource = Nothing
        StructureList.Visible = False

    End Sub

End Class