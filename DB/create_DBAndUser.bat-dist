echo off
SET DataBaseName=
SET DBFileDir=D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\
SET/P DataBaseName="作成するデータベース名を入力してください: "

if "%DataBaseName%"=="" exit /b
if "%DBFileDir%"=="" exit /b

sqlcmd -i "create_DBAndUser.sql" -o "create_DBAndUser.log"