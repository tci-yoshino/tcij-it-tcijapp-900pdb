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

�y�g�p���@�z

�� Purchase �p�f�[�^�x�[�X�\�z���s�������ꍇ

1. create_DBAndUser.bat ���G�f�B�^�ŊJ���B
2. �ϐ� DBFileDir �Ƀf�[�^�x�[�X�t�@�C�������
   �f�[�^�x�[�X���O�t�@�C����ۑ�����p�X���w�肷��B
   * �J���T�[�o�̏ꍇ�͈ȉ��̂Ƃ���B
     D:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\
3. create_DBAndUser.bat ��ۑ�����B
4. create_DBAndUser.bat �����s����B
5. �쐬�������f�[�^�x�[�X�����w�肷��B

�� Purchase �p�f�[�^�x�[�X�̏��������s�������ꍇ

1. start_initialize.bat ���G�f�B�^�ŊJ���B
2. �ϐ� ScliptPath �����̃X�N���v�g�̑��݂���t�H���_���m�F����B
   * �J���T�[�o�̏ꍇ�͈ȉ��̂Ƃ���B
     C:\tcijapp\Purchase\DB\
3. start_initialize.bat �� ���s����B
4. �쐬�������f�[�^�x�[�X�����w�肷��B

�y�t�H���_�\���z
Initialized_Purchase_Database
�� DROP
�� �� �e DROP �X�N���v�g
�� CREATE
�� �� *.sql      (�e�I�u�W�F�N�g�����Ƃ� CREATE �X�N���v�g������)
�� �� create_synonym.sql (�V�m�j���� CREATE �X�N���v�g)
�� �� create.sql (�e CREATE �X�N���v�g���Ăяo���X�N���v�g)
�� INSERT
�� �� data       (�e�[�u�������ƂɃC���|�[�g���� .txt �f�[�^������)
�� �� insert.sql (data �t�H���_�̃f�[�^���C���|�[�g����X�N���v�g)
�� create_DBAndUser.bat (DB�\�z�o�b�`)
�� create_DBAndUser.sql (DB�\�z�X�N���v�g)
�� create_DBAndUser.log (DB�\�z���s���O)
�� start_initialize.bat (DB�������o�b�`)
�� start_initialize.sql (DB�������X�N���v�g)
�� start_initialize.log (DB���������s���O)
�� readme.txt    (���̃t�@�C��)

* log �t�@�C���͊e�o�b�`�����s���ꂽ���ɐ�������܂��B
  ���ł�log �t�@�C�������݂���ꍇ�͏㏑������܂��B

�yINSERT �f�[�^�t�@�C���̎d�l�z

�����R�[�h           : UNICODE (UTF-16)
�t�B�[���h��؂蕶�� : �^�u (\t)
�s��؂蕶��         : ���s (\r\n)

