==================================================
Purchase データベース構築 & 初期化 SQL スクリプト
                                     Author:akutsu
                           Create date: 2008/08/27
==================================================

【概要】

Purchase のデータベースの構築および初期化する SQL スクリプトです。

【必須条件】

1. 以下のソフトウェアがインストールされていること。
・SQL Server 2005

【使用方法】

まず、このプログラムのフォルダをデータベースサーバの
C ドライブの直下に置き、フォルダを開く。

■ Purchase 用データベース構築を行いたい場合

1. create_DBAndUser.bat を 実行する。
2. 作成したいデータベース名を指定する。
3. データベースファイルおよびデータベースログファイルを保存するパスを指定する。
   * 開発サーバの場合は以下のとおり。
     D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\


■ Purchase 用データベースの初期化を行いたい場合

1. create_DBAndUser.bat を 実行する。
2. 作成したいデータベース名を指定する。

【フォルダ構成】
Initialized_Purchase_Database
├ DROP
│ └ 各 DROP スクリプト
├ CREATE
│ ├ *.sql      (各オブジェクト名ごとに CREATE スクリプトが存在)
│ ├ create_synonym.sql (シノニムの CREATE スクリプト)
│ └ create.sql (各 CREATE スクリプトを呼び出すスクリプト)
├ INSERT
│ ├ data       (テーブル名ごとにインポートする .txt データが存在)
│ └ insert.sql (data フォルダのデータをインポートするスクリプト)
├ create_DBAndUser.sql (DB構築スクリプト)
├ start_initialize.sql (DB初期化スクリプト)
└ readme.txt    (このファイル)

【INSERT データファイルの仕様】

文字コード           : UNICODE (UTF-16)
フィールド区切り文字 : タブ (\t)
行区切り文字         : 改行 (\r\n)

