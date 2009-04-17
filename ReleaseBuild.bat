echo OFF
cls
rem TortoisSVN�v���O�����p�X�̐ݒ�
path C:\Program Files\TortoiseSVN\bin;%path%

rem ���ϐ��̐ݒ�
set WORK_DIR=""
set WORK_DIR_PROMPT=""
set SVN_URL=svn+ssh://tcisyslog/data/svn/tcijapp/Purchase
rem �P�̃e�X�g�p���[�J��SVN�p�X�ݒ�
rem set SVN_URL=file:///D:/nokuda_workspace/SVNRepository/Purchase

:WorkDirectoryPathPrompt

rem ��ƃf�B���N�g���̐ݒu�p�X�ݒ�
set BASE_DIR=C:
set /P BASE_DIR="��ƃf�B���N�g�����쐬����p�X����͂��Ă�������(�����l C:\):"

rem ������[\]���폜���܂�
if "%BASE_DIR:~-1%"=="\" (
	set BASE_DIR=%BASE_DIR:~0,-1%
)

if not exist %BASE_DIR% (
 	echo %BASE_DIR% ��������܂���B
	goto WorkDirectoryPathPrompt
)

rem ��Ɨp�f�B���N�g���̍쐬
set WORK_DIR=%BASE_DIR%\Purchase_SVNWork

if not exist %WORK_DIR% goto WorkDirectoryCreate

:WorkDirectoryExistPrompt
set /P WORK_DIR_PROMPT="��Ɨp�f�B���N�g�������ɑ��݂��܂�(d:�폜  a:���f):"

if "%WORK_DIR_PROMPT%"=="d" (
	rd /S /Q %WORK_DIR%
	ping -n 2 localhost >NUL
	goto WorkDirectoryCreate
)
if "%WORK_DIR_PROMPT%"=="a" (
	echo �����͒��f����܂���
	pause
	exit
)
goto WorkDirectoryExistPrompt

:WorkDirectoryCreate

echo ��Ɨp�f�B���N�g���쐬�J�n
MD %WORK_DIR%
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo ��Ɨp�f�B���N�g���쐬�I��

rem SVN�`�F�b�N�A�E�g�R�}���h
echo SVN�`�F�b�N�A�E�g�_�C�A���O�\���J�n
TortoiseProc /command:checkout /url:%SVN_URL%/trunk/ /path:%WORK_DIR% /closeonend:1
if not ERRORLEVEL 0	goto ErrorProcessEnd
echo SVN�`�F�b�N�A�E�g�_�C�A���O�\���I��

rem WebConfig�R�s�[
echo WebConfig�R�s�[�J�n
copy %WORK_DIR%\Purchase\Web.config-dist %WORK_DIR%\Purchase\Web.config
echo WebConfig�R�s�[�I��

rem MSBuild�R���p�C���R�}���h
echo MSBuilde�R���p�C���J�n
msbuild.exe "%WORK_DIR%\Purchase\Purchase.vbproj"
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo MSBuilde�R���p�C���I��

rem SVN�R�s�[�R�}���h
rem logmsg��Version 1.5.0RC1�ȍ~�ŗL��
echo SVN�^�O�ǉ��_�C�A���O�\���J�n
TortoiseProc /command:copy /url:%SVN_URL%/tags/ver-x.x.x /path:%WORK_DIR% /logmsg:"tags message here" /closeonend:1
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo SVN�^�O�ǉ��_�C�A���O�\���I��

rem SVN�Ǘ����t�@�C���ǉ�
echo SVN�Ǘ��t�@�C���ǉ��J�n
TortoiseProc /command:add /path:%WORK_DIR%\Purchase\bin*%WORK_DIR%\Purchase\obj /notempfile /closeonend:1
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo SVN�Ǘ��t�@�C���ǉ��I��

rem SVN�R�~�b�g
echo SVN�R�~�b�g�_�C�A���O�\���J�n
TortoiseProc /command:commit /path:%WORK_DIR%\ /logmsg:"commit message here" /notempfile /closeonend:0
if not ERRORLEVEL 0 goto ErrorProcessEnd
echo SVN�R�~�b�g�_�C�A���O�\���I��

rem ��ƃf�B���N�g���̍폜
echo ��ƃf�B���N�g���폜�J�n
rd /S /Q %WORK_DIR%
ping -n 2 localhost >NUL
echo ��ƃf�B���N�g���폜�I��

:ProcessEnd
echo �������I�����܂����B
pause
exit

:ErrorProcessEnd
echo �������ُ�I�����܂����B
pause
exit
