-- 【概要】
-- Purchase 用のデータベースとユーザを作成します。
-- 
-- 【注意事項】
-- ユーザ作成はログイン Purchase が存在していることを前提としていますので、
-- ログイン Purchase が存在しない場合は別途ログインを作成してください。
--
-- 【変数説明】
-- ScriptPath   : このファイルが存在するディレクトリ(フルパス)
-- DataBaseName : 作成するデータベース名
-- DBFileDir    : プライマリ、ログファイルの保存ディレクトリ(フルパス)
--                開発サーバ(hs-sys0255)の場合
--                D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\
-- 【使用方法】
-- 1. 変数を設定してください。
-- 2. 接続先データベースサーバを確認、もしくはデータベースサーバに接続してください。
-- 3. メニューバーから [クエリ] - [SQLCMDモード] をチェック。
-- 4. スクリプトを実行させてください。

--:setvar ScriptPath C:\Initialized_Purchase_Database\
--:setvar DataBaseName AKUTSU_TEST
--:setvar DBFileDir "C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\"
:on error exit

USE [master]
GO

/****** オブジェクト:  Database $(DataBaseName)    スクリプト日付: 08/29/2008 09:57:48 ******/
CREATE DATABASE $(DataBaseName) ON  PRIMARY 
( NAME = N'$(DataBaseName)', FILENAME = N'$(DBFileDir)$(DataBaseName).mdf' , SIZE = 102400KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'$(DataBaseName)_log', FILENAME = N'$(DBFileDir)$(DataBaseName)_log.ldf' , SIZE = 57664KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Japanese_CI_AS
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'$(DataBaseName)', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC $(DataBaseName).[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE $(DataBaseName) SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE $(DataBaseName) SET ANSI_NULLS OFF 
GO
ALTER DATABASE $(DataBaseName) SET ANSI_PADDING OFF 
GO
ALTER DATABASE $(DataBaseName) SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE $(DataBaseName) SET ARITHABORT OFF 
GO
ALTER DATABASE $(DataBaseName) SET AUTO_CLOSE OFF 
GO
ALTER DATABASE $(DataBaseName) SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE $(DataBaseName) SET AUTO_SHRINK OFF 
GO
ALTER DATABASE $(DataBaseName) SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE $(DataBaseName) SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE $(DataBaseName) SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE $(DataBaseName) SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE $(DataBaseName) SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE $(DataBaseName) SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE $(DataBaseName) SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE $(DataBaseName) SET  DISABLE_BROKER 
GO
ALTER DATABASE $(DataBaseName) SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE $(DataBaseName) SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE $(DataBaseName) SET TRUSTWORTHY OFF 
GO
ALTER DATABASE $(DataBaseName) SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE $(DataBaseName) SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE $(DataBaseName) SET  READ_WRITE 
GO
ALTER DATABASE $(DataBaseName) SET RECOVERY FULL 
GO
ALTER DATABASE $(DataBaseName) SET  MULTI_USER 
GO
ALTER DATABASE $(DataBaseName) SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE $(DataBaseName) SET DB_CHAINING OFF 
GO

USE $(DataBaseName)
GO

/****** オブジェクト:  User [Purchase]    スクリプト日付: 08/29/2008 10:09:14 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'Purchase')
CREATE USER [Purchase] FOR LOGIN [Purchase] WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_owner', N'Purchase'
GO
