USE [Purchase]
GO
UPDATE RFQStatus SET SortOrder=7 WHERE RFQStatusCode='C'
INSERT INTO [dbo].[RFQStatus] (
	[RFQStatusCode],
    [Text],
    [SortOrder]
)
VALUES (
	'II',
    'Interface Issued',
    6
)
GO


