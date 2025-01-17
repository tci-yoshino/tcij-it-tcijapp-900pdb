/****** オブジェクト:  View [dbo].[v_UserAll]    スクリプト日付: 10/14/2009 15:53:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_UserAll] AS
SELECT
	PU.UserID,
	U.AD_AccountName AS AccountName,
	LTRIM(RTRIM(ISNULL(U.AD_GivenName, '') + ' ' + ISNULL(U.AD_Surname, ''))) AS Name,
	U.AD_Surname AS Surname,
	U.AD_GivenName AS GivenName,
	PU.RoleCode,
	PU.PrivilegeLevel,
	U.R3ID,
	PU.R3PurchasingGroup,
	U.AD_Email AS Email,
	PU.isAdmin,
	PU.isDisabled, 
	U.LocationCode,
	L.Name AS LocationName,
	U.AD_DeptName,
	U.AD_DisplayName,
	PU.UpdateDate
FROM
	PurchasingUser AS PU,
	s_User AS U,
	s_Location AS L
WHERE
	PU.UserID = U.UserID
	AND U.LocationCode = L.LocationCode

GO
