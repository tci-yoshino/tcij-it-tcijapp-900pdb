-- �y�T�v�z
-- Purchase �p�̃f�[�^�x�[�X�����������܂��B
-- 
-- �y�ϐ������z
-- ScliptPath   : ���̃t�@�C�������݂���f�B���N�g��(�t���p�X)
-- DataBaseName : ����������f�[�^�x�[�X��
-- 
-- �y�g�p���@�z
-- 1. �ϐ���ݒ肵�Ă��������B
-- 2. �ڑ���f�[�^�x�[�X�T�[�o���m�F�A�������̓f�[�^�x�[�X�T�[�o�ɐڑ����Ă��������B
-- 3. ���j���[�o�[���� [�N�G��] - [SQLCMD���[�h] ���`�F�b�N�B
-- 4. �X�N���v�g�����s�����Ă��������B

-- :setvar ScliptPath C:\Initialized_Purchase_Database\
-- :setvar DataBaseName AKUTSU_TEST

USE $(DataBaseName)
GO

-- DROP
:r $(ScliptPath)DROP\drop_sp.sql
:r $(ScliptPath)DROP\drop_synonym.sql
:r $(ScliptPath)DROP\drop_view.sql
:r $(ScliptPath)DROP\drop_table.sql
GO

-- CREATE
:r $(ScliptPath)CREATE\create.sql
GO

-- INSERT
:r $(ScliptPath)INSERT\insert.sql
GO
