echo off
SET DataBaseName=
SET ScliptPath=C:\tcijapp\Purchase\DB\
SET/P DataBaseName="データベース名を入力してください: "

if "%DataBaseName%"=="" exit /b

sqlcmd -i "start_initialize.sql" -o "start_initialize.log"