echo OFF
cls
rem TortoisSVNプログラムパスの設定
path C:\Program Files\TortoiseSVN\bin;%path%

rem 環境変数の設定
set WORK_DIR=""
set WORK_DIR_PROMPT=""
set SVN_URL=svn+ssh://tcisyslog/data/svn/tcijapp/Purchase
rem 単体テスト用ローカルSVNパス設定
rem set SVN_URL=file:///D:/nokuda_workspace/SVNRepository/Purchase

:WorkDirectoryPathPrompt

rem 作業ディレクトリの設置パス設定
set BASE_DIR=C:
set /P BASE_DIR="作業ディレクトリを作成するパスを入力してください(初期値 C:\):"

rem 末尾の[\]を削除します
if "%BASE_DIR:~-1%"=="\" (
	set BASE_DIR=%BASE_DIR:~0,-1%
)

if not exist %BASE_DIR% (
 	echo %BASE_DIR% が見つかりません。
	goto WorkDirectoryPathPrompt
)

rem 作業用ディレクトリの作成
set WORK_DIR=%BASE_DIR%\Purchase_SVNWork

if not exist %WORK_DIR% goto WorkDirectoryCreate

:WorkDirectoryExistPrompt
set /P WORK_DIR_PROMPT="作業用ディレクトリが既に存在します(d:削除  a:中断):"

if "%WORK_DIR_PROMPT%"=="d" (
	rd /S /Q %WORK_DIR%
	ping -n 2 localhost >NUL
	goto WorkDirectoryCreate
)
if "%WORK_DIR_PROMPT%"=="a" (
	echo 処理は中断されました
	pause
	exit
)
goto WorkDirectoryExistPrompt

:WorkDirectoryCreate

echo 作業用ディレクトリ作成開始
MD %WORK_DIR%
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo 作業用ディレクトリ作成終了

rem SVNチェックアウトコマンド
echo SVNチェックアウトダイアログ表示開始
TortoiseProc /command:checkout /url:%SVN_URL%/trunk/ /path:%WORK_DIR% /closeonend:1
if not ERRORLEVEL 0	goto ErrorProcessEnd
echo SVNチェックアウトダイアログ表示終了

rem WebConfigコピー
echo WebConfigコピー開始
copy %WORK_DIR%\Purchase\Web.config-dist %WORK_DIR%\Purchase\Web.config
echo WebConfigコピー終了

rem MSBuildコンパイルコマンド
echo MSBuildeコンパイル開始
msbuild.exe "%WORK_DIR%\Purchase\Purchase.vbproj"
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo MSBuildeコンパイル終了

rem SVNコピーコマンド
rem logmsgはVersion 1.5.0RC1以降で有効
echo SVNタグ追加ダイアログ表示開始
TortoiseProc /command:copy /url:%SVN_URL%/tags/ver-x.x.x /path:%WORK_DIR% /logmsg:"tags message here" /closeonend:1
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo SVNタグ追加ダイアログ表示終了

rem SVN管理下ファイル追加
echo SVN管理ファイル追加開始
TortoiseProc /command:add /path:%WORK_DIR%\Purchase\bin*%WORK_DIR%\Purchase\obj /notempfile /closeonend:1
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo SVN管理ファイル追加終了

rem SVNコミット
echo SVNコミットダイアログ表示開始
TortoiseProc /command:commit /path:%WORK_DIR%\ /logmsg:"commit message here" /notempfile /closeonend:0
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo SVNコミットダイアログ表示終了

rem 作業ディレクトリの削除
echo 作業ディレクトリ削除開始
rd /S /Q %WORK_DIR%
ping -n 2 localhost >NUL
echo 作業ディレクトリ削除終了

:ProcessEnd
echo 処理が終了しました。
pause
exit

:ErrorProcessEnd
echo 処理が異常終了しました。
pause
exit
