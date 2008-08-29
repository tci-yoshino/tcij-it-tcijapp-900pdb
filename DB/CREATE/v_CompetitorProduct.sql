/****** オブジェクト:  View [dbo].[v_CompetitorProduct]    スクリプト日付: 08/28/2008 13:42:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[v_CompetitorProduct] AS
SELECT
	CP.SCode AS ProductNumber,
	CP.Z3 AS CASNumber,
	SUM(CP.ALDRICH) AS ALDRICH,
	SUM(CP.ALFA) AS ALFA,
	SUM(CP.WAKO) AS WAKO,
	SUM(CP.KANTO) AS KANTO,
	SUM(CP.ACROS) AS ACROS
FROM
(
	SELECT
		SCode,
		Z3,
		1 AS ALDRICH,
		0 AS ALFA,
		0 AS WAKO,
		0 AS KANTO,
		0 AS ACROS
	FROM
		s_CompetitorPrice
	WHERE
		LEFT(LTRIM(STR(Z1)),1) = '1'
	UNION
	SELECT
		SCode,
		Z3,
		0 AS ALDRICH,
		1 AS ALFA,
		0 AS WAKO,
		0 AS KANTO,
		0 AS ACROS
	FROM
		s_CompetitorPrice
	WHERE
		LEFT(LTRIM(STR(Z1)),1) = '2'
	UNION
	SELECT
		SCode,
		Z3,
		0 AS ALDRICH,
		0 AS ALFA,
		1 AS WAKO,
		0 AS KANTO,
		0 AS ACROS
	FROM
		s_CompetitorPrice
	WHERE
		LEFT(LTRIM(STR(Z1)),1) = '3'
	UNION
	SELECT
		SCode,
		Z3,
		0 AS ALDRICH,
		0 AS ALFA,
		0 AS WAKO,
		1 AS KANTO,
		0 AS ACROS
	FROM
		s_CompetitorPrice
	WHERE
		LEFT(LTRIM(STR(Z1)),1) = '4'
	UNION
	SELECT
		SCode,
		Z3,
		0 AS ALDRICH,
		0 AS ALFA,
		0 AS WAKO,
		0 AS KANTO,
		1 AS ACROS
	FROM
		s_CompetitorPrice
	WHERE
		LEFT(LTRIM(STR(Z1)),1) = '5'
) AS CP
GROUP BY
	CP.SCode,
	CP.Z3
GO
