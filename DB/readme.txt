==================================================
Purchase �f�[�^�x�[�X�\�z & ������ SQL �X�N���v�g
                                     Author:akutsu
                           Create date: 2008/08/27
==================================================

�y�T�v�z

Purchase �̃f�[�^�x�[�X�̍\�z����я��������� SQL �X�N���v�g�ł��B

�y������z

1. �ȉ��̃\�t�g�E�F�A���C���X�g�[������Ă��邱�ƁB
�ESQL Server 2005
�EMicrosoft SQL Server Management Studio (�ȉ��ASSMS)

2. SQL Server �Ƀ��O�C�� Pruchase ���쐬����Ă��邱�ƁB

�y�g�p���@�z

�� �����ݒ�

1. create_DBAndUser.bat-dist�Astart_initialize.bat-dist �̃R�s�[���쐬��
   �t�@�C������ create_DBAndUser.bat�Astart_initialize.bat �Ƃ���B
2. create_DBAndUser.bat ���E�N���b�N - [�ҏW] ��I���B
3. �ϐ� DBFileDir �Ƀf�[�^�x�[�X�̕����I�ȕۑ��ꏊ���w�肷��B
   �� : D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\
4. create_DBAndUser.bat ��ۑ����A����B
5. start_initialize.bat ���E�N���b�N - [�ҏW] ��I���B
6. �ϐ� ScriptPath �����̃X�N���v�g�̑��݂���t�H���_���m�F����B
   �قȂ�ꍇ�͏C�����s���B
   �� : C:\tcijapp\Purchase\DB\
7. start_initialize.bat ��ۑ����A����B

�� Purchase �p�f�[�^�x�[�X�\�z���s�������ꍇ

1. create_DBAndUser.bat �����s����B
2. �쐬�������f�[�^�x�[�X�����w�肷��B

�� Purchase �p�f�[�^�x�[�X�̏��������s�������ꍇ

�������̓I�u�W�F�N�g�̍Đ����A�����f�[�^�����܂ł��s���܂��B

1. start_initialize.bat �� ���s����B
2. �������������f�[�^�x�[�X�����w�肷��B

�y�t�H���_�\���z

DB
�� DROP
�� �� drop_sp.sql        (�X�g�A�h�� DROP �X�N���v�g)
�� �� drop_synonym.sql   (�V�m�j���� DROP �X�N���v�g)
�� �� drop_table.sql     (�e�[�u���� DROP �X�N���v�g)
�� �� drop_view.sql      (�r���[�� DROP �X�N���v�g)
�� CREATE
�� �� *.sql      (�e�I�u�W�F�N�g�����Ƃ� CREATE �X�N���v�g������)
�� �� create_synonym.sql (�V�m�j���� CREATE �X�N���v�g)
�� �� create.sql (�e CREATE �X�N���v�g���Ăяo���X�N���v�g)
�� INSERT
�� �� data       (�e�[�u�������ƂɃC���T�[�g����e�L�X�g�f�[�^������)
�� �� insert.sql (data �t�H���_�̃f�[�^���C���T�[�g����X�N���v�g)
�� create_DBAndUser.bat-dist (DB�\�z�o�b�`)
�� create_DBAndUser.sql (DB�\�z�X�N���v�g)
�� create_DBAndUser.log (DB�\�z���s���O)
�� start_initialize.bat-dist (DB�������o�b�`)
�� start_initialize.sql (DB�������X�N���v�g)
�� start_initialize.log (DB���������s���O)
�� readme.txt    (���̃t�@�C��)

�yCREATE �N�G���̐����E�C�����@�z

1. SSMS ���A�ΏƃI�u�W�F�N�g���E�N���b�N���Ĉȉ��̎菇�ŃN�G���𐶐�����B
   [���O��t����(�I�u�W�F�N�g��)���X�N���v�g��] - [CREATE] - [�V���� �N�G�� �G�f�B�^ �E�B���h�E]

2. �N�G������ USE �R�}���h�Ǝ��s�ɋL�ڂ���Ă��� GO �R�}���h���폜����B

   ��)����2�s���폜�B
   USE �I�u�W�F�N�g��
   GO

3. �ŏI�s�� GO �R�}���h�������ꍇ�� GO �R�}���h���L�q����B
4. [�t�@�C��] - [���O��t����(�N�G���t�@�C����)��ۑ�] �ŁA
   �t�@�C�����𐶐�����I�u�W�F�N�g���Ƃ��ĕۑ��B
   �܂��͊����̃t�@�C���ɏ㏑���ۑ�����B(*1)
5. �V�K�ۑ��̏ꍇ�� �������X�N���v�g�̃t�H���_���J���A
   DB\CREATE\create.sql ����� DB\DROP\drop_*.sql �t�@�C�����C������B

(*1) �O���L�[�A����A�C���f�b�N�X�� CREATE �N�G����
     �e�[�u���� CREATE �X�N���v�g���ɋL�ڂ��Ă��������B

�yINSERT �N�G���̐����E�C�����@�z

BULK INSERT ���g�p���A�e�L�X�g�t�@�C������̃C���|�[�g���s���Ă��܂��B
���݃C���|�[�g���Ă���f�[�^�̏C�����s�������ꍇ�́A
�Y���̃e�L�X�g�t�@�C���𒼐ڏC�����Ă��������B

�V�K�ɃC���|�[�g�������ꍇ�̓e�L�X�g�t�@�C�����e�[�u�����ō쐬��A
�e�L�X�g�t�@�C���Ɠ��t�H���_�ɑ��݂��� insert.sql ���J���A
BULK INSERT �N�G����ǉ����Ă��������B

�e�L�X�g�t�@�C���̎d�l�͈ȉ��̒ʂ�B

�� �e�L�X�g�t�@�C���̎d�l

�t�@�C����           : INSERT ����e�[�u����
�g���q               : txt
�����R�[�h           : UNICODE (UTF-16)
�t�B�[���h��؂蕶�� : �^�u (\t)
�s��؂蕶��         : ���s (\r\n)

* �e�[�u���̃t�B�[���h���ƍs���Ƃ̃f�[�^���e�͈�v�����Ă��������B
* �e�[�u���̃t�B�[���h���ƍs���Ƃ̃f�[�^���͈�v�����Ă��������B
* SSMS �̃f�[�^�G�N�X�|�[�g�@�\�Ő��������t�@�C���ł��Ή��ł��܂��B
  SSMS �̋@�\���g�p����ۂ́A��L�̎d�l�ɏ]���Đ������Ă��������B


