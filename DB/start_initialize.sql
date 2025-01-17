-- 【概要】
-- Purchase 用のデータベースを初期化します。
-- 
-- 【変数説明】
-- ScriptPath   : このファイルが存在するディレクトリ(フルパス)
-- DataBaseName : 初期化するデータベース名
-- 
-- 【使用方法】
-- 1. 変数を設定してください。
-- 2. 接続先データベースサーバを確認、もしくはデータベースサーバに接続してください。
-- 3. メニューバーから [クエリ] - [SQLCMDモード] をチェック。
-- 4. スクリプトを実行させてください。

-- :setvar ScriptPath C:\Initialized_Purchase_Database\
-- :setvar DataBaseName AKUTSU_TEST
:on error exit

USE $(DataBaseName)
GO

-- DROP
:r $(ScriptPath)DROP\drop_sp.sql
:r $(ScriptPath)DROP\drop_synonym.sql
:r $(ScriptPath)DROP\drop_view.sql
:r $(ScriptPath)DROP\drop_table.sql
GO

-- CREATE
:r $(ScriptPath)CREATE\create.sql
GO

-- INSERT
:r $(ScriptPath)INSERT\insert.sql
GO
