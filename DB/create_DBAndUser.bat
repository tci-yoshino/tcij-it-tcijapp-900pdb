echo off
SET DataBaseName=
SET DBFileDir=
SET/P DataBaseName="�쐬����f�[�^�x�[�X������͂��Ă�������: "
SET/P DBFileDir="�e�[�u�����O�f�B���N�g������͂��Ă�������: "

sqlcmd -i "create_DBAndUser.sql" -o "create_DBAndUser.log"