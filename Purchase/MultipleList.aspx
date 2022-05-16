<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MultipleList.aspx.vb"Inherits="Purchase.MultipleList" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
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
                        
                        //各テキストボックスに対して値をセット(対象カラムの最大値でカット)
                        targetInput.value = searchWords[i].substring(0,<%=MaxLength%>);

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


            };
        </script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
		<div class="tabs"></div>

        <h3><%=ScreenName%> :</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server"></asp:Label></p>

            <form id="MultipleListForm" runat="server">
                <input type="hidden" id ="Action" runat="server" value="" />

                <div id="multiplelist">
                    <table id="SearchWordTable">
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord1" runat="server" MaxLength="<%#MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord2" runat="server" MaxLength="<%#MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord3" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord4" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord5" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord6" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord7" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord8" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord9" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord10" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord11" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord12" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord13" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord14" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord15" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord16" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord17" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord18" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord19" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord20" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord21" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord22" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord23" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord24" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord25" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord26" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord27" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord28" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord29" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord30" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord31" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord32" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord33" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord34" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord35" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord36" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord37" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord38" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord39" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord40" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord41" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord42" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord43" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord44" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord45" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord46" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord47" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord48" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord49" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord50" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord51" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord52" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord53" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord54" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord55" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord56" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord57" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord58" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord59" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord60" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord61" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord62" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord63" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord64" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord65" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord66" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord67" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord68" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord69" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord70" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord71" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord72" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord73" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord74" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord75" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord76" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord77" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord78" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord79" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord80" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord81" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord82" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord83" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord84" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord85" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord86" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord87" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord88" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord89" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord90" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord91" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord92" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord93" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord94" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord95" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord96" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord97" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord98" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord99" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="SearchWord100" runat="server" MaxLength="<%# MaxLength%>"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <p class="format" style="margin-bottom:10px">(You can specify a value for up to 100 lines)</p>

                <asp:Button ID="ok" runat="server" Text="OK"/>
                <asp:Button ID="cancel" runat="server" Text="Cancel"/>
            </form>
        </div>
    </div><!-- Main Content Area END -->

</body>
</html>