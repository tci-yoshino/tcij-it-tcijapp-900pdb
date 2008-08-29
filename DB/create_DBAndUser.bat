echo off
SET DataBaseName=
SET DBFileDir=
SET/P DataBaseName="作成するデータベース名を入力してください: "
SET/P DBFileDir="テーブルログディレクトリを入力してください: "

if %DBFileDir% == "" (	
  exit/b
)
sqlcmd -i "create_DBAndUser.sql" -o "create_DBAndUser.log"