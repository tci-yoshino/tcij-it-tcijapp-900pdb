USE [Purchase]
GO
INSERT INTO [dbo].[Role](
	[RoleCode],
    [Name],
    [CreatedBy],
    [CreateDate],
    [UpdatedBy],
    [UpdateDate]
)
VALUES(
	'WRITE_AA',
    'WRITE_AA',
    0,
    GETDATE(),
    0,
    GETDATE()
)
GO


