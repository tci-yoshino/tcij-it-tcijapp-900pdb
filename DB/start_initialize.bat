echo off
SET DataBaseName=
SET ScliptPath=C:\tcijapp\Purchase\DB\
SET/P DataBaseName="�f�[�^�x�[�X������͂��Ă�������: "
sqlcmd -i "start_initialize.sql" -o "start_initialize.log"