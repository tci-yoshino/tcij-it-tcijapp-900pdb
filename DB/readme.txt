==================================================
Purchase データベース構築 & 初期化 SQL スクリプト
                                     Author:akutsu
                           Create date: 2008/08/27
==================================================

【概要】

Purchase のデータベースの構築および初期化する SQL スクリプトです。

【動作環境】

1. 以下のソフトウェアがインストールされていること。
・SQL Server 2005
・Microsoft SQL Server Management Studio (以下、SSMS)

2. SQL Server にログイン Pruchase が作成されていること。

【使用方法】

■ 初期設定

1. create_DBAndUser.bat-dist、start_initialize.bat-dist のコピーを作成し
   ファイル名を create_DBAndUser.bat、start_initialize.bat とする。
2. create_DBAndUser.bat を右クリック - [編集] を選択。
3. 変数 DBFileDir にデータベースの物理的な保存場所を指定する。
   例 : D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\
4. create_DBAndUser.bat を保存し、閉じる。
5. start_initialize.bat を右クリック - [編集] を選択。
6. 変数 ScriptPath がこのスクリプトの存在するフォルダか確認する。
   異なる場合は修正を行う。
   例 : C:\tcijapp\Purchase\DB\
7. start_initialize.bat を保存し、閉じる。

■ Purchase 用データベース構築を行いたい場合

1. create_DBAndUser.bat を実行する。
2. 作成したいデータベース名を指定する。

■ Purchase 用データベースの初期化を行いたい場合

初期化はオブジェクトの再生成、初期データ投入までを行います。

1. start_initialize.bat を 実行する。
2. 初期化したいデータベース名を指定する。

【フォルダ構成】

DB
├ DROP
│ ├ drop_sp.sql        (ストアドの DROP スクリプト)
│ ├ drop_synonym.sql   (シノニムの DROP スクリプト)
│ ├ drop_table.sql     (テーブルの DROP スクリプト)
│ └ drop_view.sql      (ビューの DROP スクリプト)
├ CREATE
│ ├ *.sql      (各オブジェクト名ごとに CREATE スクリプトが存在)
│ ├ create_synonym.sql (シノニムの CREATE スクリプト)
│ └ create.sql (各 CREATE スクリプトを呼び出すスクリプト)
├ INSERT
│ ├ data       (テーブル名ごとにインサートするテキストデータが存在)
│ └ insert.sql (data フォルダのデータをインサートするスクリプト)
├ create_DBAndUser.bat-dist (DB構築バッチ)
├ create_DBAndUser.sql (DB構築スクリプト)
├ create_DBAndUser.log (DB構築実行ログ)
├ start_initialize.bat-dist (DB初期化バッチ)
├ start_initialize.sql (DB初期化スクリプト)
├ start_initialize.log (DB初期化実行ログ)
└ readme.txt    (このファイル)

【CREATE クエリの生成・修正方法】

1. SSMS より、対照オブジェクトを右クリックして以下の手順でクエリを生成する。
   [名前を付けて(オブジェクト名)をスクリプト化] - [CREATE] - [新しい クエリ エディタ ウィンドウ]

2. クエリ内の USE コマンドと次行に記載されている GO コマンドを削除する。

   例)この2行を削除。
   USE オブジェクト名
   GO

3. 最終行に GO コマンドが無い場合は GO コマンドを記述する。
4. [ファイル] - [名前を付けて(クエリファイル名)を保存] で、
   ファイル名を生成するオブジェクト名として保存。
   または既存のファイルに上書き保存する。(*1)
5. 新規保存の場合は 初期化スクリプトのフォルダを開き、
   DB\CREATE\create.sql および DB\DROP\drop_*.sql ファイルを修正する。

(*1) 外部キー、制約、インデックスの CREATE クエリは
     テーブルの CREATE スクリプト内に記載してください。

【INSERT クエリの生成・修正方法】

BULK INSERT を使用し、テキストファイルからのインポートを行っています。
現在インポートしているデータの修正を行いたい場合は、
該当のテキストファイルを直接修正してください。

新規にインポートしたい場合はテキストファイルをテーブル名で作成後、
テキストファイルと同フォルダに存在する insert.sql を開き、
BULK INSERT クエリを追加してください。

テキストファイルの仕様は以下の通り。

■ テキストファイルの仕様

ファイル名           : INSERT するテーブル名
拡張子               : txt
文字コード           : UNICODE (UTF-16)
フィールド区切り文字 : タブ (\t)
行区切り文字         : 改行 (\r\n)

* テーブルのフィールド名と行ごとのデータ内容は一致させてください。
* テーブルのフィールド数と行ごとのデータ数は一致させてください。
* SSMS のデータエクスポート機能で生成したファイルでも対応できます。
  SSMS の機能を使用する際は、上記の仕様に従って生成してください。


