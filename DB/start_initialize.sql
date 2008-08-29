-- 【概要】
-- Purchase 用のデータベースを初期化します。
-- 
-- 【変数説明】
-- ScliptPath   : このファイルが存在するディレクトリ(フルパス)
-- DataBaseName : 初期化するデータベース名
-- 
-- 【使用方法】
-- 1. 変数を設定してください。
-- 2. 接続先データベースサーバを確認、もしくはデータベースサーバに接続してください。
-- 3. メニューバーから [クエリ] - [SQLCMDモード] をチェック。
-- 4. スクリプトを実行させてください。

-- :setvar ScliptPath C:\Initialized_Purchase_Database\
-- :setvar DataBaseName AKUTSU_TEST

USE $(DataBaseName)
GO

-- DROP
:r $(ScliptPath)DROP\drop_sp.sql
:r $(ScliptPath)DROP\drop_synonym.sql
:r $(ScliptPath)DROP\drop_view.sql
:r $(ScliptPath)DROP\drop_table.sql
GO

-- CREATE
:r $(ScliptPath)CREATE\create.sql
GO

-- INSERT
:r $(ScliptPath)INSERT\insert.sql
GO
