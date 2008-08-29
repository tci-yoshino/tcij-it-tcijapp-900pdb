/****** オブジェクト:  View [dbo].[v_User]    スクリプト日付: 08/28/2008 13:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_User] AS
SELECT
	PU.UserID,
	U.AD_AccountName AS AccountName,
	U.AD_GivenName + ' ' + U.AD_Surname AS Name,
	PU.RoleCode,
	PU.PrivilegeLevel,
	U.R3ID,
	PU.R3PurchasingGroup,
	U.AD_Email AS Email,
	PU.isDisabled, 
	U.LocationCode,
	L.Name AS LocationName
FROM
	PurchasingUser AS PU,
	s_User AS U,
	s_Location AS L
WHERE
	PU.isAdmin = 0
	AND PU.UserID = U.UserID
	AND U.LocationCode = L.LocationCode

GO
