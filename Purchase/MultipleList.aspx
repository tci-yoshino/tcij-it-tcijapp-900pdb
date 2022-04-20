<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MultipleList.aspx.vb"Inherits="Purchase.MultipleList" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-y: hidden;">
    <head runat="server">
        <title><%=ScreenName%></title>
        <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
        <script type="text/javascript" src="./JS/Common.js"></script>
        <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript">
            window.onload = function () {
                const separater = "|";
                //ペースト実行時
                document.addEventListener('paste', function (e) {
                    // 入力エリアにフォーカスが当たっていない場合処理を中断する
                    if (document.activeElement.tagName !== "INPUT") {
                        // 処理を中断する
                        return false;
                    }
                    
                    //デフォルトの動作をキャンセル
                    e.preventDefault();
                    
                    //クリップボードの文字列を変数に格納
                    var searchWords = e.clipboardData.getData('Text').split('\r\n');

                    // Excelからコピーした際に、最終行に改行コードが入るため削除する
                    if (searchWords[searchWords.length - 1].length === 0) {
                        searchWords.pop()
                    }
                    // 画面メッセージを初期化
                    document.getElementById('Msg').innerText = '';
                    
                    let targetInput = document.activeElement

                    //テキストボックスへの出力
                    for (var i = 0; i < searchWords.length; i++) {
                        //各テキストボックスに対して値をセット
                        targetInput.value = searchWords[i];

                        // 親要素のtrタグを取得
                        let parentTR = targetInput.parentNode.parentNode
                        
                        // 次のinputがない場合は処理を中断する
                        if (parentTR.nextElementSibling === null) {
                            // searchWordsが最後でない場合はペーストデータに欠落があるためメッセージを表示する
                            if (searchWords[i + 1] !== undefined) {
                                document.getElementById('Msg').innerText = '<%=Purchase.Common.ERR_ITEMS_OVER_100%>';
                            }
                            break
                        }
                        // 次のinputを取得
                        targetInput = parentTR.nextElementSibling.children[0].children[0]
                    };
                });
                document.getElementById("ok").onclick = function () {
                    //親画面に対して動作
                    if (opener) {
                        let st_MultipleValue = "";

                        //テキストボックスに入力された値をセパレーター文字列区切りにして変数にセット
                        for (let i = 1; i <= 100; i++) {
                            // テキストボックスの値を取得 
                            let st_ItemID = "SearchWord" + i;
                            st_TargetValue = document.getElementById(st_ItemID).value;
                            
                            // 値が空の場合次のテキストボックスに移る
                            if (st_TargetValue.length === 0) {
                                continue;
                            }

                            // 値の追加
                            if (st_MultipleValue.length === 0) {
                                // 先頭はセパレーター不要
                                st_MultipleValue = st_MultipleValue + st_TargetValue;
                            } else {
                                // ２つめ以降はセパレーター＋値
                                st_MultipleValue = st_MultipleValue + separater + st_TargetValue;
                            };
                        }

                        //呼び出し元画面のテキストボックスにセパレーター区切りにした値をセット
                        opener.document.getElementById('<%=st_SearchItemId%>').value = st_MultipleValue;
                    };
                    window.close();
                };
                document.getElementById("cancel").onclick = function () {
                    window.close();
                }

                //親画面のRFQNumberテキストボックスに値が入力された状態で子画面を開いた場合、
                //親画面で入力されている値を子画面のテキストボックスに表示する
                var RFQNumber = window.opener.document.getElementById("<%=st_SearchItemId%>");

                if (RFQNumber.value) {
                    //親画面のRFQNumberテキストボックスに入力されている値をカンマ区切りで配列にセット
                    var RFQNumberArray = [];
                    RFQNumberArray = RFQNumber.value.split(separater);

                    //子画面のテキストボックスIDの設定
                    var i_Count = 1;
                    var st_ItemID = "RFQReferenceNumber" + i_Count;
                    var st_RFQReferenceNumberValue = document.getElementById(st_ItemID).value;
                    var Table = document.getElementById('SearchWordTable');

                    //配列にある値の分だけ、子画面の各テキストボックスに RFQReferenceNumber の値をセット
                    for (var i = 0, Len = RFQNumberArray.length; i < Len; i++) {
                        //配列に値がない場合、処理を終了
                        if (!RFQNumberArray[i]) {
                            break;
                        };
                        Table.rows[i].children[0].children[0].value = RFQNumberArray[i];
                    };
                };
            };
        </script>
    </head>
    
    <body>
        <h3 id="ScreenName"><%=ScreenName%></h3>
        <p class="attention">
            <asp:Label ID="Msg" runat="server"></asp:Label>
        </p>
        <form id="MultipleListForm" runat="server">
            <input type="hidden" id ="Action" runat="server" value="" />
            <div style="height:400px;overflow-y:scroll;">
                <table id="SearchWordTable">
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord1" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord2" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord3" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord4" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord5" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord6" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord7" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord8" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord9" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord10" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord11" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord12" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord13" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord14" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord15" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord16" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord17" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord18" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord19" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord20" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord21" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord22" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord23" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord24" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord25" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord26" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord27" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord28" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord29" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord30" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord31" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord32" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord33" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord34" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord35" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord36" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord37" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord38" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord39" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord40" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord41" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord42" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord43" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord44" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord45" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord46" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord47" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord48" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord49" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord50" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord51" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord52" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord53" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord54" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord55" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord56" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord57" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord58" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord59" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord60" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord61" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord62" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord63" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord64" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord65" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord66" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord67" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord68" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord69" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord70" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord71" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord72" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord73" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord74" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord75" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord76" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord77" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord78" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord79" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord80" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord81" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord82" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord83" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord84" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord85" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord86" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord87" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord88" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord89" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord90" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord91" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord92" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord93" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord94" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord95" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord96" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord97" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord98" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord99" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="SearchWord100" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>

                </table>
            </div>
            <div class="btns" style="text-align: left">
                <asp:Button ID="ok" runat="server" Text="OK"/>
                <asp:Button ID="cancel" runat="server" Text="Cancel"/>
            </div>

        </form>
    </body>
</html>