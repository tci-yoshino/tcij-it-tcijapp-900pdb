==================================================
Purchase �f�[�^�x�[�X�\�z & ������ SQL �X�N���v�g
                                     Author:akutsu
                           Create date: 2008/08/27
==================================================

�y�T�v�z

Purchase �̃f�[�^�x�[�X�̍\�z����я��������� SQL �X�N���v�g�ł��B

�y�K�{�����z

1. �ȉ��̃\�t�g�E�F�A���C���X�g�[������Ă��邱�ƁB
�ESQL Server 2005
�EMicrosoft SQL Server Management Studio (�ȉ��ASSMS)

�y�g�p���@�z

�� �����ݒ�
1. create_DBAndUser.bat ���E�N���b�N - [�ҏW] ��I���B
2. �ϐ� DBFileDir �Ƀf�[�^�x�[�X�t�@�C�������
   �f�[�^�x�[�X���O�t�@�C����ۑ�����p�X���w�肷��B
   * SSMS �Ńf�[�^�x�[�X�� CREATE ���쐬���� PRIMARY ��Ŋm�F�ł��܂��B
   * �J���T�[�o�̏ꍇ�͈ȉ��̂Ƃ���B
     D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\
3. create_DBAndUser.bat ��ۑ����A����B
4. start_initialize.bat ���E�N���b�N - [�ҏW] ��I���B
5. �ϐ� ScliptPath �����̃X�N���v�g�̑��݂���t�H���_���m�F����B
   �قȂ�ꍇ�͏C�����s���B
   * �J���T�[�o�̏ꍇ�͈ȉ��̂Ƃ���B
     C:\tcijapp\Purchase\DB\
6. start_initialize.bat ��ۑ����A����B

�� Purchase �p�f�[�^�x�[�X�\�z���s�������ꍇ

1. create_DBAndUser.bat �����s����B
2. �쐬�������f�[�^�x�[�X�����w�肷��B

�� Purchase �p�f�[�^�x�[�X�̏��������s�������ꍇ

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
�� �� create_synonym.sql (�V�m�j���� CREATE �X�N���v�g(*1))
�� �� create.sql (�e CREATE �X�N���v�g���Ăяo���X�N���v�g)
�� INSERT
�� �� data       (�e�[�u�������ƂɃC���T�[�g����e�L�X�g�f�[�^������)
�� �� insert.sql (data �t�H���_�̃f�[�^���C���T�[�g����X�N���v�g)
�� create_DBAndUser.bat (DB�\�z�o�b�`)
�� create_DBAndUser.sql (DB�\�z�X�N���v�g)
�� create_DBAndUser.log (DB�\�z���s���O(*2))
�� start_initialize.bat (DB�������o�b�`)
�� start_initialize.sql (DB�������X�N���v�g)
�� start_initialize.log (DB���������s���O(*2))
�� readme.txt    (���̃t�@�C��)

(*1) �V�m�j���� CREATE ���͂ЂƂ̃t�@�C���ɓZ�߂��Ă��܂��B
(*2) log �t�@�C���͊e�o�b�`�����s���ꂽ���ɐ�������܂��B
     ���ł�log �t�@�C�������݂���ꍇ�͏㏑������܂��B


�yDROP �N�G���̐����E�C�����@�z

1. SSMS ���A�ΏƃI�u�W�F�N�g���E�N���b�N���Ĉȉ��̎菇�ŃN�G���𐶐�����B
   [���O��t����(�I�u�W�F�N�g��)���X�N���v�g��] - [DROP] - [�V���� �N�G�� �G�f�B�^ �E�B���h�E]

2. �N�G������ USE �R�}���h�Ǝ��s�ɋL�ڂ���Ă��� GO �R�}���h���폜����B

   ��)����2�s���폜�B
   USE �I�u�W�F�N�g��
   GO

3. �ŏI�s�� GO �R�}���h�������ꍇ�� GO �R�}���h���L�q����B
4. �N�G����S�s�R�s�[����B
5. �������X�N���v�g�̃t�H���_���J���A DB\DROP\ �Ɉړ�����B
6. �e�I�u�W�F�N�g�ʂ� DROP �t�@�C�������݂���̂ŁA�Ώۂ̃t�@�C�����J���B
7. 3. �ŃR�s�[�����N�G������L�ŊJ�����t�@�C���ɒǉ��܂��͏C������B

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
   DB\CREATE\create.sql �t�@�C�����C������B

(*1) FK�ACH�A�C���f�b�N�X�� CREATE �N�G����
     �e�[�u���� CREATE �X�N���v�g���ɋL�ڂ��Ă��������B

�yINSERT �N�G���̐����E�C�����@�z

BULK INSERT ���g�p���A�e�L�X�g�t�@�C������̃C���T�[�g���s���Ă��܂��B
���݃C���T�[�g���Ă���f�[�^�̏C�����s�������ꍇ�́A
�Y���̃e�L�X�g�t�@�C���𒼐ڏC�����Ă��������B

�V�K�ɃC���T�[�g�������ꍇ�̓e�L�X�g�t�@�C�����e�[�u�����ō쐬��A
�e�L�X�g�t�@�C���Ɠ��t�H���_�ɑ��݂��� insert.sql ���J���A
BULK INSET �N�G����ǉ����Ă��������B

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
  SSMS �̋@�\���g�p����ۂ́A��L�̎d�l�ɏ]���Đ������Ă��������B�B


